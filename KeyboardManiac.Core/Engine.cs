using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

using KeyboardManiac.Sdk;
using KeyboardManiac.Sdk.Search;

using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

namespace KeyboardManiac.Core
{
    /// <summary>
    /// A standard implementation of <see cref="IEngine"/>.
    /// </summary>
    public class Engine : EngineBase
    {
        private const string DefaultLogFile = @"KeyboardManiac.default.log";
        private const string Log4netConfigFilename = @"Config\KeyboardManiac.log4net.xml";
        private const int SearchThreadStopInterval = 100;
        private const string SynchronousAliasPrefix = "-";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(Engine));
        private readonly List<string> m_CommandHistory = new List<string>();
        private int m_CommandHistoryPosition;
        private readonly List<ICommandPlugin> m_CommandPlugins = new List<ICommandPlugin>();
        private readonly GlobalHotKey m_HotKey;
        private readonly List<IPlugin> m_Plugins = new List<IPlugin>();
        private readonly SynchronizedList<SearchResultItem> m_Results = new SynchronizedList<SearchResultItem>();
        private readonly List<ISearchPlugin> m_SearchPlugins = new List<ISearchPlugin>();
        private SearchThread m_SearchThread;


        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        /// <param name="host">The object hosting this engine instance.</param>
        public Engine(IEngineHost host)
            : base(host)
        {
            m_HotKey = new GlobalHotKey(host);
        }

        public bool HotkeysEnabled
        {
            get { return m_HotKey.IsRegistered; }
            set
            {
                if (m_HotKey.IsRegistered != value)
                {
                    if (value)
                    {
                        m_HotKey.Register();
                    }
                    else
                    {
                        m_HotKey.Unregister();
                    }
                }
            }
        }

        override public GlobalHotKey HotKey { get { return m_HotKey; } }

        override public void ClearPlugins()
        {
            m_CommandPlugins.Clear();
            m_Plugins.Clear();
            m_SearchPlugins.Clear();
        }

        /// <summary>
        /// Initialises logging by loading the default log config file.
        /// </summary>
        override public void InitialiseLogging()
        {
            string filename = Path.Combine(
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                Log4netConfigFilename);
            InitialiseLogging(filename);
        }

        /// <summary>
        /// Initialises logging by loading the specified log config file.
        /// </summary>
        /// <param name="configFilename">
        /// The name of the file to load the logging configuration from.
        /// </param>
        override public void InitialiseLogging(string configFilename)
        {
            try
            {
                if (File.Exists(configFilename))
                {
                    XmlConfigurator.ConfigureAndWatch(new FileInfo(configFilename));
                }
                else
                {
                    InitialiseDefaultLogging();
                    Logger.WarnFormat(
                        "Failed to load log4net config file: {0}, default config used.",
                        configFilename);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed initialising logging", ex);
            }
        }

        private void InitialiseDefaultLogging()
        {
            SimpleLayout layout = new SimpleLayout();
            layout.ActivateOptions();

            FileAppender appender = new FileAppender();
            appender.File = Path.Combine(
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                DefaultLogFile);
            appender.ImmediateFlush = true;
            appender.Layout = layout;
            appender.ActivateOptions();

            BasicConfigurator.Configure(appender);
        }

        /// <summary>
        /// Registers a command plugin with the engine.
        /// </summary>
        /// <param name="plugin">The plugin to register.</param>
        override public void RegisterPlugin(ICommandPlugin plugin)
        {
            m_CommandPlugins.Add(plugin);
            m_Plugins.Add(plugin);
            Logger.DebugFormat("Command plugin registered: {0}", plugin);
        }

        /// <summary>
        /// Registers a search plugin with the engine.
        /// </summary>
        /// <param name="plugin">The plugin to register.</param>
        override public void RegisterPlugin(ISearchPlugin plugin)
        {
            plugin.ResultsFound += plugin_ResultsFound;
            m_SearchPlugins.Add(plugin);
            m_Plugins.Add(plugin);
            Logger.DebugFormat("Search plugin registered: {0}", plugin);
        }

        private void plugin_ResultsFound(object sender, ItemEventArgs<List<SearchResultItem>> e)
        {
            foreach (var result in e.Item)
            {
                if (m_CommandHistory.Contains(result.Path))
                {
                    result.Score += 100;
                }
            }

            m_Results.AddRange(e.Item);

            List<SearchResultItem> results = CalculateResults();
            OnResultsFound(new ItemEventArgs<List<SearchResultItem>>(results));
        }

        private List<SearchResultItem> CalculateResults()
        {
            List<SearchResultItem> results = m_Results.ToList();
            results.Sort((left, right) => right.Score.CompareTo(left.Score));
            return new List<SearchResultItem>(results.Take(10));
        }

        /// <summary>
        /// Runs the specified command text.
        /// </summary>
        /// <param name="commandText">The command text to run.</param>
        override public void RunCommand(string commandText)
        {
            if (commandText.StartsWith(SynchronousAliasPrefix))
            {
                string trimmedCommandText = commandText.Substring(SynchronousAliasPrefix.Length);
                ParseCommand(trimmedCommandText);
            }
            else
            {
                Thread thread = new Thread(ThreadRunCommand);
                thread.IsBackground = true;
                thread.Name = "KeyboardManiacCommandThread-" + thread.ManagedThreadId;
                thread.Start(commandText);
            }
        }

        /// <summary>
        /// Runs the specified item.
        /// </summary>
        /// <param name="item">The item to run.</param>
        override public void RunCommand(SearchResultItem item)
        {
            RunCommand(item.Path);
        }

        private void ThreadRunCommand(object argument)
        {
            string commandText = (string)argument;
            ParseCommand(commandText);
        }

        private void ParseCommand(string commandText)
        {
            CommandResult result = null;

            Logger.DebugFormat("Checking {0} command plugin(s) for handler of command: {1}", m_CommandPlugins.Count, commandText);
            foreach (ICommandPlugin plugin in m_CommandPlugins)
            {
                CommandRequest request = plugin.CanHandleCommand(commandText);
                if (request.CanHandleCommand)
                {
                    CancelSearch();
                    SetStatus("Running command: {0}", plugin.Name);
                    result = plugin.Execute(request);                
                    break;
                }
                else
                {
                    Logger.DebugFormat("Command plugin {0} cannot handle command: {1}", plugin, commandText);
                }
            }

            if (result == null)
            {
                SetStatus("Command not found");
                result = new CommandResult();
                result.Success = false;
            }

            m_CommandHistory.Add(commandText);
            m_CommandHistoryPosition = m_CommandHistory.Count - 1;

            OnCommandComplete(new ItemEventArgs<CommandResult>(result));
        }
        
        private void SetStatus(string format, params object[] args)
        {
            SetStatus(string.Format(format, args));
        }
        
        private void SetStatus(string message)
        {
            Logger.DebugFormat("Status changed to " + message);
            OnStatusChanged(new ItemEventArgs<string>(message));
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();

            m_HotKey.Dispose();

            foreach (ICommandPlugin commandPlugin in m_CommandPlugins)
            {
                IDisposable disposable = commandPlugin as IDisposable;
                if (disposable != null) disposable.Dispose();
            }
        }

        /// <summary>
        /// Gets the previous command text.
        /// </summary>
        /// <returns>The previous command text.</returns>
        override public string GetCommandHistoryPrevious()
        {
            m_CommandHistoryPosition--;
            if (m_CommandHistoryPosition < 0) m_CommandHistoryPosition = 0;

            string command;
            if (m_CommandHistory.Count > m_CommandHistoryPosition)
            {
                command = m_CommandHistory[m_CommandHistoryPosition];
            }
            else
            {
                command = string.Empty;
            }
            return command;
        }

        /// <summary>
        /// Starts a search.
        /// </summary>
        /// <param name="searchText">The text to search for.</param>
        override public void Search(string searchText)
        {
            CancelSearch();

            if (!string.IsNullOrEmpty(searchText))
            {
                m_Results.Clear();

                m_SearchThread = new SearchThread(
                    m_Plugins, 
                    new CommandRequest { CommandText = searchText, AliasCleansedCommandText = searchText });
                m_SearchThread.Name = "SearchThread-" + m_SearchThread.ManagedThreadId;
                m_SearchThread.Started += SearchThreadStartedHandler;
                m_SearchThread.Stopped += SearchThreadStoppedHandler;
                m_SearchThread.Start();
            }
        }

        private void CancelSearch()
        {
            //foreach (ISearchPlugin plugin in m_SearchPlugins)
            //{
            //    plugin.Stop();
            //}

            if (m_SearchThread != null)
            {
                m_SearchThread.Stopped -= SearchThreadStoppedHandler;
                if (m_SearchThread.IsAlive)
                {
                    m_SearchThread.Join(SearchThreadStopInterval, true, true);
                }
            }
        }

        private void SearchThreadStartedHandler(object sender, EventArgs e)
        {
            OnSearchStarted(e);
        }

        private void SearchThreadStoppedHandler(object sender, EventArgs e)
        {
            OnSearchComplete(new EventArgs());
        }

        override public void WndProc(int messageId)
        {
            if (messageId == GlobalHotKey.Constants.WM_HOTKEY_MSG_ID)
            {
                HandleHotkey();
            }
        }

        private void HandleHotkey()
        {
            EngineHost.ToggleShown();
        }

        #region IPluginHost members
        /// <summary>
        /// Gets a global setting as a <see cref="Boolean"/>.
        /// </summary>
        /// <param name="settingName">The name of the setting to get.</param>
        /// <param name="settingValue">
        /// Returns the value of the setting or a null reference if it was not found.
        /// </param>
        /// <returns>True if the setting was found; false otherwise.</returns>
        override public bool TryGetGlobalSettingAsBoolean(string settingName, out bool? settingValue)
        {
            string settingValueString;
            bool settingFound = TryGetGlobalSettingAsString(settingName, out settingValueString);
            settingValue = settingFound
                ? Convert.ToBoolean(settingValueString)
                : (bool?)null;
            return settingFound;
        }
        /// <summary>
        /// Gets a global setting as an <see cref="Int32"/>.
        /// </summary>
        /// <param name="settingName">The name of the setting to get.</param>
        /// <param name="settingValue">
        /// Returns the value of the setting or a null reference if it was not found.
        /// </param>
        /// <returns>True if the setting was found; false otherwise.</returns>
        override public bool TryGetGlobalSettingAsInt32(string settingName, out int? settingValue)
        {
            string settingValueString;
            bool settingFound = TryGetGlobalSettingAsString(settingName, out settingValueString);
            settingValue = settingFound
                ? Convert.ToInt32(settingValueString)
                : (int?)null;
            return settingFound;
        }
        /// <summary>
        /// Gets a global setting as a <see cref="String"/>.
        /// </summary>
        /// <param name="settingName">The name of the setting to get.</param>
        /// <param name="settingValue">
        /// Returns the value of the setting or the types default value if it was not found.
        /// </param>
        /// <returns>True if the setting was found; false otherwise.</returns>
        override public bool TryGetGlobalSettingAsString(string settingName, out string settingValue)
        {
            bool settingFound = Settings.Global.TryGetValue(settingName, out settingValue);
            return settingFound;
        }
        #endregion
    }  
}

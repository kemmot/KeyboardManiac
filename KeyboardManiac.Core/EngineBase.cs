using System;
using System.Collections.Generic;

using KeyboardManiac.Core.Config;
using KeyboardManiac.Sdk;
using KeyboardManiac.Sdk.Search;

namespace KeyboardManiac.Core
{
    /// <summary>
    /// A base class implementation of <see cref="IEngine"/> that provides
    /// common functionality.
    /// </summary>
    abstract public class EngineBase : DisposableBase, IEngine, IPluginHost
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineBase"/> class.
        /// </summary>
        /// <param name="host">The object hosting this engine instance.</param>
        protected EngineBase(IEngineHost host)
        {
            EngineHost = host;
        }


        #region IEngine members
        /// <summary>
        /// Raised when a command execution is complete.
        /// </summary>
        public event EventHandler<ItemEventArgs<CommandResult>> CommandComplete;
        /// <summary>
        /// Raised when search results are found.
        /// </summary>
        public event EventHandler<ItemEventArgs<List<SearchResultItem>>> ResultsFound;
        /// <summary>
        /// Raised when a search is complete.
        /// </summary>
        public event EventHandler SearchComplete;
        /// <summary>
        /// Raised when a search is started.
        /// </summary>
        public event EventHandler SearchStarted;
        /// <summary>
        /// Raised when the engine status changes.
        /// </summary>
        public event EventHandler<ItemEventArgs<string>> StatusChanged;

        abstract public GlobalHotKey HotKey { get; }
        /// <summary>
        /// Gets the settings in use by this engine.
        /// Loaded during the <see cref="Initialise"/> method.
        /// </summary>
        public ApplicationDetails Settings { get; set; }

        abstract public void ClearPlugins();
        /// <summary>
        /// Gets the previous command text.
        /// </summary>
        /// <returns>The previous command text.</returns>
        abstract public string GetCommandHistoryPrevious();
        /// <summary>
        /// Initialises logging by loading the default log config file.
        /// </summary>
        abstract public void InitialiseLogging();
        /// <summary>
        /// Initialises logging by loading the specified log config file.
        /// </summary>
        /// <param name="configFilename">
        /// The name of the file to load the logging configuration from.
        /// </param>
        abstract public void InitialiseLogging(string configFilename);
        /// <summary>
        /// Registers a command plugin with the engine.
        /// </summary>
        /// <param name="plugin">The plugin to register.</param>
        abstract public void RegisterPlugin(ICommandPlugin plugin);
        /// <summary>
        /// Registers a search plugin with the engine.
        /// </summary>
        /// <param name="plugin">The plugin to register.</param>
        abstract public void RegisterPlugin(ISearchPlugin plugin);
        /// <summary>
        /// Runs the specified command text.
        /// </summary>
        /// <param name="commandText">The command text to run.</param>
        abstract public void RunCommand(string commandText);
        /// <summary>
        /// Runs the specified item.
        /// </summary>
        /// <param name="item">The item to run.</param>
        abstract public void RunCommand(SearchResultItem item);
        /// <summary>
        /// Starts a search.
        /// </summary>
        /// <param name="searchText">The text to search for.</param>
        abstract public void Search(string searchText);
        /// <summary>
        /// Raises the <see cref="CommandComplete"/> event.
        /// </summary>
        /// <param name="e">The arguments to raise the event with.</param>
        virtual protected void OnCommandComplete(ItemEventArgs<CommandResult> e)
        {
            if (CommandComplete != null) CommandComplete(this, e);
        }
        /// <summary>
        /// Raises the <see cref="SearchComplete"/> event.
        /// </summary>
        /// <param name="e">The arguments to raise the event with.</param>
        virtual protected void OnSearchComplete(EventArgs e)
        {
            if (SearchComplete != null) SearchComplete(this, e);
        }
        /// <summary>
        /// Raises the <see cref="SearchStarted"/> event.
        /// </summary>
        /// <param name="e">The arguments to raise the event with.</param>
        virtual protected void OnSearchStarted(EventArgs e)
        {
            if (SearchStarted != null) SearchStarted(this, e);
        }
        /// <summary>
        /// Raises the <see cref="ResultsFound"/> event.
        /// </summary>
        /// <param name="e">The arguments to raise the event with.</param>
        virtual protected void OnResultsFound(ItemEventArgs<List<SearchResultItem>> e)
        {
            if (ResultsFound != null) ResultsFound(this, e);
        }
        /// <summary>
        /// Raises the <see cref="StatusChanged"/> event.
        /// </summary>
        /// <param name="e">The arguments to raise the event with.</param>
        virtual protected void OnStatusChanged(ItemEventArgs<string> e)
        {
            if (StatusChanged != null) StatusChanged(this, e);
        }
        abstract public void WndProc(int messageId);
        #endregion

        #region IPluginHost members
        /// <summary>
        /// Gets the engine host.
        /// </summary>
        public IEngineHost EngineHost { get; private set; }

        /// <summary>
        /// Gets a global setting as a <see cref="Boolean"/>.
        /// </summary>
        /// <param name="settingName">The name of the setting to get.</param>
        /// <param name="settingValue">
        /// Returns the value of the setting or a null reference if it was not found.
        /// </param>
        /// <returns>True if the setting was found; false otherwise.</returns>
        abstract public bool TryGetGlobalSettingAsBoolean(string settingName, out bool? settingValue);
        /// <summary>
        /// Gets a global setting as an <see cref="Int32"/>.
        /// </summary>
        /// <param name="settingName">The name of the setting to get.</param>
        /// <param name="settingValue">
        /// Returns the value of the setting or a null reference if it was not found.
        /// </param>
        /// <returns>True if the setting was found; false otherwise.</returns>
        abstract public bool TryGetGlobalSettingAsInt32(string settingName, out int? settingValue);
        /// <summary>
        /// Gets a global setting as a <see cref="String"/>.
        /// </summary>
        /// <param name="settingName">The name of the setting to get.</param>
        /// <param name="settingValue">
        /// Returns the value of the setting or the types default value if it was not found.
        /// </param>
        /// <returns>True if the setting was found; false otherwise.</returns>
        abstract public bool TryGetGlobalSettingAsString(string settingName, out string settingValue);
        #endregion
    }
}

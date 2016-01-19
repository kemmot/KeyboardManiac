using System;
using System.Collections.Generic;

using KeyboardManiac.Sdk;
using KeyboardManiac.Sdk.Search;

using log4net;

namespace KeyboardManiac.Core.Config
{
    public abstract class EngineConfiguratorBase : IEngineConfigurator
    {
        /// <summary>
        /// Raised when the status changes.
        /// </summary>
        public event EventHandler<ItemEventArgs<string>> StatusChanged;

        private static readonly ILog Logger = LogManager.GetLogger(typeof(EngineConfiguratorBase));
        private readonly IEngine m_Engine;

        protected EngineConfiguratorBase(IEngine engine)
        {
            if (engine == null) throw new ArgumentNullException("engine");

            m_Engine = engine;
        }

        /// <summary>
        /// Configures the engine and watches the file for changes prompting a reinitialisation if it does.
        /// </summary>
        /// <remarks>Settings will be loaded from the default file.</remarks>
        public abstract void ConfigureAndWatch();

        /// <summary>
        /// Configures the engine and watches the file for changes prompting a reinitialisation if it does.
        /// </summary>
        /// <param name="path">The name of the file to load the settings from.</param>
        public abstract void ConfigureAndWatch(string path);

        /// <summary>
        /// Configures the engine.
        /// </summary>
        /// <param name="host">The object hosting this engine instance.</param>
        /// <remarks>Settings will be loaded from the default file.</remarks>
        public abstract void Configure();

        /// <summary>
        /// Configures the engine and watches the file for changes prompting a reinitialisation if it does.
        /// </summary>
        /// <param name="path">The name of the file to load the settings from.</param>
        public abstract void Configure(string path);

        /// <summary>
        /// Configures the engine.
        /// </summary>
        /// <param name="settings">The settings to initialise the engine with.</param>
        public void Configure(ApplicationDetails settings)
        {
            m_Engine.Settings = settings;
            InitialisePlugins(settings);
            InitialiseHotkeys(settings);
            SetStatus("Engine initialised");
        }

        private void InitialisePlugins(ApplicationDetails settings)
        {
            SetStatus("Loading plugins...");
            m_Engine.ClearPlugins();
            for (int pluginCounter = 0; pluginCounter < settings.Plugins.Count; pluginCounter++)
            {
                PluginDetails pluginDetails = settings.Plugins[pluginCounter];
                string context = string.Format(
                    "Plugin {0}/{1}: {2}",
                    pluginCounter + 1,
                    settings.Plugins.Count,
                    pluginDetails);
                using (ThreadContext.Stacks["NDC"].Push(context))
                {
                    SetStatus(
                        "Loading plugin {0}/{1}: {2}",
                        pluginCounter + 1,
                        settings.Plugins.Count,
                        pluginDetails);

                    try
                    {
                        InitialisePlugin(settings, pluginDetails);
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorFormat("Failed initialising plugin, {0}", ex);
                    }
                }
            }
        }

        private void InitialisePlugin(ApplicationDetails settings, PluginDetails pluginDetails)
        {
            Type pluginType = GetPluginType(settings, pluginDetails);
            Logger.Debug("Found plugin type: " + pluginType.FullName);
            IPlugin plugin = GetPluginInstance(pluginType);
            Logger.Debug("Created plugin instance");

            plugin.Name = string.IsNullOrEmpty(pluginDetails.Name)
                ? plugin.GetType().Name
                : pluginDetails.Name;

            foreach (var alias in pluginDetails.Aliases)
            {
                plugin.RegisterAlias(alias.Name);
            }
            
            Dictionary<string, string> pluginSettings = new Dictionary<string, string>();
            foreach (Setting pluginSetting in pluginDetails.Settings)
            {
                pluginSettings[pluginSetting.Name] = pluginSetting.Value;
                Logger.DebugFormat("Available property, {0} = {1} (scope: Plugin)", pluginSetting.Name, pluginSetting.Value);
            }

            int pluginSettingsAdded = pluginSettings.Count;
            int globalSettingsAdded = 0;

            foreach (Setting globalSetting in settings.Global.Settings)
            {
                if (!pluginSettings.ContainsKey(globalSetting.Name))
                {
                    pluginSettings[globalSetting.Name] = globalSetting.Value;
                    Logger.DebugFormat("Available property, {0} = {1} (scope: Global)", globalSetting.Name, globalSetting.Value);
                }
            }

            Logger.DebugFormat(
                "{0} available properties, {1} at Plugin scope and {2} at Global scope", 
                pluginDetails.Settings.Count,
                pluginSettingsAdded,
                globalSettingsAdded);

            plugin.Initialise(pluginSettings);

            if (plugin is ICommandPlugin)
            {
                ICommandPlugin commandPlugin = (ICommandPlugin)plugin;
                m_Engine.RegisterPlugin(commandPlugin);
            }
            else if (plugin is ISearchPlugin)
            {
                ISearchPlugin searchPlugin = (ISearchPlugin)plugin;
                m_Engine.RegisterPlugin(searchPlugin);
            }
            else
            {
                Logger.WarnFormat(
                    "Plugin loaded of unknown plugin type: {0}",
                    pluginDetails);
            }
        }

        private Type GetPluginType(ApplicationDetails settings, PluginDetails pluginDetails)
        {
            Type pluginType;
            try
            {
                PluginTypeDetails pluginTypeDetails;
                if (!settings.TryGetPluginType(pluginDetails.PluginTypeId, out pluginTypeDetails))
                {
                    throw new Exception("Plugin type could not be found: " + pluginDetails.PluginTypeId);
                }

                pluginType = Type.GetType(pluginTypeDetails.ClassName, true, true);
            }
            catch (Exception ex)
            {
                throw new SettingsException("Plugin type could not be loaded", ex);
            }

            return pluginType;
        }

        private IPlugin GetPluginInstance(Type pluginType)
        {
            object pluginObject;
            try
            {
                pluginObject = Activator.CreateInstance(pluginType, m_Engine);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed creating instance of plugin", ex);
            }

            IPlugin plugin = pluginObject as IPlugin;
            if (plugin == null)
            {
                throw new InvalidCastException("Plugin does not implement IPlugin: " + pluginType.FullName);
            }

            return plugin;
        }

        private void InitialiseHotkeys(ApplicationDetails settings)
        {
            SetStatus("Registering global hotkeys...");
            if (m_Engine.HotKey.IsRegistered)
            {
                m_Engine.HotKey.Unregister();
            }

            m_Engine.HotKey.Clear();
            foreach (var hotkey in settings.HotKeys)
            {
                m_Engine.HotKey.Add(hotkey);
            }

            m_Engine.HotKey.Register();
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
        /// <summary>
        /// Raises the <see cref="StatusChanged"/> event.
        /// </summary>
        /// <param name="e">The arguments to raise the event with.</param>
        virtual protected void OnStatusChanged(ItemEventArgs<string> e)
        {
            if (StatusChanged != null) StatusChanged(this, e);
        }
    }
}

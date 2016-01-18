using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeyboardManiac.Sdk;
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
        public void Configure(KeyboardManiacSettings settings)
        {
            m_Engine.Settings = settings;
            InitialisePlugins(settings);
            InitialiseHotkeys(settings);
            SetStatus("Engine initialised");
        }
        private void InitialisePlugins(KeyboardManiacSettings settings)
        {
            SetStatus("Loading plugins...");
            m_Engine.ClearPlugins();
            for (int pluginCounter = 0; pluginCounter < settings.Plugins.Length; pluginCounter++)
            {
                KeyboardManiacSettingsPlugin pluginDetails = settings.Plugins[pluginCounter];
                string context = string.Format(
                    "Plugin {0}/{1}: {2}",
                    pluginCounter + 1,
                    settings.Plugins.Length,
                    pluginDetails);
                using (ThreadContext.Stacks["NDC"].Push(context))
                {
                    SetStatus(
                        "Loading plugin {0}/{1}: {2}",
                        pluginCounter + 1,
                        settings.Plugins.Length,
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
        private void InitialisePlugin(KeyboardManiacSettings settings, KeyboardManiacSettingsPlugin pluginDetails)
        {
            Type pluginType;
            try
            {
                KeyboardManiacSettingsPluginType pluginTypeDetails = settings.GetPluginType(pluginDetails.typeId);
                pluginType = Type.GetType(pluginTypeDetails.className, true, true);
            }
            catch (Exception ex)
            {
                throw new SettingsException("Plugin type could not be loaded", ex);
            }

            object pluginObject;
            try
            {
                pluginObject = Activator.CreateInstance(pluginType, m_Engine);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed creating instance of plugin", ex);
            }

            IPlugin plugin = (IPlugin)pluginObject;
            plugin.Name = string.IsNullOrEmpty(pluginDetails.name)
                ? pluginObject.GetType().Name
                : pluginDetails.name;
            plugin.Initialise(pluginDetails.ConfigurationNode);

            if (plugin is ICommandPlugin)
            {
                m_Engine.RegisterPlugin((ICommandPlugin)plugin);
            }
            else if (plugin is ISearchPlugin)
            {
                m_Engine.RegisterPlugin((ISearchPlugin)plugin);
            }
            else
            {
                Logger.WarnFormat(
                    "Plugin loaded of unknown plugin type: {0}",
                    pluginDetails);
            }
        }
        private void InitialiseHotkeys(KeyboardManiacSettings settings)
        {
            SetStatus("Registering global hotkeys...");
            if (m_Engine.HotKey.IsRegistered)
            {
                m_Engine.HotKey.Unregister();
            }

            m_Engine.HotKey.Clear();
            foreach (var hotkey in settings.Hotkeys)
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

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

using log4net;

namespace KeyboardManiac.Sdk
{
    /// <summary>
    /// A base class implementation of <see cref="IPlugin"/> providing
    /// functionality common to all plugins.
    /// </summary>
    abstract public class PluginBase : DisposableBase, IPlugin
    {
        private readonly static ILog Logger = LogManager.GetLogger(typeof(PluginBase));
        private readonly List<string> m_Aliases = new List<string>();
        private readonly IPluginHost m_Host;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginBase"/> class.
        /// </summary>
        protected PluginBase(IPluginHost host)
        {
            if (host == null) throw new ArgumentNullException("host");

            m_Host = host;
        }

        /// <summary>
        /// Gets the host for this plugin.
        /// </summary>
        protected IPluginHost Host { get { return m_Host; } }
        /// <summary>
        /// Gets or sets the name of this plugin.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gives the plugin an oportunity to handle the command text.
        /// </summary>
        /// <param name="commandText">The command text to assess.</param>
        /// <returns>The command request.</returns>
        virtual public CommandRequest CanHandleCommand(string commandText)
        {
            string upperCommandText = commandText.ToUpper();

            bool canHandleCommand = false;
            string matchingAlias = string.Empty;
            foreach (string alias in m_Aliases)
            {
                if (upperCommandText.StartsWith(alias))
                {
                    canHandleCommand = true;
                    matchingAlias = alias;
                    break;
                }
            }

            string aliasCleansedCommandText;
            if (canHandleCommand)
            {
                aliasCleansedCommandText = commandText.Substring(matchingAlias.Length).Trim();
            }
            else
            {
                aliasCleansedCommandText = commandText;
            }

            CommandRequest result = new CommandRequest();
            result.AliasCleansedCommandText = aliasCleansedCommandText;
            result.CanHandleCommand = canHandleCommand;
            result.CommandText = commandText;
            result.MatchingAlias = matchingAlias;
            return result;
        }
        /// <summary>
        /// Allows a plugin to register an alias.
        /// </summary>
        /// <param name="alias">The alias to register.</param>
        protected void RegisterAlias(string alias)
        {
            m_Aliases.Add(alias);
        }
        /// <summary>
        /// Allows this plugin to initialise itself.
        /// </summary>
        /// <param name="node">The plugin specific setting node.</param>
        public void Initialise(XmlNode node)
        {
            DoInitialiseAliases(node);
            //DoInitialiseSettings(node);
            InitialisePluginSettings(node);
            DoInitialise(node);
            PostInitialiseCheck();
        }
        /// <summary>
        /// Parses standard alias nodes from the plugin configuration node.
        /// </summary>
        /// <param name="node">The node to parse.</param>
        virtual protected void DoInitialiseAliases(XmlNode node)
        {
            foreach (XmlNode aliasNode in node.SelectNodes("Alias"))
            {
                string aliasName = aliasNode.Attributes["name"].Value;
                RegisterAlias(aliasName);
            }
        }
        private void InitialisePluginSettings(XmlNode node)
        {
            Dictionary<string, string> pluginSettings = new Dictionary<string, string>();
            foreach (XmlNode settingNode in node.SelectNodes("Setting"))
            {
                string settingName = settingNode.Attributes["key"].Value;
                string settingValue = settingNode.Attributes["value"].Value;
                pluginSettings[settingName] = settingValue;
            }

            foreach (PropertyInfo property in GetType().GetProperties())
            {
                string settingValueString = GetPluginSettingValue(property.Name, pluginSettings);
                if (!string.IsNullOrEmpty(settingValueString))
                {
                    SetPluginProperty(property, settingValueString);
                }
                else if (property.Name != "Name")
                {
                    Logger.WarnFormat("Setting value not found for property: {0}", property.Name);
                }
            }

            foreach (string settingName in pluginSettings.Keys)
            {
                string settingValue = pluginSettings[settingName];
                if (DoInitialiseSetting(settingName, settingValue))
                {
                    Logger.DebugFormat("Setting value set {0}={1}", settingName, settingValue);
                }
                else
                {
                    Logger.WarnFormat("Setting value not handled: {0}={1}", settingName, settingValue);
                }
            }
        }
        private string GetPluginSettingValue(string name, Dictionary<string, string> pluginSettings)
        {
            string settingValueString;
            if (pluginSettings.TryGetValue(name, out settingValueString))
            {
                pluginSettings.Remove(name);
            }
            else if (!Host.TryGetGlobalSettingAsString(name, out settingValueString))
            {
                settingValueString = null;
            }
            return settingValueString;
        }
        private void SetPluginProperty(PropertyInfo property, string settingValueString)
        {
            try
            {
                if (property.PropertyType == typeof(string))
                {
                    property.SetValue(this, settingValueString, null);
                    Logger.DebugFormat("Setting value set {0}={1}", property.Name, settingValueString);
                }
                else if (property.PropertyType == typeof(int))
                {
                    int settingValueInt = Convert.ToInt32(settingValueString);
                    property.SetValue(this, settingValueInt, null);
                    Logger.DebugFormat("Setting value set {0}={1}", property.Name, settingValueInt);
                }
                else if (property.PropertyType == typeof(bool))
                {
                    bool settingValueBool = Convert.ToBoolean(settingValueString);
                    property.SetValue(this, settingValueBool, null);
                    Logger.DebugFormat("Setting value set {0}={1}", property.Name, settingValueBool);
                }
                else
                {
                    Logger.WarnFormat("Setting type not supported: {0}", property);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(
                    "Failed setting property: {0}={1}, {2}",
                    property,
                    settingValueString,
                    ex);
            }
        }
        /// <summary>
        /// Allows a plugin to handle parsed settings.
        /// </summary>
        /// <param name="settingName">The name of the setting to handle.</param>
        /// <param name="settingValue">The value of the setting to handle.</param>
        /// <returns>True if the setting was handled; false otherwise.</returns>
        virtual protected bool DoInitialiseSetting(string settingName, string settingValue)
        {
            return false;
        }
        /// <summary>
        /// Allows this plugin to initialise itself.
        /// </summary>
        /// <param name="node">The plugin specific setting node.</param>
        virtual protected void DoInitialise(XmlNode node)
        {
            // do nothing
        }
        /// <summary>
        /// Allows a plugin to run post initialisation checks.
        /// </summary>
        /// <remarks>
        /// Any exception thrown from this method will prevent the plugin from being used.
        /// </remarks>
        virtual protected void PostInitialiseCheck()
        {
            // do nothing
        }
        /// <summary>
        /// Returns this object as a string.
        /// </summary>
        /// <returns>A string representing this object.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}

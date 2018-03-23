using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

using log4net;

namespace KeyboardManiac.Core.Config
{
    /// <summary>
    /// An implementation of <see cref="ISettingsSerialiser"/> that uses
    /// an XML file.
    /// </summary>
    public class XmlSettingsSerialiser : SettingsSerialiserBase
    {
        private readonly static ILog Logger = LogManager.GetLogger(typeof(XmlSettingsSerialiser));
        
        /// <summary>
        /// Loads settings from the specified file.
        /// </summary>
        /// <param name="filename">The file to load the settings from.</param>
        /// <returns>The loaded settings.</returns>
        override public ApplicationDetails Load(string filename)
        {
            ApplicationDetails settings = new ApplicationDetails();
            if (File.Exists(filename))
            {
                Logger.InfoFormat("Settings file loaded from file: {0}", filename);
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                ParseGlobalSettings(doc, settings);
                ParseGuiNode(doc, settings);
                ParseHotKeyNodes(doc, settings);
                ParsePluginTypes(doc, settings);
                ParsePlugins(doc, settings);
            }
            else
            {
                Logger.WarnFormat("Settings file not found so using defaults: {0}", filename);
            }
            return settings;
        }

        private void ParseGlobalSettings(XmlDocument doc, ApplicationDetails settings)
        {
            ParseSettings(doc.SelectSingleNode("KeyboardManiac/Global"), settings.Global, SettingScopes.Global);
        }

        private static void ParseGuiNode(XmlDocument doc, ApplicationDetails settings)
        {
            ParseSettings(doc.SelectSingleNode("KeyboardManiac/Gui"), settings.Gui, SettingScopes.Gui);
        }

        private static void ParseHotKeyNodes(XmlDocument doc, ApplicationDetails settings)
        {
            try
            {
                foreach (XmlNode hotkeyNode in doc.SelectNodes("KeyboardManiac/HotKeys/HotKey"))
                {
                    settings.HotKeys.Add(ParseHotKeyNode(hotkeyNode));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse hot keys", ex);
            }
        }

        private static HotKeyDetails ParseHotKeyNode(XmlNode hotkeyNode)
        {
            try
            {
                Keys key = (Keys)Enum.Parse(typeof(Keys), hotkeyNode.Attributes["key"].Value);
                
                string modifierString;
                int modifierCode;
                var modifierAttribute = hotkeyNode.SelectSingleNode("@modifier");
                if (modifierAttribute != null)
                {
                    modifierString = modifierAttribute.Value;
                    modifierCode = GlobalHotKey.Constants.GetCode(modifierString);
                }
                else
                {
                    modifierString = "[None]";
                    modifierCode = 0;
                }
                
                Logger.DebugFormat("Parsed hot key, key: {0}, modifier: {1} ({2})", key, modifierString, modifierCode);
                return new HotKeyDetails(key, modifierCode);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse hot key: " + hotkeyNode, ex);
            }
        }

        private void ParsePluginTypes(XmlDocument doc, ApplicationDetails settings)
        {
            try
            {
                var pluginTypes = new List<string>();
                foreach (XmlNode pluginTypeNode in doc.SelectNodes("KeyboardManiac/PluginTypes/PluginType"))
                {
                    PluginTypeDetails pluginType = ParsePluginTypeNode(pluginTypeNode);
                    if (pluginTypes.Contains(pluginType.Id))
                    {
                        string message = string.Format(
                            "Plugin type already registered: {0}, class name: {1}",
                            pluginType.Id,
                            pluginType.ClassName);
                        throw new SettingsException(message);
                    }

                    settings.PluginTypes.Add(pluginType);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse plugin types", ex);
            }
        }

        private PluginTypeDetails ParsePluginTypeNode(XmlNode node)
        {
            try
            {
                string id = node.Attributes["id"].Value;
                string className = node.Attributes["class"].Value;
                Logger.DebugFormat("Parsed plugin type, id: {0}, class: {1}", id, className);
                return new PluginTypeDetails { Id = id, ClassName = className };
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse plugin type: " + node, ex);
            }
        }

        private void ParsePlugins(XmlDocument doc, ApplicationDetails settings)
        {
            try
            {
                foreach (XmlNode pluginNode in doc.SelectNodes("KeyboardManiac/Plugins/Plugin"))
                {
                    settings.Plugins.Add(ParsePluginNode(pluginNode, settings));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse plugins", ex);
            }
        }

        private PluginDetails ParsePluginNode(XmlNode node, ApplicationDetails settings)
        {
            PluginDetails pluginDetails;
            try
            {
                string pluginTypeId = node.Attributes["typeId"].Value;
                XmlAttribute nameNode = node.Attributes["name"];
                string name = nameNode == null ? string.Empty : nameNode.Value;
                pluginDetails = new PluginDetails { PluginTypeId = pluginTypeId, Name = name };
                ParseSettings(node, pluginDetails, SettingScopes.Plugin);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse plugin: " + node, ex);
            }

            return pluginDetails;
        }

        private static void ParseSettings(XmlNode node, SettingsCollection settings, string scope)
        {
            foreach (XmlNode settingNode in node.SelectNodes("Setting"))
            {
                try
                {
                    string settingName = settingNode.Attributes["key"].Value;
                    string settingValue = settingNode.Attributes["value"].Value;
                    settings.Set(settingName, settingValue, scope);
                    Logger.DebugFormat("Parsed {0} setting. {1} = {2}", scope, settingName, settingValue);
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat("Failed to parse {0} setting: {1}, {2}", scope, settingNode, ex);
                }
            }
        }
    }
}

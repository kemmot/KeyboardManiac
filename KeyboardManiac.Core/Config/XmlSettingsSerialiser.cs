//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Reflection;
//using System.Windows.Forms;
//using System.Xml;

//using log4net;

//namespace KeyboardManiac.Core.Config
//{
//    /// <summary>
//    /// An implementation of <see cref="ISettingsSerialiser"/> that uses
//    /// an XML file.
//    /// </summary>
//    public class XmlSettingsSerialiser : SettingsSerialiserBase
//    {
//        private readonly static ILog Logger = LogManager.GetLogger(typeof(XmlSettingsSerialiser));
//        private readonly Dictionary<int, PluginTypeDetails> m_PluginTypes = new Dictionary<int, PluginTypeDetails>();

//        /// <summary>
//        /// Loads settings from the specified file.
//        /// </summary>
//        /// <param name="location">The file to load the settings from.</param>
//        /// <returns>The loaded settings.</returns>
//        override public Settings Load(string filename)
//        {
//            Settings settings = new Settings();
//            if (File.Exists(filename))
//            {
//                XmlDocument doc = new XmlDocument();
//                doc.Load(filename);
//                ParseGuiNode(doc, settings);
//                ParseHotkeyNodes(doc, settings);
//                ParsePluginTypes(doc, settings);
//                ParsePlugins(doc, settings);
//                ParseGlobalSettings(doc, settings);
//            }
//            else
//            {
//                Logger.WarnFormat("Settings file not found so using defaults: {0}", filename);
//            }
//            return settings;
//        }

//        private static void ParseGuiNode(XmlDocument doc, Settings settings)
//        {
//            foreach (XmlNode settingNode in doc.SelectNodes("KeyboardManiac/Gui/Setting"))
//            {
//                try
//                {
//                    string settingName = settingNode.Attributes["key"].Value;
//                    string settingValue = settingNode.Attributes["value"].Value;

//                    switch (settingName)
//                    {
//                        case "MinimiseOnLosingFocus":
//                            settings.Gui.MinimiseOnLosingFocus = bool.Parse(settingValue);
//                            break;
//                        case "MinimiseToSystemTray":
//                            settings.Gui.MinimiseToSystemTray = bool.Parse(settingValue);
//                            break;
//                        case "UseClipboardForCommandText":
//                            settings.Gui.UseClipboardForCommandText = bool.Parse(settingValue);
//                            break;
//                        default:
//                            Logger.WarnFormat(
//                                "Unknown setting node found, key: {0}, value: {1}",
//                                settingName,
//                                settingValue);
//                            break;
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Logger.ErrorFormat("Failed to parse setting: {0}, {1}", settingNode, ex);
//                }
//            }
//        }

//        private static void ParseHotkeyNodes(XmlDocument doc, Settings settings)
//        {
//            foreach (XmlNode hotkeyNode in doc.SelectNodes("KeyboardManiac/Hotkeys/Hotkey"))
//            {
//                settings.Hotkeys.Add(ParseHotkeyNode(hotkeyNode));
//            }
//        }

//        private static HotKey ParseHotkeyNode(XmlNode hotkeyNode)
//        {
//            Keys key = (Keys)Enum.Parse(typeof(Keys), hotkeyNode.Attributes["key"].Value);
//            int modifier = GlobalHotKey.Constants.GetCode(hotkeyNode.Attributes["modifier"].Value);
//            return new HotKey(key, modifier);
//        }

//        private void ParsePluginTypes(XmlDocument doc, Settings settings)
//        {
//            foreach (XmlNode pluginTypeNode in doc.SelectNodes("KeyboardManiac/PluginTypes/PluginType"))
//            {
//                PluginTypeDetails pluginType = ParsePluginTypeNode(pluginTypeNode);
//                if (m_PluginTypes.ContainsKey(pluginType.Id))
//                {
//                    string message = string.Format(
//                        "Plugin type already registered: {0}, class name: {1}",
//                        pluginType.Id,
//                        pluginType.ClassName);
//                    throw new SettingsException(message);
//                }

//                m_PluginTypes.Add(pluginType.Id, pluginType);
//            }
//        }

//        private PluginTypeDetails ParsePluginTypeNode(XmlNode node)
//        {
//            int id = Convert.ToInt32(node.Attributes["id"].Value);
//            string className = node.Attributes["class"].Value;

//            return new PluginTypeDetails { Id = id, ClassName = className };
//        }

//        private void ParsePlugins(XmlDocument doc, Settings settings)
//        {
//            foreach (XmlNode pluginNode in doc.SelectNodes("KeyboardManiac/Plugins/Plugin"))
//            {
//                settings.Plugins.Add(ParsePluginNode(pluginNode, settings));
//            }
//        }

//        private PluginDetails ParsePluginNode(XmlNode node, Settings settings)
//        {
//            int id = Convert.ToInt32(node.Attributes["typeId"].Value);
//            XmlAttribute nameNode = node.Attributes["name"];
//            string name = nameNode == null ? string.Empty : nameNode.Value;

//            PluginTypeDetails pluginType;
//            if (!m_PluginTypes.TryGetValue(id, out pluginType))
//            {
//                string message = string.Format(
//                    "Plugin type not registered: {0}",
//                    id);
//                throw new SettingsException(message);
//            }

//            return new PluginDetails { PluginType = pluginType, ConfigurationNode = node, Name = name };
//        }

//        private void ParseGlobalSettings(XmlDocument doc, Settings settings)
//        {
//            foreach (XmlNode globalSettingNode in doc.SelectNodes("KeyboardManiac/Engine/GlobalSetting"))
//            {
//                ParseGlobalSettingNode(globalSettingNode, settings.GlobalSettings);
//            }
//        }

//        private void ParseGlobalSettingNode(XmlNode node, IDictionary<string, string> globalSettings)
//        {
//            string settingName = node.Attributes["key"].Value;
//            string settingValue = node.Attributes["value"].Value;
//            globalSettings[settingName] = settingValue;
//        }
//    }
//}

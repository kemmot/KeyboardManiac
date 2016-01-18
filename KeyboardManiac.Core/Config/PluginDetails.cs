//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml;

//namespace KeyboardManiac.Core.Config
//{
//    /// <summary>
//    /// Contains the initialisation details for a plugin.
//    /// </summary>
//    partial class KeyboardManiacSettingsPlugin
//    {
//        /// <summary>
//        /// Gets or sets the configuration node of this plugin.
//        /// </summary>
//        public XmlNode ConfigurationNode { get; set; }
//        /// <summary>
//        /// Gets or sets the name of this plugin instance.
//        /// </summary>
//        public string Name { get; set; }
//        /// <summary>
//        /// Gets or sets the type of this plugin.
//        /// </summary>
//        public PluginTypeDetails PluginType { get; set; }

//        /// <summary>
//        /// Returns this object as a string.
//        /// </summary>
//        /// <returns>A string representing this object.</returns>
//        public override string ToString()
//        {
//            string result;
//            if (string.IsNullOrEmpty(Name))
//            {
//                result = PluginType == null ? "[null]" : PluginType.ToString();
//            }
//            else
//            {
//                result = Name;
//            }
//            return result;
//        }
//    }
//}

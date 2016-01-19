using System.Collections.Generic;

namespace KeyboardManiac.Core.Config
{
    /// <summary>
    /// Contains the initialisation details for a plugin.
    /// </summary>
    public class PluginDetails : SettingsCollection
    {
        private readonly List<AliasDetails> m_Aliases = new List<AliasDetails>();

        public IList<AliasDetails> Aliases { get { return m_Aliases; } }

        /// <summary>
        /// Gets or sets the name of this plugin instance.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of this plugin.
        /// </summary>
        public string PluginTypeId { get; set; }

        /// <summary>
        /// Returns this object as a string.
        /// </summary>
        /// <returns>A string representing this object.</returns>
        public override string ToString()
        {
            string result;
            if (string.IsNullOrEmpty(Name))
            {
                result = PluginTypeId ?? "[null]";
            }
            else
            {
                result = Name;
            }
            return result;
        }
    }
}

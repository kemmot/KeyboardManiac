namespace KeyboardManiac.Core.Config
{
    /// <summary>
    /// Contains the initialisation details for a plugin type.
    /// </summary>
    public class PluginTypeDetails
    {
        /// <summary>
        /// Gets or sets the plugin type ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the class containing the plugin type.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Returns this object as a string.
        /// </summary>
        /// <returns>A string representing this object.</returns>
        public override string ToString()
        {
            return string.Format(
                "Plugin type, id: {0}, class: {1}",
                Id,
                TypeName.ParseFromAssemblyQualifiedName(ClassName).ClassName);
        }
    }
}

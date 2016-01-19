namespace KeyboardManiac.Core.Config
{
    /// <summary>
    /// A base class implementation of <see cref="ISettingsSerialiser"/>
    /// providing common functionality.
    /// </summary>
    abstract public class SettingsSerialiserBase : ISettingsSerialiser
    {
        /// <summary>
        /// Loads settings from the specified location.
        /// </summary>
        /// <param name="location">The location to load the settings from.</param>
        /// <returns>The loaded settings.</returns>
        abstract public ApplicationDetails Load(string filename);
    }
}

namespace KeyboardManiac.Core.Config
{
    /// <summary>
    /// The interface that must be implemented to provide settings serialisation.
    /// </summary>
    public interface ISettingsSerialiser
    {
        /// <summary>
        /// Loads settings from the specified location.
        /// </summary>
        /// <param name="location">The location to load the settings from.</param>
        /// <returns>The loaded settings.</returns>
        ApplicationDetails Load(string location);
    }
}

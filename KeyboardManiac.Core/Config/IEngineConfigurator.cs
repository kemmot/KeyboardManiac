namespace KeyboardManiac.Core.Config
{
    /// <summary>
    /// The interface to implement to provide engine configuration functionality.
    /// </summary>
    public interface IEngineConfigurator
    {
        /// <summary>
        /// Configures the engine and watches the file for changes prompting a reinitialisation if it does.
        /// </summary>
        /// <remarks>Settings will be loaded from the default file.</remarks>
        void ConfigureAndWatch();
        /// <summary>
        /// Configures the engine and watches the file for changes prompting a reinitialisation if it does.
        /// </summary>
        /// <param name="path">The name of the file to load the settings from.</param>
        void ConfigureAndWatch(string path);
        /// <summary>
        /// Configures the engine.
        /// </summary>
        /// <param name="host">The object hosting this engine instance.</param>
        /// <remarks>Settings will be loaded from the default file.</remarks>
        void Configure();
        /// <summary>
        /// Configures the engine and watches the file for changes prompting a reinitialisation if it does.
        /// </summary>
        /// <param name="path">The name of the file to load the settings from.</param>
        void Configure(string path);
        /// <summary>
        /// Configures the engine.
        /// </summary>
        /// <param name="settings">The settings to initialise the engine with.</param>
        void Configure(KeyboardManiacSettings settings);
    }
}

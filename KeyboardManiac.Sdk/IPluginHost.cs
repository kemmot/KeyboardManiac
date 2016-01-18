using System;

namespace KeyboardManiac.Sdk
{
    /// <summary>
    /// The interface that must be implemented to provide plugin host functionality.
    /// </summary>
    public interface IPluginHost
    {
        /// <summary>
        /// Gets the engine host.
        /// </summary>
        IEngineHost EngineHost { get; }

        /// <summary>
        /// Gets a global setting as a <see cref="Boolean"/>.
        /// </summary>
        /// <param name="settingName">The name of the setting to get.</param>
        /// <param name="settingValue">
        /// Returns the value of the setting or a null reference if it was not found.
        /// </param>
        /// <returns>True if the setting was found; false otherwise.</returns>
        bool TryGetGlobalSettingAsBoolean(string settingName, out bool? settingValue);
        /// <summary>
        /// Gets a global setting as an <see cref="Int32"/>.
        /// </summary>
        /// <param name="settingName">The name of the setting to get.</param>
        /// <param name="settingValue">
        /// Returns the value of the setting or a null reference if it was not found.
        /// </param>
        /// <returns>True if the setting was found; false otherwise.</returns>
        bool TryGetGlobalSettingAsInt32(string settingName, out int? settingValue);
        /// <summary>
        /// Gets a global setting as a <see cref="String"/>.
        /// </summary>
        /// <param name="settingName">The name of the setting to get.</param>
        /// <param name="settingValue">
        /// Returns the value of the setting or the types default value if it was not found.
        /// </param>
        /// <returns>True if the setting was found; false otherwise.</returns>
        bool TryGetGlobalSettingAsString(string settingName, out string settingValue);
    }
}

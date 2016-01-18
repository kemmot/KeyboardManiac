using System;

namespace KeyboardManiac.Sdk
{
    /// <summary>
    /// An exception class that indicates a problem with a plugin.
    /// </summary>
    [Serializable]
    public class PluginException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="PluginException"/> class.
        /// </summary>
        public PluginException()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="PluginException"/> class.
        /// </summary>#
        /// <param name="message">The exception message.</param>
        public PluginException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="PluginException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public PluginException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

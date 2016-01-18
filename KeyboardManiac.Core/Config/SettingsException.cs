using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyboardManiac.Core.Config
{
    /// <summary>
    /// An exception class that indicates a problem with settings.
    /// </summary>
    [Serializable]
    public class SettingsException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SettingsException"/> class.
        /// </summary>
        public SettingsException()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SettingsException"/> class.
        /// </summary>#
        /// <param name="message">The exception message.</param>
        public SettingsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SettingsException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SettingsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

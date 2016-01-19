using System;

namespace KeyboardManiac.Sdk
{
    /// <summary>
    /// The results of a command execution.
    /// </summary>
    public class CommandResult
    {
        /// <summary>
        /// Gets or sets the command text that was executed.
        /// </summary>
        public string CommandText { get; set; }
        /// <summary>
        /// Gets or sets the command execution output.
        /// </summary>
        public string Output { get; set; }
        /// <summary>
        /// Gets or sets whether the command execution was successful.
        /// </summary>
        public bool Success { get; set; }
    }
}

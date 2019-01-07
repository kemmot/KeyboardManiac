using System;

using KeyboardManiac.Sdk.Search;

namespace KeyboardManiac.Sdk
{
    /// <summary>
    /// The interface that must be implemented to provide command plugins to the keyboard maniac engine.
    /// </summary>
    public interface ICommandPlugin : IPlugin
    {
        /// <summary>
        /// Executes the specified command text.
        /// </summary>
        /// <param name="commandRequest">The command request to execute.</param>
        /// <returns>The result of the execution.</returns>
        CommandResult Execute(CommandRequest commandRequest);
    }
}

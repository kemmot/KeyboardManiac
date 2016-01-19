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
        /// Gives the plugin an opportunity to handle the command text.
        /// </summary>
        /// <param name="item">The item to assess.</param>
        /// <returns>
        /// A result object with details on whether the plugin can handle the command.
        /// </returns>
        CommandRequest CanHandleCommand(SearchResultItem item);
        /// <summary>
        /// Executes the specified command text.
        /// </summary>
        /// <param name="commandRequest">The command request to execute.</param>
        /// <returns>The result of the execution.</returns>
        CommandResult Execute(CommandRequest commandRequest);
    }
}

using System;
using System.Collections.Generic;

using KeyboardManiac.Sdk.Search;

using log4net;

namespace KeyboardManiac.Sdk
{
    /// <summary>
    /// A base class implementation of <see cref="ICommandPlugin"/> providing
    /// functionality common to all command plugins.
    /// </summary>
    abstract public class CommandPluginBase : PluginBase, ICommandPlugin
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CommandPluginBase));
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandPluginBase"/> class.
        /// </summary>
        /// <param name="host">The host for this plugin.</param>
        protected CommandPluginBase(IPluginHost host)
            : base(host)
        {
        }

        /// <summary>
        /// Gives the plugin an opportunity to handle the command text.
        /// </summary>
        /// <param name="item">The item to assess.</param>
        /// <returns>
        /// A result object with details on whether the plugin can handle the command.
        /// </returns>
        virtual public CommandRequest CanHandleCommand(SearchResultItem item)
        {
            return CanHandleCommand(item.Path);
        }
        /// <summary>
        /// Executes the specified command text.
        /// </summary>
        /// <param name="commandRequest">The command to execute.</param>
        /// <returns>The result of the execution.</returns>
        public CommandResult Execute(CommandRequest commandRequest)
        {
            CommandResult result = DoExecute(commandRequest);
            Logger.DebugFormat("Executed command: {0}", commandRequest.AliasCleansedCommandText);
            return result;
        }
        /// <summary>
        /// Executes the specified command text.
        /// </summary>
        /// <param name="commandText">The command text to execute.</param>
        /// <returns>The result of the execution.</returns>
        abstract protected CommandResult DoExecute(CommandRequest commandRequest);
    }
}

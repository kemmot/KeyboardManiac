using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using KeyboardManiac.Sdk;

namespace KeyboardManiac.Plugins.ShellExecute
{
    public class ShellExecuteCommand : CommandPluginBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellExecuteCommand"/> class.
        /// </summary>
        /// <param name="host">The host for this plugin.</param>
        public ShellExecuteCommand(IPluginHost host)
            : base(host)
        {
        }

        /// <summary>
        /// Executes the specified command text.
        /// </summary>
        /// <param name="commandRequest">The command to execute.</param>
        /// <returns>The result of the execution.</returns>
        override protected CommandResult DoExecute(CommandRequest commandRequest)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = commandRequest.AliasCleansedCommandText;
            info.UseShellExecute = true;
            info.WorkingDirectory = Path.GetDirectoryName(commandRequest.AliasCleansedCommandText);
            Process.Start(info);

            return new CommandResult() { Success = true, CommandText = commandRequest.AliasCleansedCommandText };
        }
        /// <summary>
        /// Gives the plugin an oportunity to handle the command text.
        /// </summary>
        /// <param name="commandText">The command text to assess.</param>
        /// <returns>The command request.</returns>
        override public CommandRequest CanHandleCommand(string commandText)
        {
            bool canHandleCommand;
            string aliasCleansedCommandText;
            if (File.Exists(commandText))
            {
                canHandleCommand = true;
                aliasCleansedCommandText = commandText;
            }
            else if (Directory.Exists(commandText))
            {
                canHandleCommand = true;
                aliasCleansedCommandText = commandText;
            }
            else
            {
                canHandleCommand = false;
                aliasCleansedCommandText = commandText;
            }

            CommandRequest result = new CommandRequest();
            result.AliasCleansedCommandText = aliasCleansedCommandText;
            result.CanHandleCommand = canHandleCommand;
            result.CommandText = commandText;
            result.MatchingAlias = string.Empty;
            return result;
        }
    }
}

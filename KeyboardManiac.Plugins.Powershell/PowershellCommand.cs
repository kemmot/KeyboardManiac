using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Xml;

using KeyboardManiac.Sdk;

using log4net;

namespace KeyboardManiac.Plugins.Powershell
{
    public class PowershellCommand : CommandPluginBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PowershellCommand));
        private readonly Runspace m_Runspace;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowershellCommand"/> class.
        /// </summary>
        /// <param name="host">The host for this plugin.</param>
        public PowershellCommand(IPluginHost host)
            : base(host)
        {
            m_Runspace = RunspaceFactory.CreateRunspace();
            m_Runspace.Open();
            m_Runspace.SessionStateProxy.SetVariable("App", host);
        }
        
        /// <summary>
        /// Allows this plugin to initialise itself.
        /// </summary>
        /// <param name="node">The plugin specific setting node.</param>
        protected override void DoInitialise(XmlNode node)
        {
            base.DoInitialise(node);

            foreach (XmlNode scriptNode in node.SelectNodes("StartupScript"))
            {
                string scriptPath = scriptNode.Attributes["path"].Value;
                try
                {
                    Pipeline pipeline = m_Runspace.CreatePipeline();
                    pipeline.Commands.AddScript(System.IO.File.ReadAllText(scriptPath));
                    pipeline.Invoke();
                    Logger.DebugFormat("Successfully run start-up script: {0}", scriptPath);
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat("Failed running start-up script: {0}, {1}", scriptPath, ex);
                }
            }
        }
        /// <summary>
        /// Executes the specified command text.
        /// </summary>
        /// <param name="commandRequest">The command to execute.</param>
        /// <returns>The result of the execution.</returns>
        override protected CommandResult DoExecute(CommandRequest commandRequest)
        {
            CommandResult result = new CommandResult();
            result.CommandText = commandRequest.AliasCleansedCommandText;
            try
            {
                // create a pipeline and feed it the script text
                Pipeline pipeline = m_Runspace.CreatePipeline();
                pipeline.Commands.AddScript(commandRequest.AliasCleansedCommandText);

                // add an extra command to transform the script
                // output objects into nicely formatted strings
                // remove this line to get the actual objects
                // that the script returns. For example, the script
                // "Get-Process" returns a collection
                // of System.Diagnostics.Process instances.
                pipeline.Commands.Add("Out-String");

                // execute the script
                Collection<PSObject> results = pipeline.Invoke();
                
                // convert the script result into a single string
                StringBuilder output = new StringBuilder();
                foreach (PSObject obj in results)
                {
                    output.AppendLine(obj.ToString());
                }
                result.Output = output.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Output = ex.ToString();
                result.Success = false;                
            }

            return result;
        }
        /// <summary>
        /// Dispose managed resources.
        /// </summary>
        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();

            m_Runspace.Close();
        }
    }
}

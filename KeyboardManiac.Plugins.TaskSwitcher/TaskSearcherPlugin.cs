using System;
using System.Collections.Generic;
using System.Diagnostics;

using KeyboardManiac.Sdk;

namespace KeyboardManiac.Plugins.TaskSwitcher
{
    public class TaskSearcherPlugin : SearchPluginBase
    {
        private const string ResultType_Process = "Process";

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskSearcherPlugin"/> class.
        /// </summary>
        public TaskSearcherPlugin(IPluginHost host)
            : base(host)
        {
        }

        protected override void DoSearch(CommandRequest parameters)
        {
            List<SearchResultItem> applications = new List<SearchResultItem>();

            string upperSearchText = parameters.AliasCleansedCommandText.ToUpper();

            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                if (process.MainWindowTitle.Length > 1 && process.ProcessName.ToUpper().Contains(upperSearchText))
                {
                    applications.Add(new SearchResultItem(process.ProcessName, ResultType_Process, process.ProcessName));
                }
            }

            OnResultsFound(new ItemEventArgs<List<SearchResultItem>>(applications));
        }
    }
}

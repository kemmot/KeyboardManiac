using System;
using System.Collections.Generic;
using System.IO;

using KeyboardManiac.Sdk;

namespace KeyboardManiac.Plugins.FileSystemSearch
{
    public class FileSystemAbsoluteSearchPlugin : SearchPluginBase
    {
        protected const string ResultType_File = "File";

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemAbsoluteSearchPlugin"/> class.
        /// </summary>
        /// <param name="host">The host for this plugin.</param>
        public FileSystemAbsoluteSearchPlugin(IPluginHost host)
            : base(host)
        {
        }

        protected override void DoSearch(CommandRequest parameters)
        {
            if (!string.IsNullOrWhiteSpace(parameters.AliasCleansedCommandText))
            {
                string folder;
                string mask;
                bool folderExists = CanSearch(parameters.AliasCleansedCommandText, out folder, out mask);
                if (folderExists)
                {
                    List<string> files = new List<string>(string.IsNullOrEmpty(mask)
                        ? Directory.EnumerateFileSystemEntries(folder)
                        : Directory.EnumerateFileSystemEntries(folder, mask));
                    List<SearchResultItem> results = new List<SearchResultItem>();
                    foreach (string file in files)
                    {
                        string name = UseShortNames ? Path.GetFileName(file) : file;
                        results.Add(new SearchResultItem(name, ResultType_File, file));
                    }
                    OnResultsFound(new ItemEventArgs<List<SearchResultItem>>(results));
                }
            }
        }

        private bool CanSearch(string commandText, out string folder, out string mask)
        {
            folder = commandText;
            bool folderExists = Directory.Exists(folder);
            if (!folderExists)
            {
                try
                {
                    folder = Path.GetDirectoryName(folder);
                    folderExists = Directory.Exists(folder);
                    mask = "*" + Path.GetFileName(commandText) + "*";
                }
                catch (ArgumentException)
                {
                    folder = string.Empty;
                    folderExists = false;
                    mask = string.Empty;
                }
            }
            else
            {
                mask = string.Empty;
            }
            return folderExists;
        }
    }
}

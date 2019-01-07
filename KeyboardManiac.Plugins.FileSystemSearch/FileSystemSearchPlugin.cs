using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Xml;

using KeyboardManiac.Sdk;

using log4net;

namespace KeyboardManiac.Plugins.FileSystemSearch
{
    public class FileSystemSearchPlugin : FileSystemSearchPluginBase
    {
        private const string SettingNameFolder = "Folder";
        private readonly static ILog Logger = LogManager.GetLogger(typeof(FileSystemSearchPlugin));

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemSearchPlugin"/> class.
        /// </summary>
        /// <param name="host">The host for this plugin.</param>
        public FileSystemSearchPlugin(IPluginHost host)
            : base(host)
        {
        }

        /// <summary>
        /// Gives the plugin an opportunity to handle the command text.
        /// </summary>
        /// <param name="commandText">The command text to assess.</param>
        /// <returns>The command request.</returns>
        public override CommandRequest CanHandleCommand(string commandText)
        {
            CommandRequest request = base.CanHandleCommand(commandText);
            request.CanHandleCommand = true;
            return request;
        }
    }
}

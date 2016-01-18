using System;
using System.Collections.Generic;
using System.Xml;

using KeyboardManiac.Sdk;

using log4net;

namespace KeyboardManiac.Plugins.FileSystemSearch
{
    public class SpecialFolderSearchPlugin : FileSystemSearchPluginBase
    {
        private readonly static ILog Logger = LogManager.GetLogger(typeof(SpecialFolderSearchPlugin));

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialFolderSearchPlugin"/> class.
        /// </summary>
        public SpecialFolderSearchPlugin(IPluginHost host)
            : base(host)
        {
        }

        /// <summary>
        /// Gives the plugin an oportunity to handle the command text.
        /// </summary>
        /// <param name="commandText">The command text to assess.</param>
        /// <returns>The command request.</returns>
        public override CommandRequest CanHandleCommand(string commandText)
        {
            CommandRequest request = base.CanHandleCommand(commandText);
            request.CanHandleCommand = true;
            return request;
        }
        /// <summary>
        /// Allows a plugin to handle parsed settings.
        /// </summary>
        /// <param name="settingName">The name of the setting to handle.</param>
        /// <param name="settingValue">The value of the setting to handle.</param>
        override protected bool DoInitialiseSetting(string settingName, string settingValue)
        {
            bool initialised = base.DoInitialiseSetting(settingName, settingValue);
            if (!initialised)
            {
                switch (settingName)
                {
                    case "SpecialFolder":
                        string folder = string.Empty;
                        string[] specialFolders = settingValue.Split(';');
                        foreach (string specialFolder in specialFolders)
                        {
                            Environment.SpecialFolder specialFolderType;
                            if (!Enum.TryParse<Environment.SpecialFolder>(specialFolder, true, out specialFolderType))
                            {
                                string message = string.Format(
                                    "Failed parsing SpecialFolder type: {0}",
                                    specialFolder);
                                throw new Exception(message);
                            }
                            if (!string.IsNullOrEmpty(folder))
                            {
                                folder += ";";
                            }
                            folder += Environment.GetFolderPath(specialFolderType);
                        }
                        Folder = folder;
                        initialised = true;
                        break;
                }
            }
            return initialised;
        }
    }
}

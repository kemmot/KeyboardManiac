using System;
using System.IO;
using System.Reflection;
using System.Threading;

using KeyboardManiac.Sdk;

using log4net;

namespace KeyboardManiac.Core.Config
{
    public class XmlFileEngineConfigurator : EngineConfiguratorBase
    {
        private const string DefaultFilename = @"Config\KeyboardManiac.settings.xml";
        private const int MonitorInterval = 1000;

        private static readonly ILog Logger = LogManager.GetLogger(typeof(XmlFileEngineConfigurator));
        private DateTime m_ConfigFileLastModified;

        public XmlFileEngineConfigurator(IEngine engine)
            : base(engine)
        {
        }

        public string Filename { get; private set; }

        /// <summary>
        /// Configures the engine and watches the file for changes prompting a reinitialisation if it does.
        /// </summary>
        /// <remarks>Settings will be loaded from the default file.</remarks>
        public override void ConfigureAndWatch()
        {
            ConfigureAndWatch(GetDefaultFilename());
        }

        /// <summary>
        /// Configures the engine.
        /// </summary>
        /// <param name="host">The object hosting this engine instance.</param>
        /// <remarks>Settings will be loaded from the default file.</remarks>
        public override void Configure()
        {
            Configure(GetDefaultFilename());
        }

        private static string GetDefaultFilename()
        {
            return Path.Combine(
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                DefaultFilename);
        }

        /// <summary>
        /// Configures the engine and watches the file for changes prompting a reinitialisation if it does.
        /// </summary>
        /// <param name="filename">The name of the file to load the settings from.</param>
        public override void ConfigureAndWatch(string filename)
        {
            Configure(filename);
            if (File.Exists(filename))
            {
                Watch();
            }
        }

        private void Watch()
        {
            Thread monitorThread = new Thread(ThreadMonitorFile);
            monitorThread.IsBackground = true;
            monitorThread.Name = "ConfigurationMonitorThread";
            monitorThread.Start();
        }

        private void ThreadMonitorFile()
        {
            bool exit = false;
            Logger.InfoFormat("Starting watching settings file for changes: {0}", Filename);
            do
            {
                try
                {
                    Thread.Sleep(MonitorInterval);

                    if (File.GetLastWriteTime(Filename) > m_ConfigFileLastModified)
                    {
                        Logger.InfoFormat("Change detected in settings file: {0}", Filename);
                        Configure(Filename);
                    }
                }
                catch (ThreadAbortException)
                {
                    exit = true;
                    Thread.ResetAbort();
                }
                catch (Exception ex)
                {
                    Logger.Error("Error occurred monitoring settings file", ex);
                }
            }
            while (!exit);
            Logger.InfoFormat("Stopping watching settings file for changes: {0}", Filename);
        }

        /// <summary>
        /// Configures the engine and watches the file for changes prompting a reinitialisation if it does.
        /// </summary>
        /// <param name="filename">The name of the file to load the settings from.</param>
        public override void Configure(string filename)
        {
            Filename = filename;
            OnStatusChanged(new ItemEventArgs<string>(string.Format("Loading settings from {0}", filename)));

            ApplicationDetails settings = new XmlSettingsSerialiser().Load(filename);
            Configure(settings);
            m_ConfigFileLastModified = File.GetLastWriteTime(filename);
        }

        public override string ToString()
        {
            return string.Format("XML configurator: {0}", Filename);
        }
    }
}

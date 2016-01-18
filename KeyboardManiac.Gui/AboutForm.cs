using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using KeyboardManiac.Core.Config;
using KeyboardManiac.Sdk;

using log4net;

namespace KeyboardManiac.Gui
{
    public partial class AboutForm : Form
    {
        private const string ReleaseNotesResourceName = "KeyboardManiac.Gui.ReleaseNotes.txt";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(AboutForm));
        private readonly IEngineConfigurator m_Configurator;

        public AboutForm(IEngineConfigurator configurator)
            : this()
        {
            m_Configurator = configurator;
        }

        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            StringBuilder text = new StringBuilder();

            if (m_Configurator != null)
            {
                text.AppendLine("Settings loaded from: {0}", m_Configurator.ToString());
                text.AppendLine();
            }

            text.AppendLine(AppDomain.CurrentDomain.GetLoadedAssemblyText());
            text.AppendLine("Release Notes");
            text.AppendLine();
            text.AppendLine(LoadReleaseNotes());
            RtxtAbout.Text = text.ToString();
        }

        private string LoadReleaseNotes()
        {
            string releaseNotes;
            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ReleaseNotesResourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    releaseNotes = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                releaseNotes = "Problem loading release notes.";
                Logger.ErrorFormat(
                    "Failed loading release notes from resource: {0}, {1}",
                    ReleaseNotesResourceName,
                    ex);
            }           
            return releaseNotes;
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(RtxtAbout.Text);
            }
            catch (Exception ex)
            {
                HandleException(ex, "Failed to copy about text to clipboard");
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string filename = "[not yet set]";
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filename = saveFileDialog1.FileName;
                    File.WriteAllText(filename, RtxtAbout.Text);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "Failed saving about text to file: {0}", filename);
            }
        }

        private void HandleException(Exception ex, string format, params object[] args)
        {
            HandleException(ex, string.Format(format, args));
        }

        private void HandleException(Exception ex, string message)
        {
            MessageBox.Show(message);
            Logger.Error(message, ex);
        }
    }
}

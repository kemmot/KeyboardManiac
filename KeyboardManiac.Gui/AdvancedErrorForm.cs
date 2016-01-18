using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using log4net;

using KeyboardManiac.Sdk;

namespace KeyboardManiac.Gui
{
    public partial class AdvancedErrorForm : Form
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AdvancedErrorForm));
        private Exception m_Exception;

        public AdvancedErrorForm()
        {
            InitializeComponent();
        }

        public Exception Exception
        {
            get { return m_Exception; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                m_Exception = value;
                DisplayException();
            }
        }

        private void DisplayException()
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine(m_Exception.ToString());
            text.AppendLine();
            text.AppendLine(AppDomain.CurrentDomain.GetLoadedAssemblyText());
            RtxtException.Text = text.ToString();
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(RtxtException.Text);
            }
            catch (Exception ex)
            {
                HandleException(ex, "Failed to copy exception to clipboard");
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
                    File.WriteAllText(filename, RtxtException.Text);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "Failed saving exception to file: {0}", filename);
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

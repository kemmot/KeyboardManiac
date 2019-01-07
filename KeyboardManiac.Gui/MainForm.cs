using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using KeyboardManiac.Core;
using KeyboardManiac.Core.Config;
using KeyboardManiac.Sdk;
using KeyboardManiac.Sdk.Search;

using log4net;

namespace KeyboardManiac.Gui
{
    public partial class MainForm : Form, IEngineHost
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MainForm));
        private readonly IEngineConfigurator m_Configurator;
        private readonly IEngine m_Engine;
        private bool m_EnableHiding = true;
        private List<SearchResultItem> m_LastestResult;
        private bool m_SearchComplete;
        private readonly System.Timers.Timer m_UpdateResultsTimer = new System.Timers.Timer(1000);

        public MainForm()
        {
            InitializeComponent();

            m_Engine = new Engine(this);
            m_Configurator = new XmlFileEngineConfigurator(m_Engine);

            Thread thread = new Thread(ThreadInitialise);
            thread.IsBackground = true;
            thread.Name = "InitialiseThread-" + thread.ManagedThreadId;
            thread.Start();

            m_UpdateResultsTimer.AutoReset = false;
            m_UpdateResultsTimer.Elapsed += new System.Timers.ElapsedEventHandler(m_UpdateResultsTimer_Elapsed);
        }

        void m_UpdateResultsTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            List<SearchResultItem> lastestResult = m_LastestResult;

            ThreadStart del = delegate()
            {
                LvResults.BeginUpdate();
                try
                {
                    LvResults.Items.Clear();
                    if (lastestResult != null)
                    {
                        foreach (SearchResultItem item in lastestResult)
                        {
                            LvResults.Items.Add(new ResultListViewItem(item));
                        }
                    }

                    var resizeStyle = LvResults.Items.Count > 0
                            ? ColumnHeaderAutoResizeStyle.ColumnContent
                            : ColumnHeaderAutoResizeStyle.HeaderSize;
                    foreach (ColumnHeader column in LvResults.Columns)
                    {
                        column.AutoResize(resizeStyle);
                    }
                }
                finally
                {
                    LvResults.EndUpdate();
                }

                string status = m_SearchComplete ? "Search complete" : "Searching";
                SetStatus("{0}, {1} results found", status, LvResults.Items.Count);

                // ensure results are visible but also ensure focus remains on input box
                if (tabControl1.SelectedTab != TpResults)
                {
                    tabControl1.SelectedTab = TpResults;
                    CboInput.Focus();
                    CboInput.SelectionStart = CboInput.Text.Length;
                }
            };
            Invoke(del);
        }


        public IntPtr WindowHandle { get { return Handle; } }


        private void ThreadInitialise()
        {
            bool successful = false;
            try
            {
                SetStatus("Initialising logging...");
                m_Engine.InitialiseLogging();

                SetStatus("Initialising engine...");
                m_Engine.CommandComplete += m_Engine_CommandComplete;
                m_Engine.ResultsFound += m_Engine_ResultsFound;
                m_Engine.SearchComplete += m_Engine_SearchComplete;
                m_Engine.SearchStarted += m_Engine_SearchStarted;
                m_Engine.StatusChanged += m_Engine_StatusChanged;

                m_Configurator.ConfigureAndWatch();

                SetStatus("Ready");
                successful = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                successful = false;
            }
            finally
            {
                SetStatus("Initialised engine {0}", successful ? "successfully" : "unsuccessfully");
            }
        }

        private void m_Engine_SearchStarted(object sender, EventArgs e)
        {
            m_LastestResult = null;
            m_SearchComplete = false;
            ThreadStart del = delegate { SetStatus("Searching..."); };
            Invoke(del);
        }

        private void m_Engine_SearchComplete(object sender, EventArgs e)
        {
            m_SearchComplete = true;

            if (!m_UpdateResultsTimer.Enabled)
            {
                m_UpdateResultsTimer.Start();
            }
        }

        private void m_Engine_CommandComplete(object sender, ItemEventArgs<CommandResult> e)
        {
            ThreadStart del = delegate()
            {
                CommandResult result = e.Item;
                ShowOutput(result);
                if (result.Success)
                {
                    CboInput.Text = string.Empty;
                }
                CboInput.Focus();
                SetStatus("Done");
            };
            if (InvokeRequired) Invoke(del);
            else del();
        }

        private void m_Engine_ResultsFound(object sender, ItemEventArgs<List<SearchResultItem>> e)
        {
            m_LastestResult = e.Item;

            if (!m_UpdateResultsTimer.Enabled)
            {
                m_UpdateResultsTimer.Start();
            }
        }

        private void m_Engine_StatusChanged(object sender, ItemEventArgs<string> e)
        {
            SetStatus(e.Item);
        }

        private void SetStatus(string format, params object[] args)
        {
            SetStatus(string.Format(format, args));
        }

        private void SetStatus(string text)
        {
            ThreadStart del = delegate()
            {
                toolStripStatusLabel1.Text = text;
            };
            if (InvokeRequired) Invoke(del);
            else del();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Down:
                        if (LvResults.Items.Count > 0)
                        {
                            LvResults.SelectedIndices.Clear();
                            LvResults.SelectedIndices.Add(0);
                            LvResults.Focus();
                        }
                        break;
                    case Keys.Enter:
                        m_Engine.RunCommand(CboInput.Text);
                        break;
                    case Keys.Escape:                        
                        if (!CboInput.DroppedDown)
                        {
                            if (CboInput.Text == string.Empty)
                            {
                                WindowState = FormWindowState.Minimized;
                            }
                            else
                            {
                                CboInput.Text = string.Empty;
                            }
                        }
                        break;
                    case Keys.Up:
                        string previousCommand = m_Engine.GetCommandHistoryPrevious();
                        if (!string.IsNullOrEmpty(previousCommand))
                        {
                            CboInput.Text = previousCommand;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ShowOutput(CommandResult result)
        {
            string output = string.Format(
                "{0}{1}{2}",
                result.CommandText,
                Environment.NewLine,
                result.Output);
            ShowOutput(output);
        }

        private void ShowOutput(string text)
        {
            RtxtOutput.Text = text;
            if (tabControl1.SelectedTab != TpOutput)
            {
                tabControl1.SelectedTab = TpOutput;
            }
        }

        private void HandleException(Exception ex)
        {
            Logger.Error(ex);

            m_EnableHiding = false;
            try
            {
                SimpleErrorForm form = new SimpleErrorForm();
                form.Exception = ex;
                form.ShowDialog();
            }
            finally
            {
                m_EnableHiding = true;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m_Engine != null)
            {
                m_Engine.WndProc(m.Msg);
            }
            base.WndProc(ref m);
        }

        public void ToggleShown()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Open();
            }
            else
            {
                WindowState = FormWindowState.Minimized;
            }
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (m_EnableHiding
                && m_Engine.Settings.Gui.MinimiseOnLosingFocus)
            {
                WindowState = FormWindowState.Minimized;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized
                && m_Engine.Settings.Gui.MinimiseToSystemTray)     
            {
                Hide();
            } 
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                IDisposable disposable = m_Engine as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to dispose engine", ex);
            }
        }

        private void MiOpen_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void Open()
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void MiOptions_Click(object sender, EventArgs e)
        {
            MessageBox.Show("No settings dialog yet, please edit config file and restart app");
        }

        private void MiExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            LvResults.BeginUpdate();
            try
            {
                LvResults.Items.Clear();
            }
            finally
            {
                LvResults.EndUpdate();
            }

            m_Engine.Search(CboInput.Text);
        }

        private void LstResults_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (LvResults.SelectedItems.Count > 0)
                {
                    SearchResultItem item = ((ResultListViewItem)LvResults.SelectedItems[0]).Item;
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            m_Engine.RunCommand(item);
                            break;
                        case Keys.Right:
                            CboInput.Text = item.ToString();
                            CboInput.Focus();
                            break;
                        case Keys.Up:
                            if (LvResults.SelectedItems[0].Index == 0)
                            {
                                CboInput.Focus();
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            m_EnableHiding = false;
            try
            {
                new AboutForm(m_Configurator, m_Engine).ShowDialog();
            }
            finally
            {
                m_EnableHiding = true;
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            try
            {
                CboInput.Focus();
                if (m_Engine.Settings != null
                    && m_Engine.Settings.Gui.UseClipboardForCommandText)
                {
                    CboInput.Text = Clipboard.GetText();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }

    public class ResultListViewItem : ListViewItem
    {
        public ResultListViewItem(SearchResultItem result)
        {
            Item = result;
            Text = Item.ResultType;
            SubItems.Add(Item.Name);
            SubItems.Add(Item.Score.ToString());
            SubItems.Add(Item.Path);
        }

        public SearchResultItem Item { get; private set; }
    }
}

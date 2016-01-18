namespace KeyboardManiac.Gui
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.RtxtOutput = new System.Windows.Forms.RichTextBox();
            this.CboInput = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MiOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.MiOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.MiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TpResults = new System.Windows.Forms.TabPage();
            this.LvResults = new System.Windows.Forms.ListView();
            this.ChResultType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ChResultName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ChResultScore = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ChResultLocation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TpOutput = new System.Windows.Forms.TabPage();
            this.BtnAbout = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.TpResults.SuspendLayout();
            this.TpOutput.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RtxtOutput
            // 
            this.RtxtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RtxtOutput.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RtxtOutput.Location = new System.Drawing.Point(3, 3);
            this.RtxtOutput.Name = "RtxtOutput";
            this.RtxtOutput.Size = new System.Drawing.Size(506, 98);
            this.RtxtOutput.TabIndex = 1;
            this.RtxtOutput.Text = "";
            // 
            // CboInput
            // 
            this.CboInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CboInput.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllSystemSources;
            this.CboInput.FormattingEnabled = true;
            this.CboInput.Location = new System.Drawing.Point(4, 5);
            this.CboInput.Name = "CboInput";
            this.CboInput.Size = new System.Drawing.Size(485, 21);
            this.CboInput.TabIndex = 0;
            this.CboInput.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.CboInput.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
            this.CboInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox1_KeyDown);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 161);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(520, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Keyboard Maniac";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MiOpen,
            this.MiOptions,
            this.MiExit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(126, 70);
            // 
            // MiOpen
            // 
            this.MiOpen.Name = "MiOpen";
            this.MiOpen.Size = new System.Drawing.Size(125, 22);
            this.MiOpen.Text = "Open";
            this.MiOpen.Click += new System.EventHandler(this.MiOpen_Click);
            // 
            // MiOptions
            // 
            this.MiOptions.Name = "MiOptions";
            this.MiOptions.Size = new System.Drawing.Size(125, 22);
            this.MiOptions.Text = "Options...";
            this.MiOptions.Click += new System.EventHandler(this.MiOptions_Click);
            // 
            // MiExit
            // 
            this.MiExit.Name = "MiExit";
            this.MiExit.Size = new System.Drawing.Size(125, 22);
            this.MiExit.Text = "Exit";
            this.MiExit.Click += new System.EventHandler(this.MiExit_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TpResults);
            this.tabControl1.Controls.Add(this.TpOutput);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 31);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(520, 130);
            this.tabControl1.TabIndex = 3;
            // 
            // TpResults
            // 
            this.TpResults.Controls.Add(this.LvResults);
            this.TpResults.Location = new System.Drawing.Point(4, 22);
            this.TpResults.Name = "TpResults";
            this.TpResults.Padding = new System.Windows.Forms.Padding(3);
            this.TpResults.Size = new System.Drawing.Size(512, 104);
            this.TpResults.TabIndex = 0;
            this.TpResults.Text = "Results";
            this.TpResults.UseVisualStyleBackColor = true;
            // 
            // LvResults
            // 
            this.LvResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ChResultType,
            this.ChResultName,
            this.ChResultScore,
            this.ChResultLocation});
            this.LvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LvResults.FullRowSelect = true;
            this.LvResults.Location = new System.Drawing.Point(3, 3);
            this.LvResults.Name = "LvResults";
            this.LvResults.Size = new System.Drawing.Size(506, 98);
            this.LvResults.TabIndex = 0;
            this.LvResults.UseCompatibleStateImageBehavior = false;
            this.LvResults.View = System.Windows.Forms.View.Details;
            this.LvResults.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LstResults_KeyDown);
            // 
            // ChResultType
            // 
            this.ChResultType.Text = "Type";
            // 
            // ChResultName
            // 
            this.ChResultName.Text = "Name";
            // 
            // ChResultScore
            // 
            this.ChResultScore.Text = "Score";
            // 
            // ChResultLocation
            // 
            this.ChResultLocation.Text = "Location";
            // 
            // TpOutput
            // 
            this.TpOutput.Controls.Add(this.RtxtOutput);
            this.TpOutput.Location = new System.Drawing.Point(4, 22);
            this.TpOutput.Name = "TpOutput";
            this.TpOutput.Padding = new System.Windows.Forms.Padding(3);
            this.TpOutput.Size = new System.Drawing.Size(512, 104);
            this.TpOutput.TabIndex = 1;
            this.TpOutput.Text = "Output";
            this.TpOutput.UseVisualStyleBackColor = true;
            // 
            // BtnAbout
            // 
            this.BtnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAbout.Location = new System.Drawing.Point(495, 3);
            this.BtnAbout.Name = "BtnAbout";
            this.BtnAbout.Size = new System.Drawing.Size(22, 23);
            this.BtnAbout.TabIndex = 4;
            this.BtnAbout.Text = "?";
            this.BtnAbout.UseVisualStyleBackColor = true;
            this.BtnAbout.Click += new System.EventHandler(this.BtnAbout_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CboInput);
            this.panel1.Controls.Add(this.BtnAbout);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(520, 31);
            this.panel1.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 183);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Text = "Keyboard Maniac";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.TpResults.ResumeLayout(false);
            this.TpOutput.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox RtxtOutput;
        private System.Windows.Forms.ComboBox CboInput;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MiOpen;
        private System.Windows.Forms.ToolStripMenuItem MiOptions;
        private System.Windows.Forms.ToolStripMenuItem MiExit;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TpResults;
        private System.Windows.Forms.TabPage TpOutput;
        private System.Windows.Forms.Button BtnAbout;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView LvResults;
        private System.Windows.Forms.ColumnHeader ChResultType;
        private System.Windows.Forms.ColumnHeader ChResultName;
        private System.Windows.Forms.ColumnHeader ChResultLocation;
        private System.Windows.Forms.ColumnHeader ChResultScore;
    }
}


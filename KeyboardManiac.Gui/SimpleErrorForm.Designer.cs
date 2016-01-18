namespace KeyboardManiac.Gui
{
    partial class SimpleErrorForm
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
            this.BtnClose = new System.Windows.Forms.Button();
            this.BtnAdvanced = new System.Windows.Forms.Button();
            this.LblErrorMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtnClose.Location = new System.Drawing.Point(218, 59);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 23);
            this.BtnClose.TabIndex = 0;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            // 
            // BtnAdvanced
            // 
            this.BtnAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAdvanced.Location = new System.Drawing.Point(299, 59);
            this.BtnAdvanced.Name = "BtnAdvanced";
            this.BtnAdvanced.Size = new System.Drawing.Size(75, 23);
            this.BtnAdvanced.TabIndex = 1;
            this.BtnAdvanced.Text = "Advanced";
            this.BtnAdvanced.UseVisualStyleBackColor = true;
            this.BtnAdvanced.Click += new System.EventHandler(this.BtnAdvanced_Click);
            // 
            // LblErrorMessage
            // 
            this.LblErrorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblErrorMessage.Location = new System.Drawing.Point(12, 9);
            this.LblErrorMessage.Name = "LblErrorMessage";
            this.LblErrorMessage.Size = new System.Drawing.Size(362, 47);
            this.LblErrorMessage.TabIndex = 2;
            this.LblErrorMessage.Text = "Error message";
            // 
            // SimpleErrorForm
            // 
            this.AcceptButton = this.BtnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnClose;
            this.ClientSize = new System.Drawing.Size(386, 94);
            this.Controls.Add(this.LblErrorMessage);
            this.Controls.Add(this.BtnAdvanced);
            this.Controls.Add(this.BtnClose);
            this.Name = "SimpleErrorForm";
            this.Text = "Error Occurred";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Button BtnAdvanced;
        private System.Windows.Forms.Label LblErrorMessage;
    }
}
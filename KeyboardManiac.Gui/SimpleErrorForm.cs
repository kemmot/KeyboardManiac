using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeyboardManiac.Gui
{
    public partial class SimpleErrorForm : Form
    {
        private Exception m_Exception;

        public SimpleErrorForm()
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
                LblErrorMessage.Text = m_Exception.Message;
            }
        }

        private void BtnAdvanced_Click(object sender, EventArgs e)
        {
            AdvancedErrorForm form = new AdvancedErrorForm();
            form.Exception = Exception;
            form.ShowDialog();
        }
    }
}

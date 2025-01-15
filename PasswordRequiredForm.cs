using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThingImageLibrary
{
    public partial class PasswordRequiredForm : Form
    {
        public PasswordRequiredForm()
        {
            InitializeComponent();
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            if(textBoxPassword.Text.Length > 0)
            {
                buttonProceed.Enabled = true;
            }
            else
            {
                buttonProceed.Enabled = false;
            }
        }

        private void buttonProceed_Click(object sender, EventArgs e)
        {
            MainForm.password = textBoxPassword.Text;
            MainForm.passwordStatus = PasswordStatus.Set;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            MainForm.passwordStatus = PasswordStatus.Canceled;
            this.Close();
        }
    }
}

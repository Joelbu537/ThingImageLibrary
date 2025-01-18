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
    public partial class CreateKeyForm : Form
    {
        public CreateKeyForm()
        {
            InitializeComponent();
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            CheckEnable();
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            CheckEnable();
        }

        private void textBoxPasswordConfirm_TextChanged(object sender, EventArgs e)
        {
            CheckEnable();
        }
        private void CheckEnable()
        {
            if (textBoxName.Text.Length > 0 && textBoxPassword.Text.Length > 0 && textBoxPasswordConfirm.Text.Length > 0)
            {
                buttonOK.Enabled = true;
            }
            else
            {
                buttonOK.Enabled = false;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {

        }

        private void CreateKeyForm_Load(object sender, EventArgs e)
        {

        }

        private void checkBoxPwdRequired_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPwdRequired.Checked)
            {
                textBoxPassword.Enabled = true;
                textBoxPasswordConfirm.Enabled = true;
            }
            else
            {
                textBoxPassword.Enabled = false;
                textBoxPasswordConfirm.Enabled = false;
            }
        }
    }
}
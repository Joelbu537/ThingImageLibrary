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
            if (textBoxName.Text.Length > 0 && ((checkBoxPwdRequired.Checked && textBoxPassword.Text.Length > 6 && textBoxPasswordConfirm.Text.Length > 6 && textBoxPasswordConfirm.Text == textBoxPassword.Text) || (!checkBoxPwdRequired.Checked)) )
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
            byte[] keyBytes;
            TekKey key = new TekKey();
            if (checkBoxPwdRequired.Checked)
            {
                keyBytes = key.Generate(textBoxPassword.Text);
            }
            else
            {
                keyBytes = key.Generate();
            }
            SaveFileDialog saveKeyDialog = new SaveFileDialog
            {
                Filter = "TEK-Key (*.tek)|*.tek",
                Title = "Save your new TEK Key",
                FileName = textBoxName.Text + ".tek"
            };
            if (saveKeyDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveKeyDialog.FileName;
                System.IO.File.WriteAllBytes(filePath, keyBytes);
                MessageBox.Show("Key created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
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
            CheckEnable();
        }

        private void CreateKeyForm_Load(object sender, EventArgs e)
        {
            checkBoxPwdRequired.Checked = true;
            checkBoxPwdRequired_CheckedChanged(null, EventArgs.Empty);
        }
    }
}
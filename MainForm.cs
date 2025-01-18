using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThingImageLibrary
{
    public partial class MainForm : Form
    {
        public static PasswordStatus passwordStatus { get; set; }
        public static string password { get; set; }
        public MainForm()
        {
            InitializeComponent();
        }
        public MainForm(string[] a)
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string currentdirectory = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(currentdirectory, "*.tek");
            if(files.Length > 0)
            {
                labelKeyStatus.Text = "FOUND";
                labelKeyStatus.ForeColor = Color.Orange;
                DialogResult loadkey = MessageBox.Show("A .tek key was found in the current directory. Do you want to load it?", "Key found", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if(loadkey == DialogResult.Yes)
                {
                    TekKey key = new TekKey(files[0]);
                    if (key.IsPasswordProtected())
                    {
                        Application.Run(new PasswordRequiredForm());
                        if (passwordStatus == PasswordStatus.Set)
                        {
                            string _password = password;
                            password = System.String.Empty;
                            passwordStatus = PasswordStatus.Undefined;
                            bool keyBool = key.Load(_password);
                            if (!keyBool)
                            {
                                MessageBox.Show("Password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Application.Exit();
                            }
                        }
                    }
                    if (!key.IsPasswordProtected())
                    {
                        bool keyBool = key.Load();
                    }
                }
            }
        }

        private void listViewLibrary_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonLoadKey_Click(object sender, EventArgs e)
        {
            OpenFileDialog openKeyDialog = new OpenFileDialog
            {
                Filter = "TEK-Key (*.tek)|*.tek",
                Title = "Select a TEK Key"
            };

            if (openKeyDialog.ShowDialog() == DialogResult.OK)
            {
                string filepath = openKeyDialog.FileName;
            }
        }

        private void buttonGenerateNewKey_Click(object sender, EventArgs e)
        {

        }
    }
    public enum PasswordStatus
    {
        Canceled,
        Set,
        Undefined
    }
}

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
                TekKey key = new TekKey(files[0]);
                if (key.IsKeyProtected())
                {
                    Application.Run(new PasswordRequiredForm());
                    if (passwordStatus == PasswordStatus.Set)
                    {
                        string _password = password;
                        password = System.String.Empty;
                        passwordStatus = PasswordStatus.Undefined;
                        key.Load(_password);
                    }
                }
                if(!key.IsKeyProtected())
                {
                    key.Load();
                }
            }
        }
    }
    public enum PasswordStatus
    {
        Canceled,
        Set,
        Undefined
    }
}

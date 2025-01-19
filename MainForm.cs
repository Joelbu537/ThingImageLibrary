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
using Microsoft.Data.Sqlite;

namespace ThingImageLibrary
{
    public partial class MainForm : Form
    {
        public static PasswordStatus passwordStatus { get; set; }
        public static string password { get; set; }
        private static TekKey key = new TekKey();
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
                    try
                    {
                        bool success = false;
                        success = key.Load(files[0]);
                        if (success)
                        {
                            labelKeyStatus.Text = "LOADED";
                            labelKeyStatus.ForeColor = Color.Green;
                            buttonCreateLibrary.Enabled = true;
                            buttonLoadKey.Enabled = false;
                            buttonLoadLibrary.Enabled = true;
                        }
                    }
                    catch(TekKeyPasswordRequiredException p)
                    {
                        bool success = false;
                        do
                        {
                            PasswordRequiredForm passwordRequiredForm = new PasswordRequiredForm();
                            passwordRequiredForm.ShowDialog();
                            if (passwordStatus == PasswordStatus.Set)
                            {
                                string _password = password;
                                password = System.String.Empty;
                                passwordStatus = PasswordStatus.Undefined;
                                try
                                {
                                    success = key.Load(_password);
                                }
                                catch (TekKeyPasswordInvalidException ex)
                                {
                                    success = false;
                                }
                            }
                            else if(passwordStatus == PasswordStatus.Canceled)
                            {
                                break;
                            }
                        } while (!success);
                        if (success)
                        {
                            labelKeyStatus.Text = "LOADED";
                            labelKeyStatus.ForeColor = Color.Green;
                            buttonCreateLibrary.Enabled = true;
                            buttonLoadKey.Enabled = false;
                            buttonLoadLibrary.Enabled = true;
                        }
                        
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
                try
                {
                    key.Load(openKeyDialog.FileName);
                }
                catch (TekKeyPasswordRequiredException p)
                {
                    bool success = false;
                    do
                    {
                        PasswordRequiredForm passwordRequiredForm = new PasswordRequiredForm();
                        passwordRequiredForm.ShowDialog();
                        if (passwordStatus == PasswordStatus.Set)
                        {
                            string _password = password;
                            password = System.String.Empty;
                            passwordStatus = PasswordStatus.Undefined;
                            try
                            {
                                success = key.Load(openKeyDialog.FileName, _password);
                            }
                            catch (TekKeyPasswordInvalidException ex)
                            {
                                success = false;
                            }
                        }
                        else if (passwordStatus == PasswordStatus.Canceled)
                        {
                            break;
                        }
                    } while (!success);
                    if (success)
                    {
                        labelKeyStatus.Text = "LOADED";
                        labelKeyStatus.ForeColor = Color.Green;
                    }

                }
            }
        }

        private void buttonGenerateNewKey_Click(object sender, EventArgs e)
        {
            CreateKeyForm createKeyForm = new CreateKeyForm();
            createKeyForm.ShowDialog();
        }

        private void buttonLoadLibrary_Click(object sender, EventArgs e)
        {
            OpenFileDialog openKeyDialog = new OpenFileDialog
            {
                Filter = "TEK-Database (*.tekdb)|*.tekdb",
                Title = "Select a Library Database"
            };
            if (openKeyDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    MemoryStream tempStream = key.Decrpt(openKeyDialog.FileName).Result;
                    byte[] decryptedDatabase = tempStream.ToArray();
                    using (MemoryStream memoryStream = new MemoryStream(decryptedDatabase))
                    {
                        //Verbindung zu DB in RAM herstellen
                        var connectionString = "Data Source=file:memdb1?mode=memory&cache=shared";
                        using (var connection = new SqliteConnection(connectionString))
                        {
                            connection.Open();

                            //Daten in RAM laden
                            using (var command = connection.CreateCommand())
                            {
                                command.CommandText = "ATTACH DATABASE ':memory:' AS ramdb";
                                command.ExecuteNonQuery();

                                //Stream-Daten in SQLite kopieren
                                memoryStream.Seek(0, SeekOrigin.Begin);
                                var tempFilePath = Path.GetTempFileName(); //Temporäre Datei
                                File.WriteAllBytes(tempFilePath, decryptedDatabase); //Entschlüsselte Datenbank speichern
                                command.CommandText = $"ATTACH DATABASE '{tempFilePath}' AS tempdb";
                                command.ExecuteNonQuery();

                                //Beispieldaten abfragen
                                command.CommandText = "SELECT name FROM sqlite_master WHERE type='table'";
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Console.WriteLine($"Tabelle: {reader.GetString(0)}");
                                    }
                                }

                                // Temporäre Datei löschen
                                File.Delete(tempFilePath);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

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

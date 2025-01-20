using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        public static SqliteConnection DatabaseConnection { get; set; }
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
            string keypath;
            if (openKeyDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    bool success = false;
                    keypath = openKeyDialog.FileName;
                    success = key.Load(keypath);
                    if (success)
                    {
                        labelKeyStatus.Text = "LOADED";
                        labelKeyStatus.ForeColor = Color.Green;
                        buttonCreateLibrary.Enabled = true;
                        buttonLoadKey.Enabled = false;
                        buttonLoadLibrary.Enabled = true;
                    }
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
                        buttonCreateLibrary.Enabled = true;
                        buttonLoadKey.Enabled = false;
                        buttonLoadLibrary.Enabled = true;
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
            OpenFileDialog openLibraryDialog = new OpenFileDialog
            {
                Filter = "TEK-Database (*.tekdb)|*.tekdb",
                Title = "Select a Library"
            };
            if (openLibraryDialog.ShowDialog() != DialogResult.OK)
            {
                throw new InvalidTekKeyFormatException("No library selected");
            }

            MemoryStream tempStream = key.Decrpt(openLibraryDialog.FileName).Result;
            using (FileStream fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "library.db"), FileMode.Create, FileAccess.Write))
            {
                tempStream.WriteTo(fileStream);
            }


            //DB entschlüsseln und schreiben
            //DB lesen und backup in ram erstellen
            //DB verschlüsseln
            //Bei speichervorgang DB wieder entschlüsseln und speichern


            string persistentDbPath = $"Data Source=library.db;Version=3;";
            string memoryDbPath = "Data Source=:memory:;Version=3;";
            using (SqliteConnection persistentConnection = new SqliteConnection(persistentDbPath))
            using (DatabaseConnection = new SqliteConnection(memoryDbPath))
            {
                try
                {
                    // Verbindung zur Datenbank herstellen
                    persistentConnection.Open();
                    DatabaseConnection.Open();

                    persistentConnection.BackupDatabase(DatabaseConnection);
                    persistentConnection.Close();
                    File.Delete("library.db");
                    Debug.WriteLine("DB loaded into RAM");
                    buttonLoadLibrary.Enabled = false;
                    buttonAlterExistingDirectory.Enabled = true;
                    buttonCreateDirectory.Enabled = true;
                    buttonDeleteExistingDirectory.Enabled = true;
                }
                catch (Exception ex)
                {
                    throw new DBConcurrencyException("Error while loading the library", ex);
                }
            }
        }

        private void buttonCreateLibrary_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog
            {
                Filter = "TEK-Database (*.tekdb)|*.tekdb",
                Title = "Save Library",
                FileName = "library.tekdb"
            };
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string connectionString = "Data Source=:memory:;Version=3;";

                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    //Create all tables
                    string createTableQuery =
                        "CREATE TABLE \"Chunkmaster\"" +
                        "(" +
                        "\"ID\" INTEGER NOT NULL," +
                        "\"size\" INTEGER NOT NULL," +
                        "PRIMARY KEY(\"ID\" AUTOINCREMENT" +
                        ");" +
                        "CREATE TABLE \"directories\"" +
                        "(" +
                        "\"ID\" INTEGER NOT NULL," +
                        "\"Name\" TEXT NOT NULL," +
                        "\"ChunkID\" INTEGER NOT NULL," +
                        "\"PRIMARY KEY\"(\"ID\" AUTOINCREMENT)" +
                        ");" +
                        "CREATE TABLE \"images\"" +
                        "(" +
                        "\"ID\" INTEGER NOT NULL," +
                        "\"directoryID\" INTEGER NOT NULL," +
                        "\"chunkID\" INTEGER NOT NULL," +
                        "PRIMARY KEY(\"ID\" AUTOINCREMENT)" +
                        ");";


                    using (SqliteCommand createTableCommand = new SqliteCommand(createTableQuery, connection))
                    {
                        createTableCommand.ExecuteNonQuery();
                        Debug.WriteLine("DB created");
                    }


                    connection.BackupDatabase(new SqliteConnection($"Data Source={Path.Combine(Directory.GetCurrentDirectory(), "temp.db")};Version=3;"));
                    byte[] dbBytes = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "temp.db"));
                    File.WriteAllBytes(fileDialog.FileName, key.Encrypt(dbBytes).Result.ToArray());
                    File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "temp.db"));

                    Debug.WriteLine("DB saved");
                    connection.Close();
                    MessageBox.Show("Library created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
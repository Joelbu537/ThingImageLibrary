using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThingImageLibrary
{
    public class TekKey
    {
        private static byte[] key = null;
        private static byte[] iv = null;
        public static byte Version { get; private set; }
        public static int KeyID { get; private set; }
        private static string path = string.Empty;
        private static byte newestVersion = 2;
        private static byte oldestSupportedVersion = 2;
        public TekKey(string _path)
        {
            path = _path;
        }
        public TekKey()
        {
            path = null;
        }
        public bool IsPasswordProtected()
        {
            if (path == string.Empty)
                throw new InvalidTekKeyException("No TekKey is loaded.");
            if(File.Exists(path) && Path.GetExtension(path) == ".tek")
            {
                byte[] keyBytes = File.ReadAllBytes(path);
                return (keyBytes[3] == 1) ? true : false;
            }
            throw new InvalidTekKeyFormatException("TekKey could not be read.");
        }
        public bool Load(string password = "") //Leave empty if no password
        {
            if (File.Exists(path) && Path.GetExtension(path) == ".tek" && password != null)
            {
                byte[] keyBytes = File.ReadAllBytes(path);

                ushort keyID = (ushort)((keyBytes[1] << 8) | keyBytes[0]);
                byte version = keyBytes[2];
                bool isPasswordProtected = (keyBytes[3] == 1) ? true : false;

                byte[] hashedBytes = new byte[keyBytes.Length - 4];
                Array.Copy(keyBytes, 4, hashedBytes, 0, hashedBytes.Length);

                if (isPasswordProtected && password != "")
                {

                }
                else if(isPasswordProtected && password == "")
                {
                    throw new TekKeyPasswordException("Key is password protected but no password was passed to the method.");
                }
                else if (!isPasswordProtected)
                {
                    if(password != "")
                        Debug.WriteLine("WARNING: It is not necessary to pass a password when the target key is not password protected.");
                }
            }
            else if (password == null)
            {
                throw new ArgumentNullException("Password cannot be null!");
            }
            else if(Path.GetExtension(path) != ".tek")
            {
                throw new InvalidTekKeyFormatException("The path does not point to a .tek file.\n" + path);
            }
            else if (!File.Exists(path))
            {
                throw new FileNotFoundException("The path does not point to a existing file.\n" + path);
            }
            else
            {
                
            }
            return false;
        }
        public byte[] Generate(string name, string password = "")
        {
            byte[] result = null;
            try
            {
                //Public Info

                ushort keyID = Convert.ToUInt16(new Random().Next(0, 65535));
                byte version = newestVersion;
                byte pwdProtected = (password != "") ? (byte)1 : (byte)0;

                //Public Info END

                //Private Info

                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

               
                // Private Info END

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Fehler: {ex.Message}");
                return null;
            }
        }

        private async Task<byte[]> Encrypt(string path)
        {
            if(key == null || iv == null)
            {
                throw new InvalidOperationException("TekKey not loaded!\nUse TekKey.Load(path) to load a .tek key file.");
            }
            if(File.Exists(path) && Path.GetExtension(path) == ".tef")
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    using (FileStream fsInput = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
                    using (MemoryStream memoryStream = new MemoryStream())
                    using (CryptoStream cs = new CryptoStream(fsInput, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        await cs.CopyToAsync(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }
            else if(!File.Exists(path))
            {
                throw new FileNotFoundException("File not found at ", path);
            }
            else
            {
                throw new InvalidOperationException("File is not a .tef file!");
            }
        }
    }
    public class InvalidTekKeyException : Exception
    {
        public InvalidTekKeyException() : base() { }
        public InvalidTekKeyException(string message) : base(message) { }
        public InvalidTekKeyException(string message, Exception inner) : base(message, inner) { }
    }
    public class OutdatedTekKeyException : Exception
    {
        public OutdatedTekKeyException() : base() { }
        public OutdatedTekKeyException(string message) : base(message) { }
        public OutdatedTekKeyException(string message, Exception inner) : base(message, inner) { }
    }
    public class InvalidTekKeyFormatException : Exception
    {
        public InvalidTekKeyFormatException() : base() { }
        public InvalidTekKeyFormatException(string message) : base(message) { }
        public InvalidTekKeyFormatException(string message, Exception inner) : base(message, inner) { }
    }
    public class TekKeyPasswordException : Exception
    {
        public TekKeyPasswordException() : base() { }
        public TekKeyPasswordException(string message) : base(message) { }
        public TekKeyPasswordException(string message, Exception inner) : base(message, inner) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Drawing.Text;
using System.Net.Http;
using System.Diagnostics;
using System.Security.Policy;
using System.Windows.Forms;

namespace ThingImageLibrary
{
    public class TekKey
    {
        private static byte[] key = null;
        private static byte[] iv = null;
        public static byte Version { get; private set; }
        public static int KeyID { get; private set; }
        private static string path;
        private static byte newestVersion = 2;
        private static byte oldestSupportedVersion = 2;
        public TekKey(string _path)
        {
            path = _path;
        }
        public bool IsKeyProtected()
        {
            if(File.Exists(path) && Path.GetExtension(path) == ".tek")
            {
                byte[] keyBytes = File.ReadAllBytes(path);
                List<byte> VersionList = new List<byte>();
                List<byte> pwdHash = new List<byte>();
                for (int i = 0; i < 4; i++)
                {
                    VersionList.Add(keyBytes[i]);
                    string Version = Encoding.UTF8.GetString(VersionList.ToArray());
                    Debug.WriteLine("Loading .tek key V. " + Version);
                }
                for (int i = 4; i < 36; i++)
                {
                    if (keyBytes[i] != 0)
                    {
                        return true;
                    }
                    pwdHash.Add(keyBytes[i]);
                }
            }
            return false;
        }
        public bool Load(string password = "") //Leave empty if no password
        {
            if (File.Exists(path) && Path.GetExtension(path) == ".tek" && password != null)
            {
                byte[] keyBytes = File.ReadAllBytes(path);
                List<byte> VersionList = new List<byte>();
                List<byte> pwdHash = new List<byte>();
                List<byte> aesHash = new List<byte>();
                List<byte> ivHash = new List<byte>();
                List<byte> keyID = new List<byte>();
                bool pwdProtected = false;

                for (int i = 0; i < 4; i++)
                {
                    VersionList.Add(keyBytes[i]);
                }
                for (int i = 4; i < 36; i++)
                {
                    if (keyBytes[i] != 0)
                    {
                        pwdProtected = true;
                    }
                    pwdHash.Add(keyBytes[i]);
                }
                for (int i = 36; i < 68; i++)
                {
                    aesHash.Add(keyBytes[i]);
                }
                for (int i = 68; i < 84; i++)
                {
                    ivHash.Add(keyBytes[i]);
                }
                for(int i = 84; i < 88; i++)
                {
                    keyID.Add(keyBytes[i]);
                }

                if (pwdProtected && password != "")
                {
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        byte[] pwdHashEncrypted = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                        if(pwdHashEncrypted == pwdHash.ToArray())
                        {
                            key = aesHash.ToArray();
                            iv = ivHash.ToArray();
                            KeyID = BitConverter.ToInt32(keyID.ToArray(), 0);
                            return true;
                        }
                        else
                        {
                            throw new AccessViolationException("Password is incorrect!");
                        }
                    }
                }
                else if(pwdProtected && password == "")
                {
                    throw new AccessViolationException("Key is password protected!");
                }
                else if (!pwdProtected)
                {
                    key = aesHash.ToArray();
                    iv = ivHash.ToArray();
                }
            }
            else if (password == null)
            {
                throw new ArgumentNullException("Password cannot be null!");
            }
            else
            {
                throw new FileNotFoundException("TekKey not found at ", path);
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
                byte[] aesKey;
                byte[] aesIV;

                using (Aes aes = Aes.Create())
                {
                    aes.GenerateKey();
                    aes.GenerateIV();
                    aesKey = aes.Key;
                    aesIV = aes.IV;
                }

                //Salt erstellen
                byte[] salt = new byte[16];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                byte[] key;
                using (var rfc2898 = new Rfc2898DeriveBytes(passwordBytes, salt, 10000))
                {
                    key = rfc2898.GetBytes(32);
                }

                byte[] encryptedAesKey;
                byte[] iv;
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.GenerateIV();
                    iv = aes.IV;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(aesKey, 0, aesKey.Length);
                        }
                        encryptedAesKey = ms.ToArray();
                    }
                }

                // Private Info END

                result = BitConverter.GetBytes(keyID).Co
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
}

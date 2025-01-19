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
        private static byte[] Key = null;
        private static byte[] IV = null;
        public static byte Version { get; private set; } = 0;
        public static int KeyID { get; private set; } = 0;
        public static bool PasswordProtected { get; private set; } = false;

        private static byte newestVersion = 2;
        private static byte oldestSupportedVersion = 2;
        public byte[] Generate(string password = "")
        {
            byte[] result = null;

            byte[] aesMainKey;
            byte[] aesMainIV;
            byte[] privateMD5 = new byte[16];
            Array.Clear(privateMD5, 0, privateMD5.Length);
            try
            {
                //Public Info

                ushort keyID = Convert.ToUInt16(new Random().Next(0, 65535));
                byte version = newestVersion;
                byte pwdProtected = (password != "") ? (byte)1 : (byte)0;

                //Public Info END

                //Private Info
                using (Aes aes = Aes.Create())
                {
                    aes.GenerateKey();
                    aes.GenerateIV();
                    aesMainKey = aes.Key;
                    aesMainIV = aes.IV;
                }
                byte[] tempArray = aesMainKey.Concat(aesMainIV).ToArray();
                if (password != "")
                {
                    byte[] passwordBytes;
                    passwordBytes = Encoding.UTF8.GetBytes(password);
                    privateMD5 = GetMD5(tempArray);
                    tempArray = EncryptWithPassword(tempArray, password);
                }



                // Private Info END
                return result = BitConverter.GetBytes(keyID)                    //ID
                    .Concat(BitConverter.GetBytes(version)).ToArray()           //Version
                    .Concat(BitConverter.GetBytes(pwdProtected)).ToArray()      //PWD Status
                    .Concat(privateMD5).ToArray()                               //MD5
                    .Concat(tempArray).ToArray();                               //AES Key + IV
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Fehler: {ex.Message}");
                return null;
            }
        }
        public bool Load(byte[] keyBytes, string password = "")
        {
            Debug.WriteLine("Loading key...");
            if (password != null)
            {
                ushort keyID = (ushort)((keyBytes[1] << 8) | keyBytes[0]);
                Debug.WriteLine($"KeyID: {keyID}");

                byte version = keyBytes[2];
                Debug.WriteLine($"Version {version}");
                if (version < oldestSupportedVersion)
                {
                    throw new OutdatedTekKeyException($"TekKey version is {version}, while oldest supported version is {oldestSupportedVersion}.");
                }

                bool isPasswordProtected = (keyBytes[3] == 1) ? true : false;
                Debug.WriteLine($"Password protected: {isPasswordProtected}");

                byte[] md5Hash = new byte[16];
                Debug.Write("MD5 Hash: ");
                for(int i = 4; i < 20; i++)
                {
                    md5Hash[i - 4] = keyBytes[i];
                    Debug.Write(keyBytes[i]);
                }
                Debug.Write("\n");

                byte[] privateBytes = new byte[48];
                if (isPasswordProtected)
                {
                    Debug.WriteLine("Extracting protected AES Key & IV");
                    privateBytes = new byte[keyBytes.Length - 20];
                    for (int i = 0; i < privateBytes.Length - 20; i++)
                    {
                        privateBytes[i] = keyBytes[i + 20];
                    }
                }
                else
                {
                    Debug.WriteLine("Extracting AES Key & IV");
                }


                if (isPasswordProtected && password != "")
                {
                    Debug.WriteLine("Loading with password...");
                    byte[] _key = new byte[32];
                    byte[] _iv = new byte[16];

                    byte[] privateEncrypted = DecryptWithPassword(privateBytes, password);
                    Debug.WriteLine("Comparing MD5...");
                    if (GetMD5(privateEncrypted).SequenceEqual(md5Hash))
                    {
                        Debug.WriteLine("MD5 OK");
                        for (int i = 0; i < 32; i++)
                        {
                            _key[i] = privateEncrypted[i];
                        }
                        for (int i = 0; i < 16; i++)
                        {
                            _iv[i] = privateEncrypted[i + 32];
                        }
                        Key = _key;
                        IV = _iv;
                        Version = version;
                        KeyID = keyID;
                        PasswordProtected = isPasswordProtected;
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine("MD5 NOT OK");
                        throw new TekKeyPasswordInvalidException("Incorrect password!");
                    }
                }
                else if (isPasswordProtected && password == "")
                {
                    throw new TekKeyPasswordRequiredException("Key is password protected but no password was passed to the method.");
                }
                else if (!isPasswordProtected)
                {
                    if (password != "")
                        Debug.WriteLine("WARNING: It is not necessary to pass a password when the target key is not password protected.");

                    Version = version;
                    KeyID = keyID;
                    PasswordProtected = isPasswordProtected;

                    byte[] _key = new byte[32];
                    byte[] _iv = new byte[16];

                    Debug.Write("Key:");
                    for(int i = 0; i < 32; i++)
                    {
                        Debug.Write(" " + keyBytes[i + 20]);
                        _key[i] = keyBytes[i + 20];
                    }
                    Debug.Write("\nIV:");
                    for(int i = 0; i < 16; i++)
                    {
                        Debug.Write(" " + keyBytes[i + 52]);
                        _iv[i] = keyBytes[i + 52];
                    }
                    Debug.Write("\n");
                    Key = _key;
                    IV = _iv;
                    Debug.WriteLine("Unprotected Key loaded");
                    return true;
                }
            }
            else if (password == null)
            {
                throw new ArgumentNullException("Password cannot be null!");
            }
            return false;
        }
        public bool Load(string path, string password = "") //Leave empty if no password
        {
            if (File.Exists(path) && Path.GetExtension(path) == ".tek")
            {
                return Load(File.ReadAllBytes(path), password);
            }
            else if (Path.GetExtension(path) != ".tek")
            {
                throw new InvalidTekKeyFormatException("The path does not point to a .tek file.\n" + path);
            }
            else if (!File.Exists(path))
            {
                throw new FileNotFoundException("The path does not point to a existing file.\n" + path);
            }
            throw new Exception("Unreachable point reached! This should not happen!!!");
        }
        private async Task<MemoryStream> Encrypt(byte[] data)
        {
            if (Key == null || IV == null)
            {
                throw new InvalidOperationException("TekKey not loaded!\nUse TekKey.Load(path) to load a .tek key file.");
            }
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Key;
                    aes.IV = IV;

                    var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                    var ms = new MemoryStream();
                    var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return ms;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private async Task<MemoryStream> Encrypt(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The path does not point to a existing file");
            }
            if(Path.GetExtension(path) == ".tef")
            {
                throw new TekEncryptionTargetException("Target file is already encrypted!");
            }
            return Encrypt(File.ReadAllBytes(path)).Result;
        }
        private async Task<MemoryStream> Decrypt(byte[] data)
        {
            if (Key == null || IV == null)
            {
                throw new InvalidOperationException("TekKey not loaded!\nUse TekKey.Load(path) to load a .tek key file.");
            }
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Key;
                    aes.IV = IV;

                    var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    var ms = new MemoryStream(data);
                    var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                    var result = new MemoryStream();
                    cs.CopyTo(result);
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private async Task<MemoryStream> Decrpt(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The path does not point to a existing file");
            }
            if (Path.GetExtension(path) != ".tef")
            {
                throw new TekEncryptionTargetException("Target file is not encrypted!");
            }
            return Decrypt(File.ReadAllBytes(path)).Result;
        }
        private byte[] GetMD5(byte[] bytes)
        {
            using(MD5 md5 = MD5.Create())
            {
                return md5.ComputeHash(bytes);
            }
        }
        private byte[] EncryptWithPassword(byte[] data, string password)
        {
            var aes = Aes.Create();
            var keyDerivation = new Rfc2898DeriveBytes(password, aes.BlockSize / 8);
            aes.Key = keyDerivation.GetBytes(aes.KeySize / 8);
            aes.IV = keyDerivation.GetBytes(aes.BlockSize / 8);

            //Verschlüsseln
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            return ms.ToArray();
        }
        private byte[] DecryptWithPassword(byte[] encryptedData, string password)
        {
            var aes = Aes.Create();
            var keyDerivation = new Rfc2898DeriveBytes(password, aes.BlockSize / 8);
            aes.Key = keyDerivation.GetBytes(aes.KeySize / 8);
            aes.IV = keyDerivation.GetBytes(aes.BlockSize / 8);

            //Entschlüsseln
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var ms = new MemoryStream(encryptedData);
            var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            var reader = new MemoryStream();
            cs.CopyTo(reader);
            return reader.ToArray();
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
    public class TekKeyPasswordRequiredException : TekKeyPasswordException
    {
        public TekKeyPasswordRequiredException() : base() { }
        public TekKeyPasswordRequiredException(string message) : base(message) { }
        public TekKeyPasswordRequiredException(string message, Exception inner) : base(message, inner) { }
    }
    public class TekKeyPasswordInvalidException : TekKeyPasswordException
    {
        public TekKeyPasswordInvalidException() : base() { }
        public TekKeyPasswordInvalidException(string message) : base(message) { }
        public TekKeyPasswordInvalidException(string message, Exception inner) : base(message, inner) { }
    }
    public class TekEncryptionTargetException : Exception
    {
        public TekEncryptionTargetException() : base() { }
        public TekEncryptionTargetException(string message) : base(message) { }
        public TekEncryptionTargetException(string message, Exception inner) : base(message, inner) { }
    }
}

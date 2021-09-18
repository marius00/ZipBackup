using System;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace ZipBackup.Utils {
    class EncryptionUtil {
        public static int GetDeterministicHashCode(string str) {
            unchecked {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length; i += 2) {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }


        public static string GetCpuSerial() {
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc) {
                return mo.Properties["processorID"].Value.ToString();
            }

            return string.Empty;
        }

        //AES Encryption
        public static string EncryptString(string plainText, string keyToUse) {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create()) {
                aes.Key = Encoding.UTF8.GetBytes(Get128BitString(keyToUse));
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream()) {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write)) {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream)) {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string cipherText, string keyToUse) {
            if (string.IsNullOrEmpty(cipherText))
                return string.Empty;

            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create()) {
                aes.Key = Encoding.UTF8.GetBytes(Get128BitString(keyToUse));
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer)) {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read)) {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream)) {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static string Get128BitString(string keyToConvert) {
            StringBuilder b = new StringBuilder();
            for (int i = 0; i < 16; i++) {
                b.Append(keyToConvert[i % keyToConvert.Length]);
            }
            keyToConvert = b.ToString();

            return keyToConvert;
        }
    }
}

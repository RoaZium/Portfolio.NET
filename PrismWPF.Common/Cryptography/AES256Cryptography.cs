using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace PrismWPF.Common.Cryptography
{
    public class AES256Cryptography
    {
        // 참고 사이트
        // Coded By retn0@naver.com : http://blog.naver.com/retn0
        // Original Source Code : http://www.codeproject.com/Articles/769741/Csharp-AES-bits-Encryption-Library-with-Salt

        private static byte[] Encrypt(byte[] plainBytes, byte[] keyBytes, byte[] saltBytes, bool compressData)
        {
            byte[] encryptedBytes = null;
            byte[] plainData = compressData ? Compress(plainBytes) : plainBytes;

            using (MemoryStream MS = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    Rfc2898DeriveBytes keyInfo = new Rfc2898DeriveBytes(SHA256.Create().ComputeHash(keyBytes), saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = keyInfo.GetBytes(AES.KeySize / 8);
                    AES.IV = keyInfo.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (CryptoStream CS = new CryptoStream(MS, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        CS.Write(plainData, 0, plainData.Length);
                        CS.Close();
                    }

                    encryptedBytes = MS.ToArray();
                    keyInfo.Dispose();
                }
            }

            return encryptedBytes;
        }

        private static byte[] Decrypt(byte[] cipherBytes, byte[] keyBytes, byte[] saltBytes, bool decompressData)
        {
            byte[] decryptedBytes = null;

            using (MemoryStream MS = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    Rfc2898DeriveBytes keyInfo = new Rfc2898DeriveBytes(SHA256.Create().ComputeHash(keyBytes), saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = keyInfo.GetBytes(AES.KeySize / 8);
                    AES.IV = keyInfo.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (CryptoStream CS = new CryptoStream(MS, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        CS.Write(cipherBytes, 0, cipherBytes.Length);
                        CS.Close();
                    }

                    decryptedBytes = decompressData ? Decompress(MS.ToArray()) : MS.ToArray();
                    keyInfo.Dispose();
                }
            }

            return decryptedBytes;
        }

        private static byte[] Compress(byte[] binaryData)
        {
            MemoryStream outPut = new MemoryStream();

            using (DeflateStream dStream = new DeflateStream(outPut, CompressionLevel.Optimal))
            {
                dStream.Write(binaryData, 0, binaryData.Length);
            }

            return outPut.ToArray();
        }

        private static byte[] Decompress(byte[] binaryData)
        {
            MemoryStream inPut = new MemoryStream(binaryData);
            MemoryStream outPut = new MemoryStream();

            using (DeflateStream dStream = new DeflateStream(inPut, CompressionMode.Decompress))
            {
                dStream.CopyTo(outPut);
                inPut.Dispose();
            }

            return outPut.ToArray();
        }

        private static byte[] ConvertToBinary<T>(T convertData, bool useBase64) where T : class
        {
            byte[] convertedData = null;
            object copyData = convertData;

            if (convertData.GetType().Equals(typeof(string)))
            {
                convertedData = useBase64 ? Convert.FromBase64String((string)copyData) : Encoding.UTF8.GetBytes((string)copyData);
            }
            else
            {
                convertedData = (byte[])copyData;
            }

            return convertedData;
        }

        /// <summary>
        /// Encrypt data to binary using AES-256 algorithm.
        /// </summary>
        /// <typeparam name="P">Type of plain data. It's only byte array or string.</typeparam>
        /// <typeparam name="K">Type of key data. It's only byte array or string.</typeparam>
        /// <param name="plainData">Plain data to use for encryption.</param>
        /// <param name="keyData">Key data to use for encryption.</param>
        /// <param name="saltBytes">Salt binary data to use for encryption.</param>
        /// <param name="compressData">If this value is true, compress plain data before encryption.</param>
        public static byte[] EncryptToBinary<P, K>(P plainData, K keyData, byte[] saltBytes, bool compressData) where P : class where K : class
        {
            byte[] plainBytes = ConvertToBinary<P>(plainData, false), keyBytes = ConvertToBinary<K>(keyData, false);
            return Encrypt(plainBytes, keyBytes, saltBytes, compressData);
        }

        /// <summary>
        /// Encrypt data to string using AES-256 algorithm.
        /// </summary>
        /// <typeparam name="P">Type of plain data. It's only byte array or string.</typeparam>
        /// <typeparam name="K">Type of key data. It's only byte array or string.</typeparam>
        /// <param name="plainData">Plain data to use for encryption.</param>
        /// <param name="keyData">Key data to use for encryption.</param>
        /// <param name="saltBytes">Salt binary data to use for encryption.</param>
        /// <param name="compressData">If this value is true, compress plain data before encryption.</param>
        public static string EncryptToString<P, K>(P plainData, K keyData, byte[] saltBytes, bool compressData) where P : class where K : class
        {
            byte[] plainBytes = ConvertToBinary<P>(plainData, false), keyBytes = ConvertToBinary<K>(keyData, false);
            return Convert.ToBase64String(Encrypt(plainBytes, keyBytes, saltBytes, compressData));
        }

        /// <summary>
        /// Decrypt data to binary using AES-256 algorithm.
        /// </summary>
        /// <typeparam name="C">Type of cipher data. It's only byte array or string.</typeparam>
        /// <typeparam name="K">Type of key data. It's only byte array or string.</typeparam>
        /// <param name="cipherData">Cipher data to use for decryption.</param>
        /// <param name="keyData">Key data to use for decryption.</param>
        /// <param name="saltBytes">Salt binary data to use for decryption.</param>
        /// <param name="decompressData">If this value is true, decompress decrypted data.</param>
        public static byte[] DecryptToBinary<C, K>(C cipherData, K keyData, byte[] saltBytes, bool decompressData) where C : class where K : class
        {
            try
            {
                byte[] cipherBytes = ConvertToBinary<C>(cipherData, true), keyBytes = ConvertToBinary<K>(keyData, false);
                return Decrypt(cipherBytes, keyBytes, saltBytes, decompressData);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Decrypt data to string using AES-256 algorithm.
        /// </summary>
        /// <typeparam name="C">Type of cipher data. It's only byte array or string.</typeparam>
        /// <typeparam name="K">Type of key data. It's only byte array or string.</typeparam>
        /// <param name="cipherData">Cipher data to use for decryption.</param>
        /// <param name="keyData">Key data to use for decryption.</param>
        /// <param name="saltBytes">Salt binary data to use for decryption.</param>
        /// <param name="decompressData">If this value is true, decompress decrypted data.</param>
        public static string DecryptToString<C, K>(C cipherData, K keyData, byte[] saltBytes, bool decompressData) where C : class where K : class
        {
            try
            {
                byte[] cipherBytes = ConvertToBinary<C>(cipherData, true), keyBytes = ConvertToBinary<K>(keyData, false);
                return Encoding.UTF8.GetString(Decrypt(cipherBytes, keyBytes, saltBytes, decompressData));
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

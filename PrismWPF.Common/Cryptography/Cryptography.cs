using PrismWPF.Common.Infrastructure;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PrismWPF.Common.Cryptography
{
    public class Cryptography
    {
        private readonly RSACryptoServiceProvider rsa;

        /// <summary> 
        /// RSA 비대칭 암호화를 간단하게 제공하기 위한 클래스
        /// </summary>
        /// <param name="keyContainerName">
        /// CSP(암호화 서비스 공급자)에서 키를 식별하기 위한 이름
        /// </param>
        /// <param name="isPersistKey">
        /// 키가 CSP(암호화 서비스 공급자)에서 지속되어야 하는지 여부.<para/>
        /// false일 경우 keyContainerName이 같아도 매번 다른 키가 생성된다.<para/>
        /// Defaut : true
        /// </param>
        public Cryptography(string keyContainerName, bool isPersistKey = true)
        {
            CspParameters cspp = new CspParameters
            {
                KeyContainerName = keyContainerName
            };

            rsa = new RSACryptoServiceProvider(cspp)
            {
                PersistKeyInCsp = isPersistKey
            };
        }

        /// <summary>
        /// 평문을 간단하게 암호화하여 파일에 저장하는 메소드
        /// </summary>
        /// <param name="filePath">암호화한 내용을 저장할 파일의 경로</param>
        /// <param name="text">암호화하여 파일에 저장할 평문</param>
        /// <returns>파일에 암호문을 저장하는데 성공했는지 여부</returns>
        public bool WriteFile(string filePath, string text)
        {
            bool returnVal = false;

            RijndaelManaged rjndl = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 256,
                Mode = CipherMode.CBC
            };
            ICryptoTransform transform = rjndl.CreateEncryptor();

            byte[] keyEncrypted = rsa.Encrypt(rjndl.Key, false);

            byte[] LenK = new byte[4];
            byte[] LenIV = new byte[4];

            int lKey = keyEncrypted.Length;
            LenK = BitConverter.GetBytes(lKey);
            int lIV = rjndl.IV.Length;
            LenIV = BitConverter.GetBytes(lIV);

            string directoryPath = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directoryPath);

            using (FileStream outFs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                outFs.Write(LenK, 0, 4);
                outFs.Write(LenIV, 0, 4);
                outFs.Write(keyEncrypted, 0, lKey);
                outFs.Write(rjndl.IV, 0, lIV);

                using (CryptoStream outStreamEncrypted = new CryptoStream(outFs, transform, CryptoStreamMode.Write))
                {
                    int blockSizeBytes = rjndl.BlockSize / 8;

                    Encoding encoding = Encoding.UTF8;

                    IOHelper.Write(outStreamEncrypted, text, encoding);
                    returnVal = true;
                }
                outFs.Close();
            }

            return returnVal;
        }

        /// <summary>
        /// 암호화된 파일을 간단하게 복호화하여 평문을 가져오는 메소드
        /// </summary>
        /// <param name="filePath">암호문을 가져올 파일의 경로</param>
        /// <returns>복호화된 평문</returns>
        public string ReadFile(string filePath)
        {
            string returnVal = null;

            try
            {
                if (File.Exists(filePath))
                {
                    RijndaelManaged rjndl = new RijndaelManaged
                    {
                        KeySize = 256,
                        BlockSize = 256,
                        Mode = CipherMode.CBC
                    };

                    byte[] LenK = new byte[4];
                    byte[] LenIV = new byte[4];

                    using (FileStream inFs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        inFs.Seek(0, SeekOrigin.Begin);
                        inFs.Read(LenK, 0, 3);
                        inFs.Seek(4, SeekOrigin.Begin);
                        inFs.Read(LenIV, 0, 3);

                        int lenK = BitConverter.ToInt32(LenK, 0);
                        int lenIV = BitConverter.ToInt32(LenIV, 0);

                        int startC = lenK + lenIV + 8;
                        int lenC = (int)inFs.Length - startC;

                        byte[] KeyEncrypted = new byte[lenK];
                        byte[] IV = new byte[lenIV];

                        inFs.Seek(8, SeekOrigin.Begin);
                        inFs.Read(KeyEncrypted, 0, lenK);
                        inFs.Seek(8 + lenK, SeekOrigin.Begin);
                        inFs.Read(IV, 0, lenIV);

                        byte[] KeyDecrypted = rsa.Decrypt(KeyEncrypted, false);

                        ICryptoTransform transform = rjndl.CreateDecryptor(KeyDecrypted, IV);

                        using (CryptoStream inStreamEncrypted = new CryptoStream(inFs, transform, CryptoStreamMode.Read))
                        {
                            int blockSizeBytes = rjndl.BlockSize / 8;

                            Encoding encoding = Encoding.UTF8;

                            returnVal = IOHelper.Read(inStreamEncrypted, encoding);
                        }
                        inFs.Close();
                    }
                }
            }
            catch
            { }

            return returnVal;
        }
    }
}

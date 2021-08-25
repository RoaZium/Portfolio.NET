using System.IO;

namespace PrismWPF.Common.Infrastructure
{
    public static class FileUtils
    {
        public static byte[] GetBytesFromFile(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] bytes = new byte[fs.Length];

            fs.Read(bytes, 0, (int)fs.Length);

            return bytes;
        }
    }
}

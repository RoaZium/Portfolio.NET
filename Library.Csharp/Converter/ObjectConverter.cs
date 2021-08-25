using System;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PrismWPF.Common.Converter
{
    public class ObjectConverter
    {
        public static Stream ToStream(Bitmap bmp)
        {
            if (bmp == null)
            {
                return null;
            }

            Stream stream = new MemoryStream();
            bmp.Save(stream, bmp.RawFormat);
            bmp.Dispose();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public static Stream ToStream(Icon icon)
        {
            if (icon == null)
            {
                return null;
            }

            Stream stream = new MemoryStream();
            icon.Save(stream);
            icon.Dispose();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public static ImageSource ToImageSource(Bitmap bmp)
        {
            if (bmp == null)
            {
                return null;
            }

            return System.Windows.Media.Imaging.BitmapFrame.Create(ToStream(bmp));
        }

        public static ImageSource ToImageSource(Icon icon)
        {
            if (icon == null)
            {
                return null;
            }

            return System.Windows.Media.Imaging.BitmapFrame.Create(ToStream(icon));
        }

        public static ImageSource ToImageSource(string imageUri)
        {
            if (imageUri == null)
            {
                return null;
            }

            return ToImageSource(new Uri(imageUri));
        }

        public static ImageSource ToImageSource(Uri imageUri)
        {
            try
            {
                if (imageUri == null)
                {
                    return null;
                }

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = imageUri;
                image.EndInit();

                return image;
            }
            catch
            {
                return null;
            }
        }

        public static ImageSource ByteArrayToImageSource(byte[] byteArr)
        {
            try
            {
                if (byteArr == null)
                {
                    return null;
                }

                MemoryStream ms = new MemoryStream(byteArr);

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.EndInit();

                return image;
            }
            catch
            {
                return null;
            }
        }

        public static ImageSource StringToImageSource(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);
            var bitImg = new BitmapImage();
            ImageSource imageSource = null;

            using (var stream = new MemoryStream(bytes))
            {
                bitImg.BeginInit();
                bitImg.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitImg.CacheOption = BitmapCacheOption.OnLoad; bitImg.StreamSource = stream;
                bitImg.EndInit();
                imageSource = bitImg as ImageSource;
            }
            return imageSource;
        }

    }
}

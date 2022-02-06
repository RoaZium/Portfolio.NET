using PrismWPF.Common.Converter;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;


namespace PrismWPF.Common
{
    public class ResManager : INotifyPropertyChanged
    {
        public ResourceManager ResourceManager { get; set; }

        public ResManager()
        {
        }

        public object this[string key]
        {
            get
            {
                if (ResourceManager == null)
                {
                    throw new ArgumentException(@"Please set your resource manager to 'ResourceManager' property.");
                }
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException("key");
                }

                return GetResource(key, ResourceManager);
            }
        }

        public object GetResource(string resourceName)
        {
            try
            {
                object obj = ResourceManager.GetObject(resourceName);

                if (obj == null)
                {
                    return null;
                }
                else if (obj is string)
                {
                    return obj as string;
                }
                else if (obj is Bitmap)
                {
                    return ObjectConverter.ToImageSource(obj as Bitmap);
                }
                else if (obj is Icon)
                {
                    return ObjectConverter.ToImageSource(obj as Icon);
                }
                else
                {
                    return obj;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static object GetResource(string resourceName, ResourceManager manager)
        {
            try
            {
                object obj = manager.GetObject(resourceName);

                if (obj == null)
                {
                    return null;
                }
                else if (obj is string)
                {
                    return obj as string;
                }
                else if (obj is Bitmap)
                {
                    return ObjectConverter.ToImageSource(obj as Bitmap);
                }
                else if (obj is Icon)
                {
                    return ObjectConverter.ToImageSource(obj as Icon);
                }
                else
                {
                    return obj;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnCultureChanged(object sender, EventArgs e)
        {
            PropertyChangedEventHandler evt = PropertyChanged;

            if (evt != null)
            {
                evt.Invoke(this, new PropertyChangedEventArgs(string.Empty));
            }
        }
    }
}

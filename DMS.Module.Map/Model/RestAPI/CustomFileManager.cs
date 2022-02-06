using Prism.Mvvm;

namespace DMS.Module.Map.Model.RestAPI
{
    public class CustomFileManager : BindableBase
    {
        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                RaisePropertyChanged("FileName");
            }
        }

        private byte[] _fileData;
        public byte[] FileData
        {
            get => _fileData;
            set
            {
                _fileData = value;
                RaisePropertyChanged("FileData");
            }
        }
    }

}

using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace DMS.Module.Map.Model.RestAPI
{
    public class SystemUseCheckModel : BindableBase
    {
        private int _id;
        [JsonIgnore]
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        private string _account;
        public string Account
        {
            get => _account;
            set => _account = value;
        }

        private string _useYn;
        public string UseYn
        {
            get => _useYn;
            set => _useYn = value;
        }

        private DateTime? _regDate = DateTime.Now;
        public DateTime? RegDate
        {
            get => _regDate;
            set => _regDate = value;
        }

        private string _windowName;
        public string WindowName
        {
            get => _windowName;
            set => _windowName = value;
        }

        private string _clientName = "DMS";
        public string ClientName
        {
            get => _clientName;
            set => _clientName = value;
        }
    }
}

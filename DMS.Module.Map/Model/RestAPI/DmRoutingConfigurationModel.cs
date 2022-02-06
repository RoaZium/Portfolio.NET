using PrismWPF.Common.MVVM;
using System;

namespace DMS.Module.Map.Model.RestAPI
{
    public class DmRoutingConfigurationModel : DMViewModelBase
    {
        public string Id { get; set; }

        public string ParentId { get; set; }

        public string Group { get; set; }

        public int Revision { get; set; }

        public string MappingCode { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int Seq { get; set; }

        public string Comment { get; set; }

        public string UseYn { get; set; }

        public DateTime RegDate { get; set; }

        public string RegUser { get; set; }

        public DateTime UpDate { get; set; }

        public string UpUser { get; set; }
    }
}

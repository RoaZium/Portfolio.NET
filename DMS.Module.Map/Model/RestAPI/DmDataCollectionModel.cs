using PrismWPF.Common.MVVM;
using System;

namespace DMS.Module.Map.Model.RestAPI
{
    public class DmDataCollectionModel : DMViewModelBase
    {
        public long CollectionId { get; set; }

        public string ResourceCode { get; set; }

        public DateTime CollectionDate { get; set; }

        public string CollectionValue { get; set; }
    }
}

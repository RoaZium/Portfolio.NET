using PrismWPF.Common.MVVM;
using System;

namespace DMS.Module.Map.Model.RestAPI
{
    class DmProcessModel : DMViewModelBase
	{
		public int Id { get; set; }
		public string AliasCode { get; set; }
		public string Group { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string Comment { get; set; }
		public string UseYn { get; set; }
		public DateTime RegDate { get; set; }
		public string RegUser { get; set; }
		public DateTime UpDate { get; set; }
		public string UpUser { get; set; }
	}
}

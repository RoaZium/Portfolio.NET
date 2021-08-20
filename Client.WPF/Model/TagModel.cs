using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.WPF.Model
{
	public class TagModel : BindableBase
	{
		private string _code;
		public string Code
		{
			get => _code;
			set
			{
				_code = value;
				RaisePropertyChanged("Code");
			}
		}

		private string _name;
		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				RaisePropertyChanged("Name");
			}
		}
	}
}

using Client.WPF.Model;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Client.WPF.ViewModel
{
	public class UserControlViewModel : BindableBase
	{
		public UserControlViewModel()
		{
			SetTagData();
		}

		private ObservableCollection<TagModel> _tagList;
		public ObservableCollection<TagModel> TagList
		{
			get
			{
				if (_tagList == null)
				{
					_tagList = new ObservableCollection<TagModel>();
				}
				return _tagList;
			}
			set
			{
				_tagList = value;
				RaisePropertyChanged("TagList");
			}
		}

		private void SetTagData()
		{
			TagModel tagM1 = new TagModel();
			tagM1.Code = "1";
			tagM1.Name = "태그1";
			TagList.Add(tagM1);

			TagModel tagM2 = new TagModel();
			tagM1.Code = "2";
			tagM1.Name = "태그2";
			TagList.Add(tagM2);

			TagModel tagM3 = new TagModel();
			tagM1.Code = "3";
			tagM1.Name = "태그3";
			TagList.Add(tagM3);

			TagModel tagM4 = new TagModel();
			tagM1.Code = "4";
			tagM1.Name = "태그4";
			TagList.Add(tagM4);

			TagModel tagM5 = new TagModel();
			tagM1.Code = "5";
			tagM1.Name = "태그5";
			TagList.Add(tagM5);
		}
	}
}

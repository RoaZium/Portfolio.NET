using Coever.Lib.CoPlatform.Core.Network;
using DMS.Module.Map.Demo;
using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model;
using DMS.Module.Map.Network;
using DMS.Module.Map.Properties;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_DetailInformationViewModel : CP_CommonBaseViewModel
    {
        public override void Load()
        {
            base.Load();
        }

        private ObservableCollection<MasterDetailInfoModel> _masterDetailInfoM;
        public ObservableCollection<MasterDetailInfoModel> MasterDetailInfoM
        {
            get
            {
                if (_masterDetailInfoM == null)
                {
                    _masterDetailInfoM = new ObservableCollection<MasterDetailInfoModel>();
                }
                return _masterDetailInfoM;
            }
            set
            {
                _masterDetailInfoM = value;
                RaisePropertyChanged("MasterDetailInfoM");
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        public void SetMasterData()
        {
            try
            {
                MasterDetailInfoM = null;

                if (DmComponentM.PropertyM.ItemType == ComponentType.DM601003.ToString())
                {
                    var resultApi = WebRequests.GetEquipmentMst(CoPlatformWebClient.Instance, DmComponentM.PropertyM.ItemCode);

                    if(resultApi == null)
                    {
                        return;
                    }

                    DmComponentM.PropertyM.TitleM.Content = Res.StrEquipment;

                    MasterDetailInfoModel ms = new MasterDetailInfoModel();
                    ms.Title = "설비명";
                    ms.Value = resultApi[0].Name;
                    MasterDetailInfoM.Add(ms);

                    MasterDetailInfoModel ms2 = new MasterDetailInfoModel();
                    ms2.Title = "모델번호";
                    ms2.Value = resultApi[0].ModelNumber;
                    MasterDetailInfoM.Add(ms2);

                    MasterDetailInfoModel ms3 = new MasterDetailInfoModel();
                    ms3.Title = "제조사";
                    ms3.Value = resultApi[0].Maker;
                    MasterDetailInfoM.Add(ms3);

                    MasterDetailInfoModel ms4 = new MasterDetailInfoModel();
                    ms4.Title = "설치일";
                    ms4.Value = resultApi[0].PurchaseDate.ToString();
                    MasterDetailInfoM.Add(ms4);

                    MasterDetailInfoModel ms5 = new MasterDetailInfoModel();
                    ms5.Title = "규격";
                    ms5.Value = resultApi[0].Comment;
                    MasterDetailInfoM.Add(ms5);
                }
                else if(DmComponentM.PropertyM.ItemType == ComponentType.DM601005.ToString())
                {
                    var resultApi = WebRequests.GetResourceSensors(CoPlatformWebClient.Instance, "resourceCode=" + DmComponentM.PropertyM.ItemCode);

                    if (resultApi == null)
                    {
                        return;
                    }

                    DmComponentM.PropertyM.TitleM.Content = Res.StrResource;

                    MasterDetailInfoModel ms = new MasterDetailInfoModel();
                    ms.Title = "센서명";
                    ms.Value = resultApi[0].ResourceName;
                    MasterDetailInfoM.Add(ms);

                    MasterDetailInfoModel ms2 = new MasterDetailInfoModel();
                    ms2.Title = "관리치 하한";
                    ms2.Value = resultApi[0].MLimitLow.ToString();
                    MasterDetailInfoM.Add(ms2);

                    MasterDetailInfoModel ms3 = new MasterDetailInfoModel();
                    ms3.Title = "관리치 상한";
                    ms3.Value = resultApi[0].MLimitHigh.ToString();
                    MasterDetailInfoM.Add(ms3);

                    MasterDetailInfoModel ms4 = new MasterDetailInfoModel();
                    ms4.Title = "한계치 하한";
                    ms4.Value = resultApi[0].LimitLow.ToString();
                    MasterDetailInfoM.Add(ms4);

                    MasterDetailInfoModel ms5 = new MasterDetailInfoModel();
                    ms5.Title = "한계치 상한";
                    ms5.Value = resultApi[0].LimitHigh.ToString();
                    MasterDetailInfoM.Add(ms5);
                }
            }
            catch(Exception ex)
            {

            }
            //NAME(설비명) / MODEL_NUMBER(모델번호) / MAKER(메이커사) / PURCHASE_DATE(설치일) / COMMENT(규격)
        }

        protected override void OnComponentData() 
        {
            SetMasterData();
            //if (DmComponentM.PropertyM.CategoryCode == "0" ||
            //    DmComponentM.PropertyM.CategoryCode == null)
            //{
            //    //Demo_ChartData chartSampleData = new Demo_ChartData();
            //    //DmComponentM.DataGridList = chartSampleData.SetDetailInfoData();
            //}
            //else
            //{
            //    SetMasterData();
            //}
        }
    }
}

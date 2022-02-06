using Coever.Lib.CoPlatform.Scenario.Core;
using PrismWPF.Common.MVVM;
using System;
using System.Collections.Generic;

namespace DMS.Module.Map.Model
{
    public class AlarmMonitoringTargetModel : DMViewModelBase
    {
        private long? _CollectionID;
        public long? CollectionID
        {
            get => _CollectionID;
            set
            {
                _CollectionID = value;
                RaisePropertyChanged("CollectionID");
            }
        }

        private DateTime _AlarmDate;
        public DateTime AlarmDate
        {
            get => _AlarmDate;
            set
            {
                _AlarmDate = value;
                RaisePropertyChanged("AlarmDate");
            }
        }

        private string _ResourceCode;
        public string ResourceCode
        {
            get => _ResourceCode;
            set
            {
                _ResourceCode = value;
                RaisePropertyChanged("ResourceCode");
            }
        }

        private string _ResourceName;
        public string ResourceName
        {
            get => _ResourceName;
            set
            {
                _ResourceName = value;
                RaisePropertyChanged("ResourceName");
            }
        }

        private int? _AlarmLevel;
        public int? AlarmLevel
        {
            get => _AlarmLevel;
            set
            {
                _AlarmLevel = value;
                RaisePropertyChanged("AlarmLevel");
            }
        }

        private int? _ScenarioCode;
        public int? ScenarioCode
        {
            get => _ScenarioCode;
            set
            {
                _ScenarioCode = value;
                RaisePropertyChanged("ScenarioCode");
            }
        }

        private string _ScenarioName;
        public string ScenarioName
        {
            get => _ScenarioName;
            set
            {
                _ScenarioName = value;
                RaisePropertyChanged("ScenarioName");
            }
        }

        public string ScenarioActions { get; set; }

        private List<IActionModel> _Actions = new List<IActionModel>();
        public List<IActionModel> Actions
        {
            get => _Actions;
            set
            {
                _Actions = value;
                RaisePropertyChanged("Actions");
            }
        }
    }
}

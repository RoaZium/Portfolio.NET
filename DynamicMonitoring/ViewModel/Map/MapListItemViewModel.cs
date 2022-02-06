using DynamicMonitoring.Common;
using DynamicMonitoring.Common.MVVM;
using DynamicMonitoring.Model;
using DynamicMonitoring.Resources;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DynamicMonitoring.ViewModel
{
    public class MapListItemViewModel : DMViewModelBase
    {
        #region Properties
        private bool _IsExpanded;
        public bool IsExpanded
        {
            get
            {
                if (Model == null || Model.MapType == "layout")
                {
                    return false;
                }
                return _IsExpanded;
            }
            set
            {
                if (Model != null && Model.MapType == "folder")
                {
                    _IsExpanded = value;
                    RaisePropertyChanged("IsExpanded");
                }
            }
        }

        private bool _IsEditMode;
        public bool IsEditMode
        {
            get { return _IsEditMode; }
            set
            {
                _IsEditMode = value;
                RaisePropertyChanged("IsEditMode");
            }
        }

        public ImageSource Glyph
        {
            get
            {
                if (Model == null)
                {
                    return null;
                }

                return Common.ObjectConverter.ToImageSource(
                    Model.MapType == "folder" ?
                    (IsExpanded ? Res.ImgOpenedFolder : Res.ImgFolder)
                    : Res.ImgPage);
            }
        }

        public string ID
        {
            get { return Model == null ? null : Model.MapID; }
            set
            {
                if (Model != null)
                {
                    Model.MapID = value;
                    RaisePropertyChanged("ID");
                }
            }
        }

        public string ParentID
        {
            get { return Model == null ? null : Model.ParentID; }
            set
            {
                if (Model != null)
                {
                    Model.ParentID = value;
                    RaisePropertyChanged("ParentID");
                }
            }
        }

        private MapModel _Model;
        public MapModel Model
        {
            get { return _Model; }
            set
            {
                _Model = value;
                RaisePropertyChanged("Model");
            }
        } 
        #endregion

        #region Commands
        public DelegateCommand ModifiedMapCommand
        {
            get { return new DelegateCommand(ModifiedMapCommandExecute); }
        }

        private void ModifiedMapCommandExecute()
        {
            DBHelper.ModifyMap(ID, ParentID, Model.MapName);
        } 
        #endregion
    }
}
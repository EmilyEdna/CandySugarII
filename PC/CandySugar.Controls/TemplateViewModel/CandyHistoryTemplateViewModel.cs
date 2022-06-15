using CandySugar.Logic.Entity.CandyEntity;
using CandySugar.Logic.IService;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.TemplateViewModel
{
    public class CandyHistoryTemplateViewModel : PropertyChangedBase
    {
        public IContainer Container;
        public IWindowManager WindowManager;

        public CandyHistoryTemplateViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.XS= this.LXS = this.DM = this.HDM = this.MH = this.JY = false;
            this.CandyNovel = Container.Get<ICandyNovel>();
        }

        #region Field
        private ICandyNovel CandyNovel;
        #endregion

        #region CommomProperty_Int
        private bool _XS;
        public bool XS
        {
            get => _XS;
            set => SetAndNotify(ref _XS, value);
        }

        private bool _LXS;
        public bool LXS
        {
            get => _LXS;
            set => SetAndNotify(ref _LXS, value);
        }

        private bool _DM;
        public bool DM
        {
            get => _DM;
            set => SetAndNotify(ref _DM, value);
        }

        private bool _HDM;
        public bool HDM
        {
            get => _HDM;
            set => SetAndNotify(ref _HDM, value);
        }

        private bool _MH;
        public bool MH
        {
            get => _MH;
            set => SetAndNotify(ref _MH, value);
        }

        private bool _JY;
        public bool JY
        {
            get => _JY;
            set => SetAndNotify(ref _JY, value);
        }
        #endregion

        #region Property
        private ObservableCollection<CandyNovel> _CandyNovelResult;
        public ObservableCollection<CandyNovel> CandyNovelResult
        {
            get => _CandyNovelResult;
            set => SetAndNotify(ref _CandyNovelResult, value);
        }
        #endregion
        #region Action
        public void ChangeAction(string input)
        {
            switch (input)
            {
                case "XS":
                    this.XS = true;
                    this.LXS = this.DM = this.HDM = this.MH = this.JY = false;
                    InitNovel();
                    break;
                case "LXS":
                    this.LXS = true;
                    this.XS = this.DM = this.HDM = this.MH = this.JY = false;
                    break;
                case "DM":
                    this.DM = true;
                    this.XS = this.LXS = this.HDM = this.MH = this.JY = false;
                    break;
                case "HDM":
                    this.HDM = true;
                    this.XS = this.LXS = this.DM = this.MH = this.JY = false;
                    break;
                case "MH":
                    this.MH = true;
                    this.XS = this.LXS = this.DM = this.HDM = this.JY = false;
                    break;
                default:
                    this.JY = true;
                    this.XS = this.LXS = this.DM = this.HDM = this.MH = false;
                    break;
            }
        }
        #endregion
        #region Method
        private async void InitNovel()
        {
            CandyNovelResult = new ObservableCollection<CandyNovel>(await this.CandyNovel.Get());
        }
        #endregion
    }
}

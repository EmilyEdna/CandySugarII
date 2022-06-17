using CandySugar.Logic.Entity.CandyEntity;
using CandySugar.Logic.IService;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.MenuTemplateViewModel
{
    public class CandyImageTemplateViewModel : PropertyChangedBase
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public ICandyLabel CandyLabel;
        public CandyImageTemplateViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            CandyLabel = Container.Get<ICandyLabel>();
            InitGet();
        }
        #region Property
        private string _EnLabel;
        public string EnLabel
        {
            get => _EnLabel;
            set => SetAndNotify(ref _EnLabel, value);
        }
        private string _ZhLabel;
        public string ZhLabel
        {
            get => _ZhLabel;
            set => SetAndNotify(ref _ZhLabel, value);
        }
        private ObservableCollection<CandyLabel> _Label;
        public ObservableCollection<CandyLabel> Label
        {
            get => _Label;
            set => SetAndNotify(ref _Label, value);
        }
        #endregion

        #region Method
        private async void InitSave()
        {
            await this.CandyLabel.AddOrUpdate(new CandyLabel
            {
                EnLabel = this.EnLabel,
                ZhLabel = this.ZhLabel
            });
        }
        private async void InitGet()
        {
            this.Label = new ObservableCollection<CandyLabel>(await this.CandyLabel.Get());
        }
        private async void InitRemove(CandyLabel input)
        {
            await this.CandyLabel.Remove(input);
        }
        #endregion 

        #region Action
        public void SaveAction()
        {
            InitSave();
            InitGet();
        }
        public void RemoveAction(CandyLabel input)
        {
            InitRemove(input);
            InitGet();
        }
        #endregion
    }
}

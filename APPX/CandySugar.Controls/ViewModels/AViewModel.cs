using CandySugar.Library;
using Sdk.Component.Bgm.sdk;
using Sdk.Component.Bgm.sdk.ViewModel;
using Sdk.Component.Bgm.sdk.ViewModel.Enums;
using Sdk.Component.Bgm.sdk.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls
{
    public class AViewModel : ViewModelBase
    {
        public AViewModel(BaseServices baseServices) : base(baseServices)
        {
        }
        public override void OnLoad()
        {
            Init();
        }

        #region Property
        ObservableCollection<BgmInitResult> _Result;
        public ObservableCollection<BgmInitResult> Result
        {
            get => _Result;
            set => SetProperty(ref _Result, value);
        }
        #endregion

        #region Method
        async void Init()
        {
            this.Activity = true;
            var result = await BgmFactory.Bgm(opt =>
             {
                 opt.RequestParam = new Input
                 {
                     CacheSpan = DataBus.Cache,
                     ImplType = DataCenter.ImplType(),
                     BgmType = BgmEnum.Calendar,
                 };
             }).RunsAsync();
            Result = new ObservableCollection<BgmInitResult>(result.InitResults);
            this.Activity = false;
        }
        #endregion
    }
}

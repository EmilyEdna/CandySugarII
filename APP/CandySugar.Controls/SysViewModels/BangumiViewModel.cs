using Sdk.Component.Bgm.sdk.ViewModel.Response;
using Sdk.Component.Bgm.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdk.Component.Bgm.sdk.ViewModel;
using Sdk.Component.Bgm.sdk.ViewModel.Enums;

namespace CandySugar.Controls.SysViewModels
{
    public class BangumiViewModel : BaseViewModel
    {
        public BangumiViewModel()
        {
            Task.Run(() => InitBgm());
        }

        #region 属性
         ObservableCollection<BgmInitResult> _BgmResult;
        public ObservableCollection<BgmInitResult> BgmResult
        {
            get => _BgmResult;
            set => SetProperty(ref _BgmResult, value);
        }
        #endregion

        #region 方法
        async void InitBgm() 
        {
            var result = await BgmFactory.Bgm(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = CandySoft.Cache,
                    Proxy = StaticResource.Proxy(),
                    ImplType = StaticResource.ImplType(),
                    BgmType = BgmEnum.Calendar,
                };
            }).RunsAsync();
            BgmResult = new ObservableCollection<BgmInitResult>(result.InitResults);
        }
        #endregion
    }
}

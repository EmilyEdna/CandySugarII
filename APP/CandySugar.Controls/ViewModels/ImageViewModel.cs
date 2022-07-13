using CandySugar.Library;
using Sdk.Component.Image.sdk;
using Sdk.Component.Image.sdk.ViewModel;
using Sdk.Component.Image.sdk.ViewModel.Enums;
using Sdk.Component.Image.sdk.ViewModel.Request;
using Sdk.Component.Image.sdk.ViewModel.Response;
using System.Net.Http;

namespace CandySugar.Controls.ViewModels
{
    public class ImageViewModel : BaseViewModel
    {
        readonly int Limit = 10;
        public ImageViewModel()
        {
            this.Page = 1;
            InitImage();
        }

        #region 属性
        string _KeyWord;
        public string KeyWord
        {
            get => _KeyWord;
            set
            {
                SetProperty(ref _KeyWord, value);
            }
        }
        ObservableCollection<ImageElementResult> _ElementResults;
        public ObservableCollection<ImageElementResult> ElementResults
        {
            get => _ElementResults;
            set => SetProperty(ref _ElementResults, value);
        }
        #endregion

        #region 命令 
        public DelegateCommand QueryAction => new(() => { });
        public DelegateCommand LoadMoreAction => new(() => { });
        #endregion

        #region 方法
        async void InitImage()
        {
            if (IsBusy) return;
            try
            {
                if (CandySoft.NetState.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("网络异常！", "请检查网络是否通畅！", "是");
                    return;
                }
                ShowBusy();
                await Task.Delay(CandySoft.Wait);
                var result = await ImageFactory.Image(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        ImageType = ImageEnum.Init,
                        Init = new ImageInit
                        {
                            Page = Page,
                            Limit = Limit,
                            Tag = StaticResource.ImageModule()
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                Total = result.GlobalResult.Total;
                ElementResults = new ObservableCollection<ImageElementResult>(result.GlobalResult.Result);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("错误！", ex.Message, "是");
            }
        }
#endregion
    }
}

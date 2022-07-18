using CandySugar.Library;
using Sdk.Component.Manga.sdk.ViewModel;
using Sdk.Component.Manga.sdk;
using Sdk.Component.Manga.sdk.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdk.Component.Manga.sdk.ViewModel.Enums;
using Sdk.Component.Manga.sdk.ViewModel.Request;
using Prism.Navigation.Xaml;
using CandySugar.Controls.Views.MangaViews;

namespace CandySugar.Controls.ViewModels.MangaViewModels
{
    public class MangaChapterViewModel : BaseViewModel
    {
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Title = query["Name"].ToString();
            DetailResult = new ObservableCollection<MangaChapterDetailResult>(query["Chapter"] as List<MangaChapterDetailResult>);
        }
        #region 属性
        string _Title;
        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }
        ObservableCollection<MangaChapterDetailResult> _DetailResult;
        public ObservableCollection<MangaChapterDetailResult> DetailResult
        {
            get => _DetailResult;
            set => SetProperty(ref _DetailResult, value);
        }
        #endregion

        #region 命令
        public DelegateCommand<MangaChapterDetailResult> ViewAction => new(input =>
        {
            InitContent(input.Route);
        });
        #endregion

        #region 方法
        async void InitContent(string input)
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
                await Task.Delay(CandySoft.Wait / 2);
                var result = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        MangaType = MangaEnum.Content,
                        Content = new MangaContent
                        {
                            Route = input
                        }
                    };
                }).RunsAsync();
                await Task.Delay(CandySoft.Wait / 2);
                var bytes = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        MangaType = MangaEnum.Download,
                        Down = new MangaBytes
                        {
                            Route = result.ContentResult.Route,
                            CacheKey = result.ContentResult.CacheKey
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                Navigation(bytes.DwonResult.Bytes);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("错误！", ex.Message, "是");
            }
        }
        async void Navigation(List<byte[]> input) 
        {
            await Shell.Current.GoToAsync(nameof(MangaWatchView), new Dictionary<string, object> { { "Result", input } });
        }
        #endregion
    }
}

using Sdk.Component.Novel.sdk;
using Sdk.Component.Novel.sdk.ViewModel;
using Sdk.Component.Novel.sdk.ViewModel.Enums;
using Sdk.Component.Novel.sdk.ViewModel.Request;
using Sdk.Component.Novel.sdk.ViewModel.Response;
using System.Linq;

namespace CandySugar.Controls.ViewModels.NovelViewModels
{
    public class NovelContentViewModel : BaseViewModel
    {
        ICandyService Service;
        public NovelContentViewModel()
        {
            Service = CandyContainer.Instance.Resolves<ICandyService>();
        }

        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var detail = query["Key"] as NovelDetailElementResult;
            DetailResult = query["Book"] as NovelDetailRootResult;
            InitContent(detail.ChapterRoute);
        }

        #region 属性
        ObservableCollection<string> _Content;
        public ObservableCollection<string> Content
        {
            get { return _Content; }
            set { SetProperty(ref _Content, value); }
        }

        NovelDetailRootResult _DetailResult;
        public NovelDetailRootResult DetailResult
        {
            get => _DetailResult;
            set => SetProperty(ref _DetailResult, value);
        }
        string _ChapterName;
        public string ChapterName
        {
            get { return _ChapterName; }
            set { SetProperty(ref _ChapterName, value); }
        }
        string _Next;
        public string Next
        {
            get { return _Next; }
            set { SetProperty(ref _Next, value); }
        }
        #endregion

        #region 方法
        async void InitContent(string input)
        {
            if (IsBusy) return;
            try
            {
                if (CandySoft.NetState.NetworkAccess != NetworkAccess.Internet)
                {
                    StaticResource.PopToast("请检查网络是否通畅!");
                    return;
                }
                ShowBusy();
                await Task.Delay(CandySoft.Wait);
                var result = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        NovelType = NovelEnum.View,
                        View = new NovelView
                        {
                            Route = input
                        }
                    };
                }).RunsAsync();

                this.ChapterName = result.ContentResult.ChapterName;

                Next = result.ContentResult.NextPage.IsNullOrEmpty() ? result.ContentResult.NextChapter : result.ContentResult.NextPage;
                result.ContentResult.Content = result.ContentResult.Content.Replace("　", "\t");

                if (this.Content == null)
                    this.Content = new ObservableCollection<string>(new List<string>());

                result.ContentResult.Content.Split("\t", StringSplitOptions.RemoveEmptyEntries).ForEnumerEach(t =>
                {
                    this.Content.Add("\t\t\t\t\t" + t + "\r\n");
                });
                CloseBusy();
                Logic(ChapterName, Next);
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        void Logic(string ChapterName, string ChapterRoute)
        {
            var Model = DetailResult.ToMapest<CandyNovel>();
            Model.Route = ChapterRoute;
            Model.Chapter = ChapterName;
            Service.AddOrAlterNovel(Model);
        }
        #endregion

        #region 命令
        public DelegateCommand LoadMoreAction => new(() =>
        {
            if (Content.Count > 10000)
            {
                var temp = Content.ToList();
                temp.RemoveRange(0, 10000);
                Content = new ObservableCollection<string>(temp);
            }
            if (!Next.IsNullOrEmpty())
                InitContent(Next);
        });
        #endregion
    }
}

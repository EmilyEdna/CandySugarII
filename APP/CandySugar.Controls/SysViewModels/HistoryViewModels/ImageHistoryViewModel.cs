using CandySugar.Controls.Views.ImageViews;
using Sdk.Component.Image.sdk;
using Sdk.Component.Image.sdk.ViewModel;
using Sdk.Component.Image.sdk.ViewModel.Enums;
using Sdk.Component.Image.sdk.ViewModel.Request;


namespace CandySugar.Controls.SysViewModels.HistoryViewModels
{
    public class ImageHistoryViewModel : BaseViewModel
    {
        ICandyService CandyService;
        public ImageHistoryViewModel()
        {
            this.Page = 1;
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
            Task.Run(() => Query());
        }

        #region 属性
        ObservableCollection<CandyImage> _Root;
        public ObservableCollection<CandyImage> Root
        {
            get => _Root;
            set => SetProperty(ref _Root, value);
        }
        #endregion

        #region 方法
        void Query()
        {
            var result = CandyService.GetImage(this.Page);
            Total = result.Total;
            if (Root == null)
                Root = new ObservableCollection<CandyImage>(result.Result);
            else
                result.Result.ForEach(item =>
                {
                    Root.Add(item);
                });
        }
        async void Navigation(string input)
        {
            await Shell.Current.GoToAsync($"{nameof(ImageDetailView)}?Key={input}");
        }

        void Remove(CandyImage input)
        {
            StaticResource.PopComfirm("确认删除", nameof(ImageHistoryViewModel));
            MessagingCenter.Subscribe<ComfirmViewModel, bool>(this, nameof(ImageHistoryViewModel), (sender, args) =>
            {
                if (args == true)
                {
                    CandyService.RemoveImage(input);
                    var temp = Root.ToList();
                    temp.RemoveAll(t => t.CandyId == input.CandyId);
                    Root = new ObservableCollection<CandyImage>(temp);
                }
            });
        }

        async Task<byte[]> Download(string input)
        {
            return (await ImageFactory.Image(opt =>
             {
                 opt.RequestParam = new Input
                 {
                     CacheSpan = CandySoft.Cache,
                     Proxy = StaticResource.Proxy(),
                     ImplType = StaticResource.ImplType(),
                     ImageType = ImageEnum.Download,
                     Download = new ImageDownload
                     {
                         Route = input
                     }
                 };
             }).RunsAsync()).DownResult.Bytes;
        }

        async void InitDownload(string input)
        {    
            var bytes = await Download(input);
            var Directory = SyncStatic.CreateDir(Path.Combine(ICrossExtension.Instance.AndriodPath, "CandyDown", "Wallpaper"));
            var Files = SyncStatic.CreateFile(Path.Combine(Directory, input.Split("/").LastOrDefault()));
            var WriteResult = SyncStatic.WriteFile(bytes, Files);
            if (!WriteResult.IsNullOrEmpty())
                StaticResource.PopToast("下载完成!");
        }
        #endregion

        #region 命令
        public DelegateCommand LoadMoreAction => new(() =>
        {
            this.Page += 1;
            if (this.Page > Total) return;
            Query();
        });

        public DelegateCommand<CandyImage> ViewAction => new(input =>
        {
            Navigation(input.Original);
        });

        public DelegateCommand<CandyImage> DownAction => new(input =>
        {
            Task.Run(() => StaticResource.PopToast("开始下载!"));
            InitDownload(input.Original);
        });

        public DelegateCommand<CandyImage> RemoveAction => new(input => Remove(input));
        #endregion
    }
}

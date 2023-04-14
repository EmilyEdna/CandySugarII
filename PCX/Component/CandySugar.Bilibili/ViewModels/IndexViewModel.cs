using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Bilibili.ViewModels
{
    public class IndexViewModel : PropertyChangedBase
    {
        #region Property
        private string _Route;
        public string Route
        {
            get => _Route;
            set => SetAndNotify(ref _Route, value);
        }

        private BiliVideoInfoResult _InfoResult;
        /// <summary>
        /// 视频信息
        /// </summary>
        public BiliVideoInfoResult InfoResult
        {
            get => _InfoResult;
            set => SetAndNotify(ref _InfoResult, value);
        }
        #endregion

        #region Command
        public void CookieCommand()
        {

        }
        public void QeuryCommand()
        {
            if (Route.IsNullOrEmpty()) return;
            OnInitVideo();
        }
        public void HandleCommand(string key)
        {
            var condition = key.AsInt();
            if (condition == 1) OnDownload();
            if (condition == 2) { }
            if (condition == 3) { }
            if (condition == 4) { }
        }
        #endregion

        #region Method
        private void OnDownload()
        {
            Task.Run(async () =>
            {
                var bytes = await new HttpClient().GetByteArrayAsync(InfoResult.Cover);
                bytes.FileCreate(InfoResult.Title, FileTypes.Jpg, "Bilibili", (catalog, fileName) =>
                {
                    Application.Current.Dispatcher.Invoke(() => new ScreenDownNofityView(CommonHelper.DownloadFinishInformation, catalog).Show());
                });
            });
        }
        private void OnInitVideo()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await BilibiliFactory.Bili(opt =>
                     {
                         opt.RequestParam = new Input
                         {
                             BiliType = BiliEnum.VideoInfo,
                             CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                             ImplType = SdkImpl.Rest,
                             VideoInfo = new BiliVideoInfo
                             {
                                 Route = Route,
                             }
                         };
                     }).RunsAsync()).InfoResult;
                    InfoResult = result;
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    ErrorNotify();
                }
            });
        }
        private void ErrorNotify()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                new ScreenNotifyView(CommonHelper.ComponentErrorInformation).Show();
            });
        }
        #endregion
    }
}

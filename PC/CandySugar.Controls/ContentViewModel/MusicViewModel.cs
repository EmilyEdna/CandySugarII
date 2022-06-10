using Sdk.Component.Music.sdk.ViewModel;
using Sdk.Component.Music.sdk;
using Sdk.Component.Music.sdk.ViewModel.Enums;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CandySugar.Library;
using Sdk.Component.Music.sdk.ViewModel.Request;
using Sdk.Component.Music.sdk.ViewModel.Response;
using System.Collections.ObjectModel;
using HandyControl.Data;
using CandySugar.Resource.Properties;

namespace CandySugar.Controls.ContentViewModel
{
    public class MusicViewModel : Screen
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public MusicViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.PlatformType = PlatformEnum.NeteaseMusic;
            this.Handle = new Dictionary<PlatformEnum, string>
            {
                { PlatformEnum.QQMusic,"QQ"},
                { PlatformEnum.NeteaseMusic,"网易"},
                { PlatformEnum.KuGouMusic,"酷狗"},
                { PlatformEnum.KuWoMusic,"酷我"},
                { PlatformEnum.BiliBiliMusic,"B站"},
                { PlatformEnum.MiGuMusic,"咪咕"}
            };
            this.ChangeType = 0;
            OnViewLoaded();
        }
        #region Field
        private PlatformEnum PlatformType;
        private string QueryWord;
        private int ChangeType;
        #endregion

        #region CommomProperty_Bool
        private bool _Loading;
        public bool Loading
        {
            get => _Loading;
            set => SetAndNotify(ref _Loading, value);
        }
        #endregion

        #region ComomProperty_Int
        private int? _Total;
        public int? Total
        {
            get => _Total;
            set => SetAndNotify(ref _Total, value);
        }
        private int _Page;
        public int Page
        {
            get => _Page;
            set => SetAndNotify(ref _Page, value);
        }
        #endregion

        #region Property
        public Dictionary<PlatformEnum, string> Handle { get; set; }
        private ObservableCollection<MusicSongElementResult> _ItemResult;
        /// <summary>
        /// 单曲
        /// </summary>
        public ObservableCollection<MusicSongElementResult> ItemResult
        {
            get => _ItemResult;
            set => SetAndNotify(ref _ItemResult, value);
        }
        private ObservableCollection<MusicSheetElementResult> _ItemsResult;
        /// <summary>
        /// 歌单
        /// </summary>
        public ObservableCollection<MusicSheetElementResult> ItemsResult
        {
            get => _ItemsResult;
            set => SetAndNotify(ref _ItemsResult, value);
        }
        private ObservableCollection<MusicSongElementResult> _AlbumResult;
        /// <summary>
        /// 专辑
        /// </summary>
        public ObservableCollection<MusicSongElementResult> AlbumResult
        {
            get => _AlbumResult;
            set => SetAndNotify(ref _AlbumResult, value);
        }
        private MusicSheetDetailRootResult _DetailResult;
        /// <summary>
        /// 详情
        /// </summary>
        public MusicSheetDetailRootResult DetailResult
        {
            get => _DetailResult;
            set => SetAndNotify(ref _DetailResult, value);
        }
        #endregion

        #region Override
        protected override void OnViewLoaded()
        {
            SearchAction("我是真的爱上你");
        }
        #endregion

        #region Action
        public void SearchAction(string input)
        {
            this.QueryWord = input;
            InitSearch(input);
        }
        public void CategoryAction(PlatformEnum input)
        {
            this.Page = 1;
            this.PlatformType = input;
            if (this.ChangeType == 0)
                InitSearch(this.QueryWord);
            else if (this.ChangeType == 1)
                InitQuery(this.QueryWord);
        }
        public void ChangeAction(string input)
        {
            this.Page = 1;
            if (input.Equals("单曲"))
            {
                this.ChangeType = 0;
                InitSearch(this.QueryWord);
            }
            else if (input.Equals("歌单"))
            {
                this.ChangeType = 1;
                InitQuery(this.QueryWord);
            }
            //列表
            else
            {
                this.ChangeType = 2;
            }
        }
        public void PageAction(FunctionEventArgs<int> input)
        {
            this.Page = input.Info;
            if (this.ChangeType == 0)
                InitSearch(this.QueryWord);
            else if (this.ChangeType == 1)
                InitQuery(this.QueryWord);
        }
        public void PlayListAction(string input)
        {
            var x = input;
        }
        public void LinkAlbumAction(string input)
        {
            InitAlbum(input);
        }
        public void ShowSheetAction(dynamic input)
        {
            InitDetail(input.ToString());
        }
        #endregion

        #region Method
        private async void InitSearch(string input)
        {
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var MusicQueryData = await MusicFactory.Music(opt =>
            {
                opt.RequestParam = new Input
                {
                    PlatformType = this.PlatformType,
                    CacheSpan = CandySoft.Default.Cache,
                    MusicType = MusicEnum.Song,
                    ImplType = StaticResource.ImplType(),
                    Search = new MusicSearch
                    {
                        KeyWord = input
                    }
                };
            }).RunsAsync();
            this.Loading = false;
            Total = MusicQueryData.SongResult.Total;
            ItemResult = new ObservableCollection<MusicSongElementResult>(MusicQueryData.SongResult.ElementResults);
        }
        private async void InitQuery(string input)
        {
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var MusicQueryData = await MusicFactory.Music(opt =>
            {
                opt.RequestParam = new Input
                {
                    PlatformType = this.PlatformType,
                    CacheSpan = CandySoft.Default.Cache,
                    MusicType = MusicEnum.Sheet,
                    ImplType = StaticResource.ImplType(),
                    Search = new MusicSearch
                    {
                        KeyWord = input
                    }
                };
            }).RunsAsync();
            this.Loading = false;
            Total = MusicQueryData.SheetResult.Total;
            ItemsResult = new ObservableCollection<MusicSheetElementResult>(MusicQueryData.SheetResult.ElementResults);
        }
        private async void InitAlbum(string input)
        {
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var MusicAlbumData = await MusicFactory.Music(opt =>
            {
                opt.RequestParam = new Input
                {
                    PlatformType = this.PlatformType,
                    CacheSpan = CandySoft.Default.Cache,
                    MusicType = MusicEnum.AlbumDetail,
                    ImplType = StaticResource.ImplType(),
                    AlbumDetail = new MusicAlbumDetail
                    {
                        AlbumId = input
                    }
                };
            }).RunsAsync();
            this.Loading = false;
            AlbumResult = new ObservableCollection<MusicSongElementResult>(MusicAlbumData.AlbumResult.ElementResults);
        }
        private async void InitDetail(string input)
        {
            this.Loading = true;
            await Task.Delay(CandySoft.Default.WaitSpan);
            var MusicDetailData = await MusicFactory.Music(opt =>
            {
                opt.RequestParam = new Input
                {
                    PlatformType = this.PlatformType,
                    MusicType = MusicEnum.SheetDetail,
                    ImplType = StaticResource.ImplType(),
                    CacheSpan=CandySoft.Default.Cache,
                    SheetDetail = new MusicSheetDetail
                    {
                        Id = input
                    }
                };
            }).RunsAsync();
            this.Loading = false;
            DetailResult = MusicDetailData.SheetDetailResult;
        }
        #endregion
    }
}

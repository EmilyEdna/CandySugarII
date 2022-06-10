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
using CandySugar.Logic.Entity.CandyEntity;
using CandySugar.Logic.IService;

namespace CandySugar.Controls.ContentViewModel
{
    public class MusicViewModel : Screen
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public ICandyMusic CandyMusic;
        public MusicViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.CandyMusic = Container.Get<ICandyMusic>();
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
            this.Page = 1;
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
        private ObservableCollection<CandyMusicList> _CandyList;
        /// <summary>
        /// 播放列表
        /// </summary>
        public ObservableCollection<CandyMusicList> CandyList
        {
            get => _CandyList;
            set => SetAndNotify(ref _CandyList, value);
        }
        #endregion

        #region Override
        protected override void OnViewLoaded()
        {

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
                InitPlayList();
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
        public void PlayListAction(Dictionary<object, object> input)
        {
            InitList(input);
        }
        public void LinkAlbumAction(string input)
        {
            InitAlbum(input);
        }

        public void AlbumAction(string input)
        {
            this.PlatformType = (PlatformEnum)CandyList.FirstOrDefault(t => t.AlbumId == input).Platform;
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
                        Page=this.Page,
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
                        Page=this.Page,
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
                    CacheSpan = CandySoft.Default.Cache,
                    SheetDetail = new MusicSheetDetail
                    {
                        Id = input
                    }
                };
            }).RunsAsync();
            this.Loading = false;
            DetailResult = MusicDetailData.SheetDetailResult;
        }
        private async void InitPlayList()
        {
            CandyList = new ObservableCollection<CandyMusicList>(await this.CandyMusic.Get());
        }
        private async void InitList(Dictionary<object, object> input)
        {
            CandyMusicList CandyList = null;
            if (input.Keys.FirstOrDefault().ToString().Equals("Single"))
                CandyList = ItemResult.Where(t => t.SongId == input.Values.FirstOrDefault().ToString()).Select(t => new CandyMusicList
                {
                    SongId = t.SongId,
                    AlbumId = t.SongAlbumId,
                    SongName = t.SongName,
                    AlbumName = t.SongAlbumName,
                    SongArtist = String.Join(",", t.SongArtistName),
                    Platform = (int)this.PlatformType
                }).FirstOrDefault();
            else if (input.Keys.FirstOrDefault().ToString().Equals("Detail"))
                CandyList = DetailResult.ElementResults.Where(t => t.SongId == input.Values.FirstOrDefault().ToString()).Select(t => new CandyMusicList
                {
                    SongId = t.SongId,
                    AlbumId = t.SongAlbumId,
                    SongName = t.SongName,
                    AlbumName = t.SongAlbumName,
                    SongArtist = String.Join(",", t.SongArtistName),
                    Platform = (int)this.PlatformType
                }).FirstOrDefault();
            else
                CandyList = AlbumResult.Where(t => t.SongId == input.Values.FirstOrDefault().ToString()).Select(t => new CandyMusicList
                {
                    SongId = t.SongId,
                    AlbumId = t.SongAlbumId,
                    SongName = t.SongName,
                    AlbumName = t.SongAlbumName,
                    SongArtist = String.Join(",", t.SongArtistName),
                    Platform = (int)this.PlatformType
                }).FirstOrDefault();
            await this.CandyMusic.Add(CandyList);
        }
        #endregion
    }
}

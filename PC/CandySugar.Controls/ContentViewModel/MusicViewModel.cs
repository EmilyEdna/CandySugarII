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
using System.IO;
using XExten.Advance.HttpFramework.MultiFactory;
using HandyControl.Controls;
using CandySugar.Library.AudioTemplate;
using System.Timers;
using XExten.Advance.LinqFramework;
using NAudio.Wave;

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
            this.PlayMoudle = 1;
            this.ChangeType = 0;
            this.Index = 0;
            this.IsPlay = true;
            this.NonPlay = false;
            OnViewLoaded();
        }
        #region Field
        public AudioFactory AudioFactory;
        private PlatformEnum PlatformType;
        private string QueryWord;
        private int ChangeType;
        //1-列表循环；0-单曲循环
        private int PlayMoudle;
        private Timer timer;
        private int Index;
        #endregion

        #region CommomProperty_Bool
        private bool _Loading;
        public bool Loading
        {
            get => _Loading;
            set => SetAndNotify(ref _Loading, value);
        }
        private bool _IsPlay;
        public bool IsPlay
        {
            get => _IsPlay;
            set => SetAndNotify(ref _IsPlay, value);
        }
        private bool _NonPlay;
        public bool NonPlay
        {
            get => _NonPlay;
            set => SetAndNotify(ref _NonPlay, value);
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
        private double _CurrentSecond;
        public double CurrentSecond
        {
            get => _CurrentSecond;
            set => SetAndNotify(ref _CurrentSecond, value);
        }
        #endregion

        #region ComomProperty_str
        private string _Songer;
        public string Songer
        {
            get => _Songer;
            set => SetAndNotify(ref _Songer, value);
        }
        private string _SongName;
        public string SongName
        {
            get => _SongName;
            set => SetAndNotify(ref _SongName, value);
        }
        private string _CurrentSpan;
        public string CurrentSpan
        {
            get => _CurrentSpan;
            set => SetAndNotify(ref _CurrentSpan, value);
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
        private ObservableCollection<double> _Channel;
        /// <summary>
        /// 音频数据
        /// </summary>
        public ObservableCollection<double> Channel
        {
            get => _Channel;
            set => SetAndNotify(ref _Channel, value);
        }
        private AudioModel _Audio;
        public AudioModel Audio
        {
            get => _Audio;
            set => SetAndNotify(ref _Audio, value);
        }
        private ObservableCollection<MusicLyricElemetResult> _LyricResult;
        /// <summary>
        /// 歌词
        /// </summary>
        public ObservableCollection<MusicLyricElemetResult> LyricResult
        {
            get => _LyricResult;
            set => SetAndNotify(ref _LyricResult, value);
        }
        #endregion

        #region Override
        protected override void OnViewLoaded()
        {
            timer = new Timer
            {
                Interval = 3000
            };
            InitPlayList();
        }
        #endregion

        #region Action
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="input"></param>
        public void SearchAction(string input)
        {
            this.QueryWord = input;
            InitSearch(input);
        }
        /// <summary>
        /// 切换平台
        /// </summary>
        /// <param name="input"></param>
        public void CategoryAction(PlatformEnum input)
        {
            this.Page = 1;
            this.PlatformType = input;
            if (this.ChangeType == 0)
                InitSearch(this.QueryWord);
            else if (this.ChangeType == 1)
                InitQuery(this.QueryWord);
        }
        /// <summary>
        /// 切换条件
        /// </summary>
        /// <param name="input"></param>
        public void ChangeAction(string input)
        {
            this.Page = 1;
            if (input.Equals("单曲") && !this.QueryWord.IsNullOrEmpty())
            {
                this.ChangeType = 0;
                InitSearch(this.QueryWord);
            }
            else if (input.Equals("歌单") && !this.QueryWord.IsNullOrEmpty())
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
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input"></param>
        public void PageAction(FunctionEventArgs<int> input)
        {
            this.Page = input.Info;
            if (this.ChangeType == 0)
                InitSearch(this.QueryWord);
            else if (this.ChangeType == 1)
                InitQuery(this.QueryWord);
        }
        /// <summary>
        /// 添加到播放列表
        /// </summary>
        /// <param name="input"></param>
        public void PlayListAction(Dictionary<object, object> input)
        {
            InitList(input);
            Growl.Info("Add Success!");
        }
        /// <summary>
        /// 关联专辑
        /// </summary>
        /// <param name="input"></param>
        public void LinkAlbumAction(string input)
        {
            InitAlbum(input);
        }
        /// <summary>
        /// 从播放列表中关联专辑
        /// </summary>
        /// <param name="input"></param>
        public void AlbumAction(string input)
        {
            this.PlatformType = (PlatformEnum)CandyList.FirstOrDefault(t => t.AlbumId == input).Platform;
            InitAlbum(input);
        }
        /// <summary>
        /// 歌单详情
        /// </summary>
        /// <param name="input"></param>
        public void ShowSheetAction(dynamic input)
        {
            InitDetail(input.ToString());
        }
        /// <summary>
        /// 加载网络音乐到本地并播放
        /// </summary>
        public void DownPlayAction()
        {
            InitPlayList();

            this.IsPlay = false;
            this.NonPlay = true;
            if (AudioFactory == null)
                PlayConditon();
            else
                AudioFactory.PlayOut().Play();
        }
        /// <summary>
        /// 暂停
        /// </summary>
        public void PauseAciton()
        {
            this.IsPlay = true;
            this.NonPlay = false;
            AudioFactory.PlayOut().Pause();
        }
        /// <summary>
        /// 上一首
        /// </summary>
        public void SkipPreviousAciton()
        {
            Index -= 1;
            if (Index < 0) Index = CandyList.Count - 1;
            if (AudioFactory != null)
            {
                PlayConditon();
            }
        }
        /// <summary>
        /// 下一首
        /// </summary>
        public void SkipNextAciton()
        {
            Index += 1;
            if (Index > CandyList.Count - 1) Index = 0;
            if (AudioFactory != null)
            {
                PlayConditon();
            }
        }
        /// <summary>
        /// 切换播放模式
        /// </summary>
        /// <param name="input"></param>
        public void ChangeModuleAction(string input)
        {
            if (this.PlayMoudle != input.AsInt())
                this.PlayMoudle = input.AsInt();
            if (AudioFactory != null) //此时正在播放
            {
                if (this.PlayMoudle == 1) ListRuch();
                else Single();
            }
        }
        /// <summary>
        /// 删除列表
        /// </summary>
        /// <param name="input"></param>
        public void Remove(Guid input)
        {
            this.CandyMusic.Remove(CandyList.FirstOrDefault(t => t.CandyId == input));
            this.InitPlayList();
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
                        Page = this.Page,
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
                        Page = this.Page,
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
        private async void InitDownloadPlay(string input)
        {
            CandyMusicList candy = CandyList.FirstOrDefault(t => t.SongId == input);
            var Platform = (PlatformEnum)candy.Platform;
            if (!candy.IsComplete)
            {
                var MusicPlayData = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        PlatformType = Platform,
                        MusicType = MusicEnum.Route,
                        ImplType = StaticResource.ImplType(),
                        CacheSpan = CandySoft.Default.Cache,
                        Play = Platform == PlatformEnum.KuGouMusic ? new MusicPlaySearch
                        {
                            Dynamic = input,
                            KuGouAlbumId = candy.AlbumId,
                        } : new MusicPlaySearch
                        {
                            Dynamic = input,
                        }
                    };
                }).RunsAsync();
                if (MusicPlayData.PlayResult.CanPlay)
                {
                    var SongFile = $"{candy.SongName}({candy.AlbumName})-{candy.SongArtist}_{Platform}";
                    if (Platform == PlatformEnum.BiliBiliMusic)
                    {
                        candy.LocalRoute = StaticResource.Download(MusicPlayData.PlayResult.BilibiliFileBytes, Path.Combine("Music", candy.SongArtist), SongFile, "mp3");
                    }
                    else
                    {
                        var filebytes = IHttpMultiClient.HttpMulti.AddNode(opt => opt.NodePath = MusicPlayData.PlayResult.SongURL).Build().RunBytes().FirstOrDefault();
                        candy.LocalRoute = StaticResource.Download(filebytes, Path.Combine("Music", candy.SongArtist), SongFile, "mp3");
                        candy.NetRoute = MusicPlayData.PlayResult.SongURL;
                    }
                    await this.CandyMusic.Update(candy);
                    InitPlayList();
                }
                else
                    Growl.Info("当前歌曲已下架，请切换到其他其他平台搜索");
            }

            AudioFactory = AudioFactory.Instance.InitConfig(candy.LocalRoute, (data) =>
            {
                Channel = new ObservableCollection<double>(data.Item1);
                CurrentSpan = data.Item2;
                CurrentSecond = data.Item3;
            }).Run(data => Audio = data);
            InitLyric(candy);
        }
        private async void InitLyric(CandyMusicList candy)
        {
            var MusicLyricData = await MusicFactory.Music(opt =>
            {
                opt.RequestParam = new Input
                {
                    PlatformType = (PlatformEnum)candy.Platform,
                    MusicType = MusicEnum.Lyric,
                    ImplType = StaticResource.ImplType(),
                    CacheSpan = CandySoft.Default.Cache,
                    Lyric = new MusicLyricSearch
                    {
                        Dynamic = candy.SongId
                    }
                };
            }).RunsAsync();
            LyricResult = new ObservableCollection<MusicLyricElemetResult>(MusicLyricData.LyricResult.Lyrics);
        }
        #endregion

        #region  Commom
        private void PlayConditon()
        {
            var PlayNum = CandyList.Count;
            if (PlayNum <= 0)
            {
                Growl.Info("播放列表为添加任何歌曲！");
                return;
            }
            //列表循环
            if (PlayMoudle == 1)
            {
                InitDownloadPlay(CandyList[Index].SongId);
                SongName = CandyList[Index].SongName;
                Songer = CandyList[Index].SongArtist;
                ListRuch();
            }
            else
            {
                InitDownloadPlay(CandyList[Index].SongId);
                SongName = CandyList[Index].SongName;
                Songer = CandyList[Index].SongArtist;
                Single();
            }
        }
        /// <summary>
        /// 单曲循环
        /// </summary>
        private void Single()
        {
            timer.Elapsed -= ListRuchEvent;
            timer.Elapsed += SingleEvent;
            timer.Start();
        }
        /// <summary>
        /// 列表循环
        /// </summary>
        private void ListRuch()
        {
            timer.Elapsed -= SingleEvent;
            timer.Elapsed += ListRuchEvent;
            timer.Start();
        }
        private object SimpleLocker = new object();
        private void ListRuchEvent(object sender, ElapsedEventArgs e)
        {

            if (AudioFactory.PlayOut() != null && AudioFactory.PlayOut().PlaybackState == PlaybackState.Stopped)
            {
                var PlayNum = CandyList.Count;
                //播放完成
                if (CurrentSecond >= Audio.Seconds)
                {
                    Index += 1;
                    if (Index < PlayNum)
                    {
                        //播放下一首
                        lock (SimpleLocker)
                        {
                            InitDownloadPlay(CandyList[Index].SongId);
                        }
                        SongName = CandyList[Index].SongName;
                        Songer = CandyList[Index].SongArtist;
                    }
                    else
                    {
                        Index = 0;
                        lock (SimpleLocker)
                        {
                            InitDownloadPlay(CandyList[Index].SongId);
                        }
                        SongName = CandyList[Index].SongName;
                        Songer = CandyList[Index].SongArtist;
                    }
                }
            }
        }
        private void SingleEvent(object sender, ElapsedEventArgs e)
        {
            if (AudioFactory.PlayOut() != null && AudioFactory.PlayOut().PlaybackState == PlaybackState.Stopped)
            {
                //播放完成
                if (CurrentSecond >= Audio.Seconds)
                {
                    lock (SimpleLocker)
                    {
                        InitDownloadPlay(CandyList[Index].SongId);
                    }
                    SongName = CandyList[Index].SongName;
                    Songer = CandyList[Index].SongArtist;
                }
            }
        }
        #endregion
    }
}

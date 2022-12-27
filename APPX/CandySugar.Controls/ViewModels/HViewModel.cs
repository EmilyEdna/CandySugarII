using CandySugar.Library;
using Sdk.Component.Music.sdk.ViewModel;
using Sdk.Component.Music.sdk.ViewModel.Response;
using Sdk.Component.Music.sdk;
using Sdk.Component.Music.sdk.ViewModel.Enums;
using Sdk.Component.Music.sdk.ViewModel.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using XExten.Advance.LinqFramework;
using CandySugar.Library.Common;
using XExten.Advance.HttpFramework.MultiFactory;

namespace CandySugar.Controls
{
    public class HViewModel : ViewModelBase
    {
        readonly IService Service;
        public IPlayService PlayService;
        public HViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
            PlayService = this.Container.Resolve<IPlayService>();
            this.P1 = this.P2 = this.P3 = this.P4 = 1;
            LikeInit();
        }

        public override void OnLoad()
        {
            MessagingCenter.Subscribe<string, bool>("Music", "Ref", (sender, args) =>
            {
                LikeInit();
            });
        }

        #region Property
        public string Key { get; set; }
        public int P1 { get; set; }
        public int P2 { get; set; }
        public int P3 { get; set; }
        public int P4 { get; set; }
        public int? T1 { get; set; }
        public int? T2 { get; set; }
        public int? T3 { get; set; }
        public int? T4 { get; set; }
        public HRootEntity Entity { get; set; }
        #endregion

        #region Property
        ObservableCollection<MusicSongElementResult> _SongResult;
        public ObservableCollection<MusicSongElementResult> SongResult
        {
            get => _SongResult;
            set => SetProperty(ref _SongResult, value);
        }
        ObservableCollection<MusicSheetElementResult> _PlayListResult;
        public ObservableCollection<MusicSheetElementResult> PlayListResult
        {
            get => _PlayListResult;
            set => SetProperty(ref _PlayListResult, value);
        }
        ObservableCollection<HRootEntity> _Result;
        public ObservableCollection<HRootEntity> Result
        {
            get => _Result;
            set => SetProperty(ref _Result, value);
        }
        #endregion

        #region Method
        public async void QueryInit(bool More)
        {
            try
            {
                SetState(More);
                await Task.Delay(DataBus.Delay);
                var QQ = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        PlatformType = PlatformEnum.QQMusic,
                        MusicType = MusicEnum.Song,
                        Search = new MusicSearch
                        {
                            Page = this.P1,
                            KeyWord = Key
                        }
                    };
                }).RunsAsync();
                var NetEasy = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        PlatformType = PlatformEnum.NeteaseMusic,
                        MusicType = MusicEnum.Song,
                        Search = new MusicSearch
                        {
                            Page = this.P2,
                            KeyWord = Key
                        }
                    };
                }).RunsAsync();
                var KuWo = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        PlatformType = PlatformEnum.KuWoMusic,
                        MusicType = MusicEnum.Song,
                        Search = new MusicSearch
                        {
                            Page = this.P3,
                            KeyWord = Key
                        }
                    };
                }).RunsAsync();
                var KuGou = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        PlatformType = PlatformEnum.KuGouMusic,
                        MusicType = MusicEnum.Song,
                        Search = new MusicSearch
                        {
                            Page = this.P4,
                            KeyWord = Key
                        }
                    };
                }).RunsAsync();
                T1 = QQ.SongResult.Total;
                T2 = NetEasy.SongResult.Total;
                T3 = KuGou.SongResult.Total;
                T4 = KuWo.SongResult.Total;
                if (More)
                {
                    QQ.SongResult.ElementResults.ForEach(SongResult.Add);
                    NetEasy.SongResult.ElementResults.ForEach(SongResult.Add);
                    KuWo.SongResult.ElementResults.ForEach(SongResult.Add);
                    KuGou.SongResult.ElementResults.ForEach(SongResult.Add);
                }
                else
                {
                    SongResult = new ObservableCollection<MusicSongElementResult>(QQ.SongResult.ElementResults);
                    NetEasy.SongResult.ElementResults.ForEach(SongResult.Add);
                    KuWo.SongResult.ElementResults.ForEach(SongResult.Add);
                    KuGou.SongResult.ElementResults.ForEach(SongResult.Add);
                }
                SetState();
            }
            catch (Exception ex)
            {
                SetState();
                await Service.AddLog("HQueryInit异常", ex);
                "HQueryInit异常".OpenToast();
            }
        }
        async void QueryListInit(bool More)
        {
            try
            {
                SetState(More);
                await Task.Delay(DataBus.Delay);
                var QQ = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        PlatformType = PlatformEnum.QQMusic,
                        MusicType = MusicEnum.Sheet,
                        Search = new MusicSearch
                        {
                            Page = this.P1,
                            KeyWord = Key
                        }
                    };
                }).RunsAsync();
                var NetEasy = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        PlatformType = PlatformEnum.NeteaseMusic,
                        MusicType = MusicEnum.Sheet,
                        Search = new MusicSearch
                        {
                            Page = this.P2,
                            KeyWord = Key
                        }
                    };
                }).RunsAsync();
                var KuWo = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        PlatformType = PlatformEnum.KuWoMusic,
                        MusicType = MusicEnum.Sheet,
                        Search = new MusicSearch
                        {
                            Page = this.P3,
                            KeyWord = Key
                        }
                    };
                }).RunsAsync();
                var KuGou = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        PlatformType = PlatformEnum.KuGouMusic,
                        MusicType = MusicEnum.Sheet,
                        Search = new MusicSearch
                        {
                            Page = this.P4,
                            KeyWord = Key
                        }
                    };
                }).RunsAsync();
                T1 = QQ.SheetResult.Total;
                T2 = NetEasy.SheetResult.Total;
                T3 = KuGou.SheetResult.Total;
                T4 = KuWo.SheetResult.Total;
                if (More)
                {
                    QQ.SheetResult.ElementResults.ForEach(PlayListResult.Add);
                    NetEasy.SheetResult.ElementResults.ForEach(PlayListResult.Add);
                    KuWo.SheetResult.ElementResults.ForEach(PlayListResult.Add);
                    KuGou.SheetResult.ElementResults.ForEach(PlayListResult.Add);
                }
                else
                {
                    PlayListResult = new ObservableCollection<MusicSheetElementResult>(QQ.SheetResult.ElementResults);
                    NetEasy.SheetResult.ElementResults.ForEach(PlayListResult.Add);
                    KuWo.SheetResult.ElementResults.ForEach(PlayListResult.Add);
                    KuGou.SheetResult.ElementResults.ForEach(PlayListResult.Add);
                }
                SetState();
            }
            catch (Exception ex)
            {
                SetState();
                await Service.AddLog("HQueryListInit异常", ex);
                "HQueryListInit异常".OpenToast();
            }
        }
        async void LikeInit()
        {
            var res = await Service.HQuery();
            if (res != null)
                Result = new ObservableCollection<HRootEntity>(res);
        }
        async void Add(MusicSongElementResult input)
        {
            var res = await Service.HAdd(new HRootEntity
            {
                ArtistName = string.Join(",", input.SongArtistName),
                Name = input.SongName,
                AlbumId = input.SongAlbumId,
                Platfrom = Platform((int)input.MusicPlatformType),
                SongId = input.SongId,
                AlbumName = input.SongAlbumName
            });
            if (res) "加入我的歌单成功".OpenToast();
            LikeInit();
        }
        async void Delete(dynamic input)
        {
            var res = await Service.HRemove(input);
            if (res)
            {
                "从我的歌单移除成功".OpenToast();
            }
            LikeInit();
        }
        string Platform(int Platform)
        {
            if (Platform == 1) return "QQ";
            else if (Platform == 2) return "网易";
            else if (Platform == 3) return "酷狗";
            else return "酷我";
        }
        PlatformEnum Platform(string input)
        {
            if (input == "QQ") return PlatformEnum.QQMusic;
            else if (input == "网易") return PlatformEnum.NeteaseMusic;
            else if (input == "酷狗") return PlatformEnum.KuGouMusic;
            else return PlatformEnum.KuWoMusic;
        }
        async void PlayInit(HRootEntity input)
        {
            try
            {
                var result = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        MusicType = MusicEnum.Route,
                        PlatformType = Platform(input.Platfrom),
                        Play = Platform(input.Platfrom) == PlatformEnum.KuGouMusic ? new MusicPlaySearch
                        {
                            Dynamic = input.SongId,
                            KuGouAlbumId = input.AlbumId,
                        } : new MusicPlaySearch
                        {
                            Dynamic = input.SongId,
                        }
                    };
                }).RunsAsync();

                if (result.PlayResult.CanPlay == false)
                {
                    "当前歌曲已下架".OpenToast();
                    return;
                }
                var Model = (await Service.HQuery()).Where(t => t.Id == input.Id).FirstOrDefault();
                if (!Model.Route.IsNullOrEmpty())
                {
                    Entity = Model;
                    LikeInit();
                    await PlayService.PlayAsync(Entity);
                    return;
                }
                var SongFile = $"{input.Name}({input.AlbumName})-{string.Join(",", input.ArtistName)}_{input.Platfrom}.mp3";
                var Directory = SyncStatic.CreateDir(Path.Combine(ICrossExtension.Instance.AndriodPath, "CandyDown", "Music"));
                var Files = SyncStatic.CreateFile(Path.Combine(Directory, SongFile));

                var filebytes = IHttpMultiClient.HttpMulti.AddNode(opt => opt.NodePath = result.PlayResult.SongURL).Build().RunBytesFirst();
                var CacheAddress = SyncStatic.WriteFile(filebytes, Files);
                Entity = await Service.HAlter(input.Id, CacheAddress);
                LikeInit();
                await PlayService.PlayAsync(Entity);
            }
            catch (Exception ex)
            {
                SetState();
                await Service.AddLog("HPlayInit异常", ex);
                "HPlayInit异常".OpenToast();
            }
        }
        #endregion

        #region Command
        public DelegateCommand MoreCommand => new(() =>
        {

            P1 += 1;
            P2 += 1;
            P3 += 1;
            P4 += 1;
            if (P1 > T1) P1 = T1.Value; if (P2 > T2) P2 = T2.Value;
            if (P3 > T3) P3 = T3.Value; if (P4 > T4) P4 = T4.Value;
            QueryInit(true);
        });
        public DelegateCommand MoresCommand => new(() =>
        {

            P1 += 1;
            P2 += 1;
            P3 += 1;
            P4 += 1;
            if (P1 > T1) P1 = T1.Value; if (P2 > T2) P2 = T2.Value;
            if (P3 > T3) P3 = T3.Value; if (P4 > T4) P4 = T4.Value;
            QueryListInit(true);
        });
        public DelegateCommand SingleCommand => new(() =>
        {
            this.P1 = this.P2 = this.P3 = this.P4 = 1;
            if (!Key.IsNullOrEmpty())
                QueryInit(false);
        });
        public DelegateCommand ListCommand => new(() =>
        {
            this.P1 = this.P2 = this.P3 = this.P4 = 1;
            if (!Key.IsNullOrEmpty())
                QueryListInit(false);
        });
        public DelegateCommand<MusicSongElementResult> AlbumCommand => new(input =>
        {
            Nav.NavigateAsync(new Uri("H1", UriKind.Relative), new NavigationParameters { { "Data", input } });
        });
        public DelegateCommand<MusicSheetElementResult> PlayListCommand => new(input =>
        {
            Nav.NavigateAsync(new Uri("H2", UriKind.Relative), new NavigationParameters { { "Data", input } });
        });
        public DelegateCommand<MusicSongElementResult> LikeCommand => new(Add);
        public DelegateCommand<dynamic> DelCommand => new(Delete);
        public DelegateCommand<HRootEntity> PlayCommand => new(input =>
        {
            Task.Run(() => PlayInit(input));
        });
        public DelegateCommand PlayOrPauseCommand => new(()=>
        {
            PlayService.PlayAsync(Entity);
        });
        #endregion
    }
}

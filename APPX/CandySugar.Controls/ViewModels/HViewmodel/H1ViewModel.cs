using Sdk.Component.Music.sdk.ViewModel;
using Sdk.Component.Music.sdk.ViewModel.Response;
using Sdk.Component.Music.sdk;
using Sdk.Component.Music.sdk.ViewModel.Enums;
using Sdk.Component.Music.sdk.ViewModel.Request;
using System.Collections.ObjectModel;

namespace CandySugar.Controls
{
    public class H1ViewModel : ViewModelBase
    {
        readonly IService Service;
        public H1ViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            Album = parameters.GetValue<MusicSongElementResult>("Data");
            Task.Run(AlbumInit);
        }

        #region Property
        MusicSongElementResult _Album;
        public MusicSongElementResult Album
        {
            get => _Album;
            set => SetProperty(ref _Album, value);
        }

        ObservableCollection<MusicSongElementResult> _Result;
        public ObservableCollection<MusicSongElementResult> Result
        {
            get => _Result;
            set => SetProperty(ref _Result, value);
        }
        #endregion

        #region Command
        public DelegateCommand BackCommand => new(() =>
        {
            Nav.GoBackAsync();
            MessagingCenter.Send("Music", "Ref", true);
        });
        public DelegateCommand<MusicSongElementResult> LikeCommand => new(Add);
        #endregion

        #region Method
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
        }
        string Platform(int Platform)
        {
            if (Platform == 1) return "QQ";
            else if (Platform == 2) return "网易";
            else if (Platform == 3) return "酷狗";
            else return "酷我";
        }
        async void AlbumInit()
        {
            try
            {
                this.Activity = true;
                await Task.Delay(DataBus.Delay);
                var result = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        PlatformType = Album.MusicPlatformType,
                        MusicType = MusicEnum.AlbumDetail,
                        AlbumDetail = new MusicAlbumDetail
                        {
                            AlbumId = Album.SongAlbumId
                        }
                    };
                }).RunsAsync();
                Result = new ObservableCollection<MusicSongElementResult>(result.AlbumResult.ElementResults);
                SetState();
            }
            catch (Exception ex)
            {
                SetState();
                await Service.AddLog("HAlbumInit异常", ex);
                "HAlbumInit异常".OpenToast();
            }
        }
        #endregion
    }
}

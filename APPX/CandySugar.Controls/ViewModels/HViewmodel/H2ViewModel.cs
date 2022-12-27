using Sdk.Component.Music.sdk.ViewModel;
using Sdk.Component.Music.sdk.ViewModel.Response;
using Sdk.Component.Music.sdk;
using Sdk.Component.Music.sdk.ViewModel.Enums;
using Sdk.Component.Music.sdk.ViewModel.Request;
using System.Collections.ObjectModel;

namespace CandySugar.Controls
{
    public class H2ViewModel : ViewModelBase
    {
        readonly IService Service;
        public H2ViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            Sheet = parameters.GetValue<MusicSheetElementResult>("Data");
            Task.Run(SheetInit);
        }

        #region Command
        public DelegateCommand BackCommand => new(() =>
        {
            Nav.GoBackAsync();
            MessagingCenter.Send("Music", "Ref", true);
        });
        public DelegateCommand<MusicSongElementResult> LikeCommand => new(Add);
        #endregion

        #region Property
        MusicSheetElementResult _Sheet;
        public MusicSheetElementResult Sheet
        {
            get => _Sheet;
            set => SetProperty(ref _Sheet, value);
        }
        ObservableCollection<MusicSongElementResult> _Result;
        public ObservableCollection<MusicSongElementResult> Result
        {
            get => _Result;
            set => SetProperty(ref _Result, value);
        }
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
        async void SheetInit()
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
                        PlatformType = Sheet.MusicPlatformType,
                        MusicType = MusicEnum.SheetDetail,
                        SheetDetail = new MusicSheetDetail
                        {
                            Id = Sheet.SongSheetId.ToString()
                        }
                    };
                }).RunsAsync();
                Result = new ObservableCollection<MusicSongElementResult>(result.SheetDetailResult.ElementResults);
                SetState();
            }
            catch (Exception ex)
            {
                SetState();
                await Service.AddLog("HSheetInit异常", ex);
                "HSheetInit异常".OpenToast();
            }
        }
        #endregion
    }
}

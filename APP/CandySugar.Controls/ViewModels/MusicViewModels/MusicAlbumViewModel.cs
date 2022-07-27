using Sdk.Component.Music.sdk.ViewModel.Enums;
using Sdk.Component.Music.sdk;
using Sdk.Component.Music.sdk.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.HttpFramework.MultiFactory;
using Sdk.Component.Music.sdk.ViewModel.Request;
using Sdk.Component.Music.sdk.ViewModel;

namespace CandySugar.Controls.ViewModels.MusicViewModels
{
    public class MusicAlbumViewModel : BaseViewModel
    {
        ICandyService CandyService;
        PlatformEnum PlatformType;
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            CandyService = CandyContainer.Instance.Resolve<ICandyService>();
            PlatformType = (PlatformEnum)query["Key"].ToString().AsInt();
            AlbumResult = new(query["Data"] as List<MusicSongElementResult>);
        }

        #region 属性
        ObservableCollection<MusicSongElementResult> _AlbumResult;
        public ObservableCollection<MusicSongElementResult> AlbumResult
        {
            get => _AlbumResult;
            set => SetProperty(ref _AlbumResult, value);
        }
        #endregion

        #region 命令
        public DelegateCommand<MusicSongElementResult> AddPlayAction => new(input => Task.Run(()=> InitDown(input)));
        #endregion

        #region 方法
        async void InitDown(MusicSongElementResult input)
        {
            if (CandyService.GetMusic().FirstOrDefault(t => t.SongId == input.SongId) == null)
            {
                var result = await MusicFactory.Music(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        PlatformType = PlatformType,
                        ImplType = StaticResource.ImplType(),
                        MusicType = MusicEnum.Route,
                        Play = PlatformType == PlatformEnum.KuGouMusic ? new MusicPlaySearch
                        {
                            Dynamic = input.SongId,
                            KuGouAlbumId = input.SongAlbumId,
                        } : new MusicPlaySearch
                        {
                            Dynamic = input.SongId,
                        }
                    };
                }).RunsAsync();

                if (result.PlayResult.CanPlay == false)
                {
                    StaticResource.PopToast("当前歌曲已下架");
                    return;
                }

                var SongFile = $"{input.SongName}({input.SongAlbumName})-{string.Join(",", input.SongArtistName)}_{PlatformType}.mp3";
                var Directory = SyncStatic.CreateDir(Path.Combine(ICrossExtension.Instance.AndriodPath, "CandyDown", "Music"));
                var Files = SyncStatic.CreateFile(Path.Combine(Directory, SongFile));

                if (this.PlatformType == PlatformEnum.BiliBiliMusic)
                {
                    var CacheAddress = SyncStatic.WriteFile(result.PlayResult.BilibiliFileBytes, Files);
                }
                else
                {
                    var filebytes = IHttpMultiClient.HttpMulti.AddNode(opt => opt.NodePath = result.PlayResult.SongURL).Build().RunBytesFirst();
                    var CacheAddress = SyncStatic.WriteFile(filebytes, Files);
                }

                CandyService.AddMusic(new CandyMusic
                {
                    AlbumId = input.SongAlbumId,
                    AlbumName = input.SongAlbumName,
                    SongArtist = String.Join(",", input.SongArtistName),
                    IsComplete = true,
                    LocalRoute = Files,
                    NetRoute = result.PlayResult.SongURL,
                    SongId = input.SongId,
                    Platform = (int)PlatformType,
                    SongName = input.SongName
                });
            }
        }
        #endregion
    }
}

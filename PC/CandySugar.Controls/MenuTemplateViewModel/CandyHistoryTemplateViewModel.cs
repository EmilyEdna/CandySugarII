using CandySugar.Library;
using CandySugar.Logic.Entity.CandyEntity;
using CandySugar.Logic.IService;
using CandySugar.Resource.Properties;
using DnsClient.Protocol;
using HandyControl.Controls;
using HandyControl.Data;
using NAudio;
using Sdk.Component.Bgm.sdk;
using Sdk.Component.Bgm.sdk.ViewModel.Enums;
using Sdk.Component.Bgm.sdk.ViewModel.Response;
using Sdk.Component.Image.sdk;
using Sdk.Component.Image.sdk.ViewModel;
using Sdk.Component.Image.sdk.ViewModel.Enums;
using Sdk.Component.Image.sdk.ViewModel.Request;
using Stylet;
using StyletIoC;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CandySugar.Controls.MenuTemplateViewModel
{
    public class CandyHistoryTemplateViewModel : PropertyChangedBase
    {
        public IContainer Container;
        public IWindowManager WindowManager;

        public CandyHistoryTemplateViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            ZF = true;
            XS = LXS = DM = HDM = MH = BZ = JY = DY = false;
            CandyNovel = Container.Get<ICandyNovel>();
            CandyLovel = Container.Get<ICandyLovel>();
            CandyAnime = Container.Get<ICandyAnime>();
            CandyManga = Container.Get<ICandyManga>();
            CandyImage = Container.Get<ICandyImage>();
            CandyHnime = Container.Get<ICandyHnime>();
            CandyAxgle = Container.Get<ICandyAxgle>();
            CandyMovie = Container.Get<ICandyMovie>();
        }

        #region 字段
        private ICandyNovel CandyNovel;
        private ICandyLovel CandyLovel;
        private ICandyAnime CandyAnime;
        private ICandyManga CandyManga;
        private ICandyImage CandyImage;
        private ICandyHnime CandyHnime;
        private ICandyAxgle CandyAxgle;
        private ICandyMovie CandyMovie;
        #endregion

        #region 整型
        private int _Total;
        public int Total
        {
            get => _Total;
            set => SetAndNotify(ref _Total, value);
        }
        #endregion

        #region 布尔
        private bool _ZF;
        public bool ZF
        {
            get => _ZF;
            set => SetAndNotify(ref _ZF, value);
        }

        private bool _XS;
        public bool XS
        {
            get => _XS;
            set => SetAndNotify(ref _XS, value);
        }

        private bool _LXS;
        public bool LXS
        {
            get => _LXS;
            set => SetAndNotify(ref _LXS, value);
        }

        private bool _DM;
        public bool DM
        {
            get => _DM;
            set => SetAndNotify(ref _DM, value);
        }

        private bool _HDM;
        public bool HDM
        {
            get => _HDM;
            set => SetAndNotify(ref _HDM, value);
        }

        private bool _MH;
        public bool MH
        {
            get => _MH;
            set => SetAndNotify(ref _MH, value);
        }

        private bool _BZ;
        public bool BZ
        {
            get => _BZ;
            set => SetAndNotify(ref _BZ, value);
        }

        private bool _JY;
        public bool JY
        {
            get => _JY;
            set => SetAndNotify(ref _JY, value);
        }

        private bool _DY;
        public bool DY
        {
            get => _DY;
            set => SetAndNotify(ref _DY, value);
        }
        #endregion

        #region 属性
        private ObservableCollection<BgmInitResult> _BgmResult;
        public ObservableCollection<BgmInitResult> BgmResult
        {
            get => _BgmResult;
            set => SetAndNotify(ref _BgmResult, value);
        }
        private ObservableCollection<CandyNovel> _CandyNovelResult;
        public ObservableCollection<CandyNovel> CandyNovelResult
        {
            get => _CandyNovelResult;
            set => SetAndNotify(ref _CandyNovelResult, value);
        }

        private ObservableCollection<CandyLovel> _CandyLovelResult;
        public ObservableCollection<CandyLovel> CandyLovelResult
        {
            get => _CandyLovelResult;
            set => SetAndNotify(ref _CandyLovelResult, value);
        }

        private ObservableCollection<CandyAnimeRoot> _CandyAnimeRootResult;
        public ObservableCollection<CandyAnimeRoot> CandyAnimeRootResult
        {
            get => _CandyAnimeRootResult;
            set => SetAndNotify(ref _CandyAnimeRootResult, value);
        }

        private ObservableCollection<CandyManga> _CandyMangaResult;
        public ObservableCollection<CandyManga> CandyMangaResult
        {
            get => _CandyMangaResult;
            set => SetAndNotify(ref _CandyMangaResult, value);
        }

        private ObservableCollection<CandyImage> _CandyImageResult;
        public ObservableCollection<CandyImage> CandyImageResult
        {
            get => _CandyImageResult;
            set => SetAndNotify(ref _CandyImageResult, value);
        }

        private ObservableCollection<CandyHnime> _CandyHnimeResult;
        public ObservableCollection<CandyHnime> CandyHnimeResult
        {
            get => _CandyHnimeResult;
            set => SetAndNotify(ref _CandyHnimeResult, value);
        }

        private ObservableCollection<CandyMovie> _CandyMovieResult;
        public ObservableCollection<CandyMovie> CandyMovieResult
        {
            get => _CandyMovieResult;
            set => SetAndNotify(ref _CandyMovieResult, value);
        }

        private ObservableCollection<CandyAxgle> _CandyAxgleResult;
        public ObservableCollection<CandyAxgle> CandyAxgleResult
        {
            get => _CandyAxgleResult;
            set => SetAndNotify(ref _CandyAxgleResult, value);
        }
        #endregion

        #region 命令
        public void ChangeAction(string input)
        {
            switch (input)
            {
                case "ZF":
                    ZF = true;
                    XS = LXS = DM = HDM = MH = BZ = JY = DY = false;
                    InitBGM();
                    break;
                case "XS":
                    XS = true;
                    ZF = LXS = DM = HDM = MH = BZ = JY = DY = false;
                    InitNovel();
                    break;
                case "LXS":
                    LXS = true;
                    ZF = XS = DM = HDM = MH = BZ = JY = DY = false;
                    InitLovel();
                    break;
                case "DM":
                    DM = true;
                    ZF = XS = LXS = HDM = MH = BZ = JY = DY = false;
                    InitAnime();
                    break;
                case "HDM":
                    HDM = true;
                    ZF = XS = LXS = DM = MH = BZ = JY = DY = false;
                    InitHnime();
                    break;
                case "MH":
                    MH = true;
                    ZF = XS = LXS = DM = HDM = BZ = JY = DY = false;
                    InitManga();
                    break;
                case "BZ":
                    BZ = true;
                    ZF = MH = XS = LXS = DM = HDM = JY = DY = false;
                    InitImage();
                    break;
                case "JY":
                    JY = true;
                    ZF = XS = LXS = DM = HDM = MH = BZ = DY = false;
                    InitAxgle();
                    break;
                default:
                    DY = true;
                    ZF = XS = LXS = DM = HDM = MH = BZ = JY = false;
                    InitMovie();
                    break;
            }
        }
        public void RemoveAction(dynamic input)
        {
            if (input is CandyNovel Novel)
                DelNovel(Novel);
            if (input is CandyLovel Lovel)
                DelLovel(Lovel);
            if (input is CandyAnimeRoot Anime)
                DelAnime(Anime);
            if (input is CandyManga Manga)
                DelManga(Manga);
            if (input is CandyImage Image)
                DelImage(Image);
            if (input is CandyHnime Hnime)
                DelHnime(Hnime);
            if (input is CandyAxgle Axgle)
                DelAxgle(Axgle);
            if (input is CandyMovie Movie)
                DelMovie(Movie);
        }

        public void ImagePageAction(FunctionEventArgs<int> input)
        {
            InitImage(input.Info);
        }
        public void DownloadAction(string input)
        {
            Growl.Info("已添加到下载线程请稍后！");
            Task.Run(() =>
            {
                var File = Regex.Split(input.Split("/").LastOrDefault(), "\\d+").LastOrDefault();
                var ImageInitData = ImageFactory.Image(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Default.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        ImageType = ImageEnum.Download,
                        Download = new ImageDownload
                        {
                            Route = input
                        }
                    };
                }).RunsAsync().GetAwaiter().GetResult();
                StaticResource.Download(ImageInitData.DownResult.Bytes, "Image", File.Split(".").FirstOrDefault(), File.Split(".").LastOrDefault());
            });
        }
        #endregion

        #region 方法
        private async void InitBGM()
        {
            var result = await BgmFactory.Bgm(opt =>
             {
                 opt.RequestParam = new Sdk.Component.Bgm.sdk.ViewModel.Input
                 {
                     CacheSpan = CandySoft.Default.Cache,
                     Proxy = StaticResource.Proxy(),
                     ImplType = StaticResource.ImplType(),
                     BgmType = BgmEnum.Calendar,
                 };
             }).RunsAsync();
            BgmResult = new ObservableCollection<BgmInitResult>(result.InitResults);
        }
        private async void InitNovel()
        {
            CandyNovelResult = new ObservableCollection<CandyNovel>(await CandyNovel.Get());
        }
        private async void InitLovel()
        {
            CandyLovelResult = new ObservableCollection<CandyLovel>(await CandyLovel.Get());
        }
        private async void InitAnime()
        {
            CandyAnimeRootResult = new ObservableCollection<CandyAnimeRoot>(await CandyAnime.Get());
        }
        private async void InitHnime()
        {
            CandyHnimeResult = new ObservableCollection<CandyHnime>(await CandyHnime.Get());
        }
        private async void InitManga()
        {
            CandyMangaResult = new ObservableCollection<CandyManga>(await CandyManga.Get());
        }
        private async void InitImage(int page = 1)
        {
            var data = await CandyImage.Get(page);
            Total = data.Item2;
            CandyImageResult = new ObservableCollection<CandyImage>(data.Item1);
        }
        private async void InitAxgle()
        {
            CandyAxgleResult = new ObservableCollection<CandyAxgle>(await CandyAxgle.Get());
        }
        private async void InitMovie()
        {
            CandyMovieResult = new ObservableCollection<CandyMovie>(await CandyMovie.Get());
        }
        #endregion

        #region 删除
        private async void DelNovel(CandyNovel input)
        {
            await CandyNovel.Remove(input);
            InitNovel();
        }
        private async void DelLovel(CandyLovel input)
        {
            await CandyLovel.Remove(input);
            InitLovel();
        }
        private async void DelAnime(CandyAnimeRoot input)
        {
            await CandyAnime.Remove(input);
            InitAnime();
        }
        private async void DelManga(CandyManga input)
        {
            await CandyManga.Remove(input);
            InitManga();
        }
        private async void DelImage(CandyImage input)
        {
            await CandyImage.Remove(input);
            InitImage();
        }
        private async void DelHnime(CandyHnime input)
        {
            await CandyHnime.Remove(input);
            InitHnime();
        }
        private async void DelMovie(CandyMovie movie)
        {
            await CandyMovie.Remove(movie);
            InitMovie();
        }

        private async void DelAxgle(CandyAxgle axgle)
        {
            await CandyAxgle.Remove(axgle);
            InitAxgle();
        }
        #endregion
    }
}

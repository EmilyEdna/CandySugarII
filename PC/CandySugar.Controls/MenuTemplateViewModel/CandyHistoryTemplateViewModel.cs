using CandySugar.Library;
using CandySugar.Logic.Entity.CandyEntity;
using CandySugar.Logic.IService;
using CandySugar.Resource.Properties;
using HandyControl.Controls;
using HandyControl.Data;
using Polly.Caching;
using Sdk.Component.Image.sdk;
using Sdk.Component.Image.sdk.ViewModel;
using Sdk.Component.Image.sdk.ViewModel.Enums;
using Sdk.Component.Image.sdk.ViewModel.Request;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
            XS = LXS = DM = HDM = MH = BZ = JY = false;
            CandyNovel = Container.Get<ICandyNovel>();
            CandyLovel = Container.Get<ICandyLovel>();
            CandyAnime = Container.Get<ICandyAnime>();
            CandyManga = Container.Get<ICandyManga>();
            CandyImage = Container.Get<ICandyImage>();
            CandyHnime= Container.Get<ICandyHnime>();
        }

        #region Field
        private ICandyNovel CandyNovel;
        private ICandyLovel CandyLovel;
        private ICandyAnime CandyAnime;
        private ICandyManga CandyManga;
        private ICandyImage CandyImage;
        private ICandyHnime CandyHnime;
        #endregion

        #region CommomProperty_Int
        private int _Total;
        public int Total
        {
            get => _Total;
            set => SetAndNotify(ref _Total, value);
        }
        #endregion

        #region CommomProperty_Bool
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
        #endregion

        #region Property
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
        #endregion

        #region Action
        public void ChangeAction(string input)
        {
            switch (input)
            {
                case "XS":
                    XS = true;
                    LXS = DM = HDM = MH = BZ = JY = false;
                    InitNovel();
                    break;
                case "LXS":
                    LXS = true;
                    XS = DM = HDM = MH = BZ = JY = false;
                    InitLovel();
                    break;
                case "DM":
                    DM = true;
                    XS = LXS = HDM = MH = BZ = JY = false;
                    InitAnime();
                    break;
                case "HDM":
                    HDM = true;
                    XS = LXS = DM = MH = BZ = JY = false;
                    InitHnime();
                    break;
                case "MH":
                    MH = true;
                    XS = LXS = DM = HDM = BZ = JY = false;
                    InitManga();
                    break;
                case "BZ":
                    BZ = true;
                    MH = XS = LXS = DM = HDM = JY = false;
                    InitImage();
                    break;
                default:
                    JY = true;
                    XS = LXS = DM = HDM = MH = BZ = false;
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
            if(input is CandyHnime Hnime)
                DelHnime(Hnime);
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
                var File = Regex.Split(input.Split("/").LastOrDefault(),"\\d+").LastOrDefault();
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

        #region Method
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
        #endregion

        #region Remove
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
        #endregion
    }
}

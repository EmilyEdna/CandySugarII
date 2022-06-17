using CandySugar.Logic.Entity.CandyEntity;
using CandySugar.Logic.IService;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
            XS = LXS = DM = HDM = MH = JY = false;
            CandyNovel = Container.Get<ICandyNovel>();
            CandyLovel = Container.Get<ICandyLovel>();
            CandyAnime = Container.Get<ICandyAnime>();
            CandyManga = Container.Get<ICandyManga>();
        }

        #region Field
        private ICandyNovel CandyNovel;
        private ICandyLovel CandyLovel;
        private ICandyAnime CandyAnime;
        private ICandyManga CandyManga;
        #endregion

        #region CommomProperty_Int
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
        #endregion

        #region Action
        public void ChangeAction(string input)
        {
            switch (input)
            {
                case "XS":
                    XS = true;
                    LXS = DM = HDM = MH = JY = false;
                    InitNovel();
                    break;
                case "LXS":
                    LXS = true;
                    XS = DM = HDM = MH = JY = false;
                    InitLovel();
                    break;
                case "DM":
                    DM = true;
                    XS = LXS = HDM = MH = JY = false;
                    InitAnime();
                    break;
                case "HDM":
                    HDM = true;
                    XS = LXS = DM = MH = JY = false;
                    break;
                case "MH":
                    MH = true;
                    XS = LXS = DM = HDM = JY = false;
                    InitManga();
                    break;
                default:
                    JY = true;
                    XS = LXS = DM = HDM = MH = false;
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
        private async void InitManga()
        {
            CandyMangaResult = new ObservableCollection<CandyManga>(await CandyManga.Get());
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
        #endregion
    }
}

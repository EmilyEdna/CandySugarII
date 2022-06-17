using CandySugar.Controls.Template;
using CandySugar.Controls.TemplateViewModel;
using CandySugar.Entry.CandyViewModels;
using CandySugar.Library.Template;
using CandySugar.Resource.Properties;
using Stylet;
using StyletIoC;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using CandySugar.Library;
using CandySugar.Controls.ContentView;
using CandySugar.Controls.ContentViewModel;
using CandySugar.Logic.Entity.CandyEntity;
using Sdk.Component.Anime.sdk.ViewModel.Response;
using CandySugar.Logic.IService;
using Sdk.Component.Manga.sdk.ViewModel.Response;

namespace CandySugar.Entry.ViewModels
{
    public class RootViewModel : Conductor<IScreen>
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public RootViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.Container = Container;

            this.WindowManager = WindowManager;
            SilderView = new CandySilderTemplateView
            {
                DataContext = Container.Get<CandySilderTemplateViewModel>()
            };
            HeadViewModel = Container.Get<CandyHeadTemplateViewModel>();
        }

        public CandySilderTemplateView SilderView { get; set; }
        public CandyHeadTemplateViewModel HeadViewModel { get; set; }

        private CandyControl _Ctrl;
        public CandyControl Ctrl
        {
            get => _Ctrl;
            set => SetAndNotify(ref _Ctrl, value);
        }

        #region Action
        public void SettingAction()
        {
            WindowManager.ShowWindow(Container.Get<OptionViewModel>());
        }

        public void SearchAction(Dictionary<object, object> param)
        {

            if (param.Keys.FirstOrDefault() is string type)
            {
                switch (type)
                {
                    case "XS":
                        Ctrl = StaticResource.CreateControl<NovelView>(Container.Get<NovelViewModel>(), param.Values.FirstOrDefault().ToString());
                        break;
                    case "LXS":
                        Ctrl = StaticResource.CreateControl<LovelView>(Container.Get<LovelViewModel>(), param.Values.FirstOrDefault().ToString());
                        break;
                    case "DM":
                        Ctrl = StaticResource.CreateControl<AnimeView>(Container.Get<AnimeViewModel>(), param.Values.FirstOrDefault().ToString());
                        break;
                    case "MH":
                        Ctrl = StaticResource.CreateControl<MangaView>(Container.Get<MangaViewModel>(), param.Values.FirstOrDefault().ToString());
                        break;
                    case "BZ":
                        Ctrl = StaticResource.CreateControl<ImageView>(Container.Get<ImageViewModel>(), param.Values.FirstOrDefault().ToString());
                        break;
                    case "YY":
                        Ctrl = StaticResource.CreateControl<MusicView>(Container.Get<MusicViewModel>(), param.Values.FirstOrDefault().ToString());
                        break;
                    default:
                        break;
                }
            }
        }

        public void HistoryAction()
        {
            Ctrl = StaticResource.CreateControl<CandyHistoryTemplateView>(Container.Get<CandyHistoryTemplateViewModel>(), null);
        }

        public async void ContinueAction(dynamic input)
        {
            if (input is CandyNovel Novel)
                Ctrl = StaticResource.CreateControl<NovelView>(Container.Get<NovelViewModel>(), Novel.Route, "ViewAction");
            if (input is CandyLovel Lovel)
                Ctrl = StaticResource.CreateControl<LovelView>(Container.Get<LovelViewModel>(), Lovel.Route, "ContentAction");
            if (input is CandyAnimeRoot Anime)
                Ctrl = StaticResource.CreateControl<AnimeView>(Container.Get<AnimeViewModel>(), new AnimeDetailResult
                {
                    CollectName = Anime.CollectName,
                    Cover = Anime.Cover,
                    Name = Anime.AnimeName,
                    IsDownURL = false,
                    WatchRoute = Anime.Route
                }, "WatchAction");
            if (input is CandyAnimeElement AnimeEle)
            {
               var Root =(await Container.Get<ICandyAnime>().Get()).FirstOrDefault(t => t.CandyId == AnimeEle.RootId);
                Ctrl = StaticResource.CreateControl<AnimeView>(Container.Get<AnimeViewModel>(), new AnimeDetailResult
                {
                    CollectName = AnimeEle.Name,
                    Cover = Root.Cover,
                    Name = Root.AnimeName,
                    IsDownURL = false,
                    WatchRoute = AnimeEle.Route
                }, "WatchAction");
            }
            if(input is CandyManga Manga)
                Ctrl = StaticResource.CreateControl<MangaView>(Container.Get<MangaViewModel>(), new MangaChapterDetailResult
                {
                    Name = Manga.Name,
                    Route= Manga.Route,
                    TagKey= Manga.Key,
                    Title=Manga.CollectName
                }, "WatchAction");
        }
        #endregion
        public void ScreenActivity(CandyControl input)
        {
            Ctrl = input;
        }

        protected override void OnViewLoaded()
        {
            HistoryAction();
        }
    }
}

using CandySugar.Controls.ContentView;
using CandySugar.Controls.ContentViewModel;
using CandySugar.Controls.MenuTemplate;
using CandySugar.Controls.MenuTemplateViewModel;
using CandySugar.Controls.Template;
using CandySugar.Controls.TemplateViewModel;
using CandySugar.Entry.CandyViewModels;
using CandySugar.Library;
using CandySugar.Library.Template;
using CandySugar.Logic.Entity.CandyEntity;
using CandySugar.Logic.IService;
using Sdk.Component.Anime.sdk.ViewModel.Response;
using Sdk.Component.Hnime.sdk.ViewModel.Request;
using Sdk.Component.Hnime.sdk.ViewModel.Response;
using Sdk.Component.Manga.sdk.ViewModel.Response;
using Stylet;
using StyletIoC;
using System.Collections.Generic;
using System.Linq;
using XExten.Advance.LinqFramework;

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
        /// <summary>
        /// 全局查询
        /// </summary>
        /// <param name="param"></param>
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
                    case "HDM":
                        var ViewModel = Container.Get<LabelViewModel>();
                        var Res = WindowManager.ShowDialog(ViewModel);
                        Ctrl = StaticResource.CreateControl<HnimeView>(Container.Get<HnimeViewModel>(), new HnimeSearch
                        {
                            Brands = Res.Value ? ViewModel.Brands : null,
                            HnimeType = Res.Value ? ViewModel.Category : string.Empty,
                            KeyWord = param.Values.FirstOrDefault().ToString(),
                            Tags = Res.Value ? ViewModel.Properties : null
                        });
                        break;
                    case "HMH":
                        Ctrl = StaticResource.CreateControl<ComicView>(Container.Get<ComicViewModel>(), param.Values.FirstOrDefault().ToString());
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
                    case "JY":
                        Ctrl = StaticResource.CreateControl<AxgleView>(Container.Get<AxgleViewModel>(), param.Values.FirstOrDefault().ToString());
                        break;
                    default:
                        Ctrl = StaticResource.CreateControl<MovieView>(Container.Get<MovieViewModel>(), param.Values.FirstOrDefault().ToString());
                        break;
                }
            }
        }
        /// <summary>
        /// 继续操作
        /// </summary>
        /// <param name="input"></param>
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
                var Root = (await Container.Get<ICandyAnime>().Get()).FirstOrDefault(t => t.CandyId == AnimeEle.RootId);
                Ctrl = StaticResource.CreateControl<AnimeView>(Container.Get<AnimeViewModel>(), new AnimeDetailResult
                {
                    CollectName = AnimeEle.Name,
                    Cover = Root.Cover,
                    Name = Root.AnimeName,
                    IsDownURL = false,
                    WatchRoute = AnimeEle.Route
                }, "WatchAction");
            }
            if (input is CandyManga Manga)
                Ctrl = StaticResource.CreateControl<MangaView>(Container.Get<MangaViewModel>(), new MangaChapterDetailResult
                {
                    Name = Manga.Name,
                    Route = Manga.Route,
                    TagKey = Manga.Key,
                    Title = Manga.CollectName
                }, "WatchAction");
            if (input is CandyHnime Hnime)
                Ctrl = StaticResource.CreateControl<HnimeView>(Container.Get<HnimeViewModel>(), new HnimePlayResult
                {
                    Title= Hnime.Name,
                    PlayRoute = Hnime.Route,
                    IsPlaying = true
                }, "PlayAction");
            if (input is CandyMovie Movie)
                Ctrl = StaticResource.CreateControl<MovieView>(Container.Get<MovieViewModel>(), Movie.Route, "WatchAction");
            if (input is CandyAxgle Axgle)
                Ctrl = StaticResource.CreateControl<AxgleView>(Container.Get<AxgleViewModel>(), Axgle.Play, "ViewAction");
        }
        /// <summary>
        /// 菜单
        /// </summary>
        /// <param name="input"></param>
        public void HandleAction(string input)
        {
            switch (input.AsInt())
            {
                case 0: //标签
                    Ctrl = StaticResource.CreateControl<CandyImageTemplateView>(Container.Get<CandyImageTemplateViewModel>(), null);
                    break;
                case 1://历史
                    Ctrl = StaticResource.CreateControl<CandyHistoryTemplateView>(Container.Get<CandyHistoryTemplateViewModel>(), null);
                    break;
                case 2://日志
                    Ctrl = StaticResource.CreateControl<CandyLogTemplateView>(Container.Get<CandyLogTemplateViewModel>(), null);
                    break;
                case 3://设置
                    WindowManager.ShowWindow(Container.Get<OptionViewModel>());
                    break;
                default:
                    break;
            }
        }
        #endregion

        public void ScreenActivity(CandyControl input)
        {
            Ctrl = input;
        }

        protected override void OnViewLoaded()
        {
            HandleAction("1");
        }
    }
}

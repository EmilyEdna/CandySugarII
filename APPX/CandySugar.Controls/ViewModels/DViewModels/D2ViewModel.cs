﻿using Sdk.Component.Manga.sdk;
using Sdk.Component.Manga.sdk.ViewModel;
using Sdk.Component.Manga.sdk.ViewModel.Enums;
using Sdk.Component.Manga.sdk.ViewModel.Request;
using System.Collections.ObjectModel;

namespace CandySugar.Controls
{
    public class D2ViewModel : ViewModelBase
    {
        readonly IService Service;
        public D2ViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = this.Container.Resolve<IService>();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            Route= parameters.GetValue<string>("Route");
            Title = parameters.GetValue<string>("Title");
            Task.Run(ContentInit);
        }

        #region Property
        string _Title;
        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }
        string _Route;
        public string Route
        {
            get => _Route;
            set => SetProperty(ref _Route, value);
        }
        ObservableCollection<ImageSource> _Source;
        public ObservableCollection<ImageSource> Source
        {
            get => _Source;
            set => SetProperty(ref _Source, value);
        }
        #endregion

        #region Method
        async void ContentInit()
        {
            try
            {
                Activity = true;
                await Task.Delay(DataBus.Delay);
                var result = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        MangaType = MangaEnum.Content,
                        Content = new MangaContent
                        {
                            Route = Route
                        }
                    };
                }).RunsAsync();
                var bytes = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        MangaType = MangaEnum.Download,
                        Down = new MangaBytes
                        {
                            Route = result.ContentResult.Route,
                            CacheKey = result.ContentResult.CacheKey
                        }
                    };
                }).RunsAsync();
                Source = new ObservableCollection<ImageSource>();
                bytes.DwonResult.Bytes.ForEach(item =>
                {
                    Source.Add(ImageSource.FromStream(() => new MemoryStream(item)));
                });
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("D2ContentInit异常", ex);
                "D2ContentInit异常".OpenToast();
            }
        }
        #endregion

        #region Command
        public DelegateCommand BackCommand => new(() =>
        {
            Nav.GoBackAsync();
        });
        #endregion
    }
}

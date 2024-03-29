﻿using CandySugar.Controls.Views.ImageViews;
using CandySugar.Controls.Views.ImageViews.Popups;
using Sdk.Component.Image.sdk;
using Sdk.Component.Image.sdk.ViewModel;
using Sdk.Component.Image.sdk.ViewModel.Enums;
using Sdk.Component.Image.sdk.ViewModel.Request;
using Sdk.Component.Image.sdk.ViewModel.Response;

namespace CandySugar.Controls.ViewModels
{
    public class ImageViewModel : BaseViewModel
    {

        public ImageViewModel()
        {
            this.Page = 1;
            CandyService = CandyContainer.Instance.Resolves<ICandyService>();
            Task.Run(() => InitImage());
        }

        #region 字段
        readonly int Limit = 10;
        bool LoadMore = false;
        ICandyService CandyService;
        #endregion

        #region 属性
        ObservableCollection<ImageElementResult> _ElementResults;
        public ObservableCollection<ImageElementResult> ElementResults
        {
            get => _ElementResults;
            set => SetProperty(ref _ElementResults, value);
        }
        #endregion

        #region 命令 
        public DelegateCommand<ImageElementResult> ViewAction => new(input =>
        {
            var param = input.OriginalJepg.IsNullOrEmpty() ? input.OriginalPng : input.OriginalJepg;
            Navigation(param);
        });
        public DelegateCommand QueryAction => new(() =>
        {
            SetRefresh();
            LoadMore = false;
            Task.Run(() => InitSearch(KeyWord));
        });
        public DelegateCommand LoadMoreAction => new(() =>
        {
            SetRefresh();
            //网络慢不能使用异步加载更多
            if (Lock) return;
            LoadMore = true;
            this.Page += 1;
            if (this.Page > Total) return;
            if (KeyWord.IsNullOrEmpty()) InitImage();
            else InitImage();
        });
        public DelegateCommand<string> SearchAction => new(input =>
        {
            KeyWord = input;
            SetRefresh();
            LoadMore = false;
            Task.Run(() => InitSearch(input));
        });
        public DelegateCommand RefreshAction => new(() =>
        {
            SetRefresh(false);
            LoadMore = false;
            this.Page = 1;
            if (KeyWord.IsNullOrEmpty()) Task.Run(() => InitImage());
            else Task.Run(() => InitSearch(KeyWord));
        });
        public DelegateCommand LabelPlusAction => new(() =>
        {
            PushPopup();
        });
        public DelegateCommand<ImageElementResult> StarAction => new(input =>
        {
            Logic(input.Preview, input.OriginalPng.IsNullOrEmpty() ? input.OriginalPng : input.OriginalJepg);
        });
        #endregion

        #region 方法
        async void InitImage()
        {
            if (IsBusy) return;
            try
            {
                if (CandySoft.NetState.NetworkAccess != NetworkAccess.Internet)
                {
                    StaticResource.PopToast("请检查网络是否通畅!");
                    return;
                }
                ShowBusy();
                await Task.Delay(CandySoft.Wait);
                var result = await ImageFactory.Image(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        ImageType = ImageEnum.Init,
                        Init = new ImageInit
                        {
                            Page = Page,
                            Limit = Limit,
                            Tag = StaticResource.ImageModule()
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                Total = result.GlobalResult.Total;
                if (!LoadMore)
                    ElementResults = new ObservableCollection<ImageElementResult>(result.GlobalResult.Result);
                else
                {
                    if (ElementResults == null) ElementResults = new ObservableCollection<ImageElementResult>(result.GlobalResult.Result);
                    else result.GlobalResult.Result.ForEach(item => ElementResults.Add(item));
                }
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void InitSearch(string input)
        {
            if (IsBusy) return;
            try
            {
                if (CandySoft.NetState.NetworkAccess != NetworkAccess.Internet)
                {
                    StaticResource.PopToast("请检查网络是否通畅!");
                    return;
                }
                ShowBusy();
                await Task.Delay(CandySoft.Wait);
                var result = await ImageFactory.Image(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = CandySoft.Cache,
                        Proxy = StaticResource.Proxy(),
                        ImplType = StaticResource.ImplType(),
                        ImageType = ImageEnum.Search,
                        Search = new ImageSearch
                        {
                            Page = Page,
                            Limit = Limit,
                            KeyWord = $"{input} {StaticResource.ImageModule()}"
                        }
                    };
                }).RunsAsync();
                CloseBusy();
                Total = result.GlobalResult.Total;
                if (!LoadMore)
                    ElementResults = new ObservableCollection<ImageElementResult>(result.GlobalResult.Result);
                else
                {
                    if (ElementResults == null) ElementResults = new ObservableCollection<ImageElementResult>(result.GlobalResult.Result);
                    else result.GlobalResult.Result.ForEach(item => ElementResults.Add(item));
                }
            }
            catch (Exception ex)
            {
                StaticResource.PopToast(ex.Message);
            }
        }
        async void Navigation(string input)
        {
            await Shell.Current.GoToAsync($"{nameof(ImageDetailView)}?Key={input}");
        }
        async void PushPopup()
        {
            ImageLabelView LabelView = new ImageLabelView();
            await MopupService.Instance.PushAsync(LabelView);
        }
        void Logic(string Preview, string Original)
        {
            CandyService.AddOrAlterImage(new CandyImage
            {
                Preview = Preview,
                Original = Original
            });
            StaticResource.PopToast("收藏成功!");
        }
        #endregion
    }
}

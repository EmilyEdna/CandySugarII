using CandySugar.Library;
using Sdk.Component.Image.sdk.ViewModel;
using Sdk.Component.Image.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdk.Component.Image.sdk.ViewModel.Enums;
using Sdk.Component.Image.sdk.ViewModel.Request;
using Sdk.Component.Image.sdk.ViewModel.Response;
using System.Collections.ObjectModel;
using XExten.Advance.LinqFramework;
using CandySugar.Logic;
using Prism.Ioc;

namespace CandySugar.Controls
{
    public class CViewModel : ViewModelBase
    {
        readonly IService Service;
        public CViewModel(BaseServices baseServices) : base(baseServices)
        {
            Service = base.Container.Resolve<IService>();
        }
        public override void OnLoad()
        {
            Task.Run(() => Init(false));
        }

        /// <summary>
        /// 1 初始化 2 查询
        /// </summary>
        public static int Module = 0;

        #region Property
        public string Key { get; set; }
        public ImageElementResult Element { get; set; }
        #endregion

        #region Property
        ObservableCollection<ImageElementResult> _Result;
        public ObservableCollection<ImageElementResult> Result
        {
            get => _Result;
            set => SetProperty(ref _Result, value);
        }
        ObservableCollection<string> _Tage;
        public ObservableCollection<string> Tage
        {
            get => _Tage;
            set => SetProperty(ref _Tage, value);
        }
        #endregion

        #region Method
        async void Init(bool More)
        {
            try
            {
                Module = 1;
                SetActivity();
                await Task.Delay(DataBus.Delay);
                var result = await ImageFactory.Image(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        ImageType = ImageEnum.Init,
                        Init = new ImageInit
                        {
                            Page = Page,
                            Limit = 10,
                            Tag = DataCenter.ImageType()
                        }
                    };
                }).RunsAsync();
                Total = result.GlobalResult.Total;
                if (More)
                {
                    if (Result == null) Result = new ObservableCollection<ImageElementResult>(result.GlobalResult.Result);
                    else result.GlobalResult.Result.ForEach(Result.Add);
                }
                else
                    Result = new ObservableCollection<ImageElementResult>(result.GlobalResult.Result);
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("CInit异常", ex);
                "CInit异常".OpenToast();
            }
        }
        async void Add(ImageElementResult input)
        {
            var Entity = new CRootEntity
            {
                Original = input.OriginalPng.IsNullOrEmpty() ? input.OriginalPng : input.OriginalJepg,
                Preview = input.Preview,
                Children = input.Labels.Select(t => new CElementEntity { Name = t }).ToList()
            };
            if (await Service.CAdd(Entity)) "收藏成功".OpenToast(); else "收藏失败".OpenToast();
        }
        public async void QueryInit(bool More)
        {
            try
            {
                Module = 2;
                SetActivity();
                await Task.Delay(DataBus.Delay);
                var result = await ImageFactory.Image(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        CacheSpan = DataBus.Cache,
                        ImplType = DataCenter.ImplType(),
                        ImageType = ImageEnum.Search,
                        Search = new ImageSearch
                        {
                            Page = Page,
                            Limit = 10,
                            KeyWord = $"{Key} {DataCenter.ImageType()}"
                        }
                    };
                }).RunsAsync();
                Total = result.GlobalResult.Total;
                if (More)
                {
                    if (Result == null) Result = new ObservableCollection<ImageElementResult>(result.GlobalResult.Result);
                    else result.GlobalResult.Result.ForEach(Result.Add);
                }
                else
                    Result = new ObservableCollection<ImageElementResult>(result.GlobalResult.Result);
                SetState();
            }
            catch (Exception ex)
            {
                await Service.AddLog("CQueryInit异常", ex);
                "CQueryInit异常".OpenToast();
            }
        }
        #endregion

        #region Command
        public DelegateCommand RefreshCommand => new(() =>
        {
            this.Page = 1;
            if (Module == 1) Task.Run(() => Init(false));
            if (Module == 2) Task.Run(() => QueryInit(false));
        });
        public DelegateCommand MoreCommand => new(() =>
        {
            this.Page += 1;
            if (this.Page > this.Total) return;
            if (Module == 1) Task.Run(() => Init(true));
            if (Module == 2) Task.Run(() => QueryInit(true));
        });
        public DelegateCommand<string> TypeCammand => new(input =>
        {
            this.Key = input;
            Task.Run(() => QueryInit(false));
        });
        public DelegateCommand<ImageElementResult> ViewCommand => new(input =>
        {
            Tage = new ObservableCollection<string>(input.Labels);
            Element = input;
        });
        public DelegateCommand<ImageElementResult> LikeCommand => new(input =>
        {
            Task.Run(() => Add(input));
        });
        public DelegateCommand ShowCammand => new(() =>
        {
            Nav.NavigateAsync(new Uri("C1", UriKind.Relative), new NavigationParameters { { "Route", Element.OriginalPng.IsNullOrEmpty() ? Element.OriginalPng : Element.OriginalJepg } });
        });
        #endregion
    }
}

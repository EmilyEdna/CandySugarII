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

namespace CandySugar.Controls
{
    public class CViewModel : ViewModelBase
    {
        public CViewModel(BaseServices baseServices) : base(baseServices)
        {
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
            Module = 1;
            if (!More) Activity = true;
            await Task.Delay(100);
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
            if (!More)
                Result = new ObservableCollection<ImageElementResult>(result.GlobalResult.Result);
            else
            {
                if (Result == null) Result = new ObservableCollection<ImageElementResult>(result.GlobalResult.Result);
                else result.GlobalResult.Result.ForEach(Result.Add);
            }
            Activity = false;
        }
        async void QueryInit(bool More)
        {
            Module = 2;
            if (!More) Activity = true;
            await Task.Delay(100);
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
            if (!More)
                Result = new ObservableCollection<ImageElementResult>(result.GlobalResult.Result);
            else
            {
                if (Result == null) Result = new ObservableCollection<ImageElementResult>(result.GlobalResult.Result);
                else result.GlobalResult.Result.ForEach(Result.Add);
            }
            Activity = false;
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
        #endregion
    }
}

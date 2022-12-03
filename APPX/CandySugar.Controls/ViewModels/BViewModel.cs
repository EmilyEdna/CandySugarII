using CandySugar.Library;
using Sdk.Component.Anime.sdk;
using Sdk.Component.Anime.sdk.ViewModel;
using Sdk.Component.Anime.sdk.ViewModel.Enums;
using Sdk.Component.Anime.sdk.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sdk.Component.Anime.sdk.ViewModel.Request;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;

namespace CandySugar.Controls
{
    public class BViewModel : ViewModelBase
    {
        public BViewModel(BaseServices baseServices) : base(baseServices) { }

        public override void OnLoad()
        {
            Task.Run(() => Init());
        }
        /// <summary>
        /// 1 查询 2 字母分类 3类被分类
        /// </summary>
        public static int Module = 0;

        #region Property
        public string Type { get; set; }
        public string Group { get; set; }
        public string Key { get; set; }
        #endregion

        #region Property
        /// <summary>
        /// 字母
        /// </summary>
        ObservableCollection<string> _Words;
        public ObservableCollection<string> Words
        {
            get => _Words;
            set => SetProperty(ref _Words, value);
        }
        /// <summary>
        /// 初始化结果
        /// </summary>
        ObservableCollection<AnimeWeekDayIndexResult> _InitResult;
        public ObservableCollection<AnimeWeekDayIndexResult> InitResult
        {
            get => _InitResult;
            set => SetProperty(ref _InitResult, value);
        }
        /// <summary>
        /// 检索分类结果
        /// </summary>
        ObservableCollection<AnimeSearchElementResult> _SearchResult;
        public ObservableCollection<AnimeSearchElementResult> SearchResult
        {
            get => _SearchResult;
            set => SetProperty(ref _SearchResult, value);
        }
        #endregion

        #region Method
        async void Init()
        {
            Activity = true;
            await Task.Delay(100);
            var result = await AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = DataBus.Cache,
                    ImplType = DataCenter.ImplType(),
                    AnimeType = AnimeEnum.Init
                };
            }).RunsAsync();
            this.Words = new ObservableCollection<string>(result.InitResult.Letters.Where(t => !t.Equals("全部")));
            InitResult = new ObservableCollection<AnimeWeekDayIndexResult>(result.InitResult.RecResults);
            Activity = false;
        }
        async void TypeInit(bool More)
        {
            Module = 2;
            Activity = true;
            await Task.Delay(100);
            var result = await AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = DataBus.Cache,
                    ImplType = DataCenter.ImplType(),
                    AnimeType = AnimeEnum.Category,
                    Category = new AnimeCategory
                    {
                        LetterType = Enum.Parse<AnimeLetterEnum>(Type),
                        Page = this.Page
                    }
                };
            }).RunsAsync();
            Total = result.SeachResult.Total;
            if (More)
                result.SeachResult.ElementResult.ForEach(item =>
                {
                    SearchResult.Add(item);
                });
            else
                SearchResult = new ObservableCollection<AnimeSearchElementResult>(result.SeachResult.ElementResult);
            Activity = false;
        }
        async void GroupInit(bool More)
        {
            Module = 3;
            Activity = true;
            await Task.Delay(100);
            var result = await AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = DataBus.Cache,
                    ImplType = DataCenter.ImplType(),
                    AnimeType = AnimeEnum.CategoryType,
                    Category = new AnimeCategory
                    {
                        Route = Group,
                        Page = this.Page
                    }
                };
            }).RunsAsync();
            Total = result.SeachResult.Total;
            if (More)
                result.SeachResult.ElementResult.ForEach(item =>
                {
                    SearchResult.Add(item);
                });
            else
                SearchResult = new ObservableCollection<AnimeSearchElementResult>(result.SeachResult.ElementResult);
            Activity = false;
        }
        public async void QueryInit(bool More)
        {
            Module = 1;
            Activity = true;
            await Task.Delay(100);
            var result = await AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    CacheSpan = DataBus.Cache,
                    ImplType = DataCenter.ImplType(),
                    AnimeType = AnimeEnum.Search,
                    Search = new AnimeSearch
                    {
                        KeyWord = Key,
                        Page = this.Page
                    }
                };
            }).RunsAsync();
            Total = result.SeachResult.Total;
            if (More)
                result.SeachResult.ElementResult.ForEach(item =>
                {
                    SearchResult.Add(item);
                });
            else
                SearchResult = new ObservableCollection<AnimeSearchElementResult>(result.SeachResult.ElementResult);
            Activity = false;
        }
        #endregion

        #region Command
        public DelegateCommand<string> TypeCammand => new(input =>
        {
            this.Type = input;
            Task.Run(() => TypeInit(false));
        });
        public DelegateCommand<string> GroupCammand => new(input =>
        {
            this.Group = input;
            Task.Run(() => GroupInit(false));
        });
        public DelegateCommand<string> DetailCommand => new(input => { });
        public DelegateCommand RefreshCommand => new(() =>
        {
            this.Page = 1;
            if (Module == 1) QueryInit(false);
            if (Module == 2) TypeInit(false);
            if (Module == 3) GroupInit(false);
        });
        public DelegateCommand MoreCommand => new(() => { 
        
            this.Page+= 1;
            if (this.Page > this.Total) return;
            if (Module == 1) QueryInit(true);
            if (Module == 2) TypeInit(true);
            if (Module == 3) GroupInit(true);
        });
        #endregion

    }
}

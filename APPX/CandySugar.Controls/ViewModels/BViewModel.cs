using CandySugar.Library;
using Sdk.Component.Anime.sdk;
using Sdk.Component.Anime.sdk.ViewModel;
using Sdk.Component.Anime.sdk.ViewModel.Enums;
using Sdk.Component.Anime.sdk.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls
{
    public class BViewModel : ViewModelBase
    {
        public BViewModel(BaseServices baseServices) : base(baseServices) { }

        public override void OnLoad()
        {
            Task.Run(() => Init());
        }

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
        #endregion
    }
}

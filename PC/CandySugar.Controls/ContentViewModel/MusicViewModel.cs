using Sdk.Component.Music.sdk.ViewModel;
using Sdk.Component.Music.sdk;
using Sdk.Component.Music.sdk.ViewModel.Enums;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CandySugar.Library;
using Sdk.Component.Music.sdk.ViewModel.Request;
using Sdk.Component.Music.sdk.ViewModel.Response;
using System.Collections.ObjectModel;

namespace CandySugar.Controls.ContentViewModel
{
    public class MusicViewModel : Screen
    {
        public IContainer Container;
        public IWindowManager WindowManager;
        public MusicViewModel(IContainer Container, IWindowManager WindowManager)
        {
            this.WindowManager = WindowManager;
            this.Container = Container;
            this.PlatformType = PlatformEnum.NeteaseMusic;
            this.Handle = new Dictionary<PlatformEnum, string>
            {
                { PlatformEnum.QQMusic,"QQ"},
                { PlatformEnum.NeteaseMusic,"网易"},
                { PlatformEnum.KuGouMusic,"酷狗"},
                { PlatformEnum.KuWoMusic,"酷我"},
                { PlatformEnum.BiliBiliMusic,"B站"},
                { PlatformEnum.MiGuMusic,"咪咕"}
            };
            OnViewLoaded();
        }
        #region Field
        private PlatformEnum PlatformType;
        private string QueryWord;
        #endregion

        #region CommomProperty_Bool
        private bool _Loading;
        public bool Loading
        {
            get => _Loading;
            set => SetAndNotify(ref _Loading, value);
        }
        #endregion

        #region ComomProperty_Int
        private int? _Total;
        public int? Total
        {
            get => _Total;
            set => SetAndNotify(ref _Total, value);
        }
        private int _Page;
        public int Page
        {
            get => _Page;
            set => SetAndNotify(ref _Page, value);
        }
        #endregion

        #region Property
        public Dictionary<PlatformEnum, string> Handle { get; set; }
        private ObservableCollection<MusicSongElementResult> _ItemResult;
        public ObservableCollection<MusicSongElementResult> ItemResult
        {
            get => _ItemResult;
            set => SetAndNotify(ref _ItemResult, value);
        }
        #endregion

        #region Override
        protected override void OnViewLoaded()
        {
            SearchAction("Sakura");
        }
        #endregion

        #region Action
        public void SearchAction(string input)
        {
            this.QueryWord = input;
            InitSearch(input);
        }
        public void CategoryAction(PlatformEnum input)
        {
            this.PlatformType = input;
            InitSearch(this.QueryWord);
        }
        #endregion

        #region Method
        private async void InitSearch(string input)
        {
            var MusicQueryData = await MusicFactory.Music(opt =>
            {
                opt.RequestParam = new Input
                {
                    PlatformType = this.PlatformType,
                    MusicType = MusicEnum.Song,
                    ImplType = StaticResource.ImplType(),
                    Search = new MusicSearch
                    {
                        KeyWord = input
                    }
                };
            }).RunsAsync();

            Total = MusicQueryData.SongResult.Total;
            ItemResult = new ObservableCollection<MusicSongElementResult>(MusicQueryData.SongResult.ElementResults);
        }
        #endregion
    }
}

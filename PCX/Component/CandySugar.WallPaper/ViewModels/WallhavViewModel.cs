using CandySugar.Com.Controls.UIExtenControls;
using CandySugar.Com.Library;
using CandySugar.Com.Options.ComponentGeneric;
using Sdk.Component.Wallhav.sdk.ViewModel;
using Sdk.Component.Wallhav.sdk.ViewModel.Enums;
using Sdk.Component.Wallhav.sdk;
using Serilog;
using System.Windows;
using XExten.Advance.LinqFramework;
using CandySugar.Com.Options.ComponentObject;
using Sdk.Component.Plugins;
using Sdk.Component.Wallhav.sdk.ViewModel.Request;
using Sdk.Component.Wallhav.sdk.ViewModel.Response;
using System.Collections.ObjectModel;
using System.Windows.Data;
using CommunityToolkit.Mvvm.Input;

namespace CandySugar.WallPaper.ViewModels
{
    public class WallhavViewModel : PropertyChangedBase
    {
        private object LockObject = new object();
        public WallhavViewModel()
        {
            GenericDelegate.SearchAction = new(SearchHandler);
            OnInit();
        }

        #region Field
        private string Keyword;
        private int Total;
        #endregion

        #region Property
        private ObservableCollection<WallhavSearchElementResult> _ElementResult;
        public ObservableCollection<WallhavSearchElementResult> ElementResult
        {
            get => _ElementResult;
            set => SetAndNotify(ref _ElementResult, value);
        }
        #endregion

        #region Method
        private void OnInit()
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = (await WallhavFactory.Wall(opt =>
                    {
                        opt.RequestParam = new Input
                        {

                            CacheSpan = ComponentBinding.OptionObjectModels.Cache,
                            ImplType = SdkImpl.Rest,
                            WallhavType = WallhavEnum.Search,
                            Search = new WallhavSearch
                            {
                                KeyWord = this.Keyword,
                                PageIndex = 1,
                                QueryType = QueryEnum.General
                            }

                        };
                    }).RunsAsync()).SearchResult;
                    Total = result.Total;
                    ElementResult = new ObservableCollection<WallhavSearchElementResult>(result.ElementResult);
                    BindingOperations.EnableCollectionSynchronization(ElementResult, LockObject);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "");
                    ErrorNotify();
                }
            });
        }
        private void ErrorNotify(string Info = "")
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                new ScreenNotifyView(Info.IsNullOrEmpty() ? CommonHelper.ComponentErrorInformation : Info).Show();
            });
        }
        #endregion

        #region Command

        /// <summary>
        /// 加载更多
        /// </summary>
        public RelayCommand<ScrollChangedEventArgs> ScrollCommand => new((obj) =>
        {
           
        });
        #endregion

        #region ExternalCalls
        /// <summary>
        /// 检索数据
        /// </summary>
        /// <param name="keyword"></param>
        private void SearchHandler(string keyword)
        {
            this.Keyword = keyword;
            OnInit();
        }
        #endregion
    }
}

using CandySugar.Library;
using Sdk.Component.Bgm.sdk;
using Sdk.Component.Bgm.sdk.ViewModel;
using Sdk.Component.Bgm.sdk.ViewModel.Enums;
using Sdk.Component.Bgm.sdk.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Controls
{
    public class AViewModel : ViewModelBase
    {
        public AViewModel(BaseServices baseServices) : base(baseServices)
        {
        }
        public override void OnLoad()
        {
            Init();
        }

        #region Property
        ObservableCollection<TabItem> _Result;
        public ObservableCollection<TabItem> Result
        {
            get => _Result;
            set => SetProperty(ref _Result, value);
        }
        #endregion

        #region Method
        async void Init()
        {
            var result = await BgmFactory.Bgm(opt =>
             {
                 opt.RequestParam = new Input
                 {
                     CacheSpan = DataBus.Cache,
                     ImplType = DataCenter.ImplType(),
                     BgmType = BgmEnum.Calendar,
                 };
             }).RunsAsync();
            List<TabItem> model = new List<TabItem>();
            #region 周一到周六
            result.InitResults.Where(t=>!t.WeekDay.Equals("星期日")).ForEnumerEach(item =>
            {
                var layout = new VerticalStackLayout
                {
                    HorizontalOptions = LayoutOptions.Fill,
                };
                item.Name.ForEach(node =>
                {
                    var lbl = new Label
                    {
                        Text = node,
                        Margin = new Thickness(5),
                        FontSize = 18,
                    };
                    lbl.SetAppTheme(Label.TextColorProperty, Color.FromRgb(0, 0, 0), Color.FromRgb(255, 255, 255));
                    layout.Children.Add(lbl);
                });

                var tab = new TabItem
                {
                    Title = item.WeekDay,
                    Content = layout
                };

                model.Add(tab);
            });
            #endregion
            #region 周日
            var week = result.InitResults.FirstOrDefault();

            var layout = new VerticalStackLayout
            {
                HorizontalOptions = LayoutOptions.Fill,
            };
            week.Name.ForEach(node =>
            {
                var lbl = new Label
                {
                    Text = node,
                    Margin = new Thickness(5),
                    FontSize = 18,
                };
                lbl.SetAppTheme(Label.TextColorProperty, Color.FromRgb(0, 0, 0), Color.FromRgb(255, 255, 255));
                layout.Children.Add(lbl);
            });

            var tab = new TabItem
            {
                Title = week.WeekDay,
                Content = layout
            };

            model.Add(tab);
            #endregion
            Result = new ObservableCollection<TabItem>(model);
        }
        #endregion
    }
}

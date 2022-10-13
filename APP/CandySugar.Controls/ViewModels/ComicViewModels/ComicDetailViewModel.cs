using CandySugar.Controls.ViewModels.ComicViewModels.Dto;
using CandySugar.Controls.Views.ComicViews;
using Microsoft.Maui.Storage;
using Sdk.Component.Comic.sdk.ViewModel.Response;
using Sdk.Component.Lovel.sdk.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CandySugar.Controls.ViewModels.ComicViewModels
{
    public class ComicDetailViewModel : BaseViewModel
    {
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            SearchResult = query["Key"] as ComicSearchElementResult;
            ViewResult = query["View"] as ComicViewResult;

            Properties = new ObservableCollection<ComicProperty>(ViewResult.Tag.Select(t => new ComicProperty
            {
                Name = t.Key,
                Value = t.Value
            }));

            foreach (var Author in ViewResult.Author)
            {
                Properties.Add(new ComicProperty
                {
                    Name = $"作者:{Author.Key}",
                    Value = Author.Value
                });
            }
            foreach (var Group in ViewResult.Group)
            {
                Properties.Add(new ComicProperty
                {
                    Name = $"团体:{Group.Key}",
                    Value = Group.Value
                });
            }
            foreach (var Category in ViewResult.Category)
            {
                Properties.Add(new ComicProperty
                {
                    Name = $"分类:{Category.Key}",
                    Value = Category.Value
                });
            }
            foreach (var Parodies in ViewResult.Parodies)
            {
                Properties.Add(new ComicProperty
                {
                    Name = $"同人:{Parodies.Key}",
                    Value = Parodies.Value
                });
            }
            foreach (var Language in ViewResult.Language)
            {
                Properties.Add(new ComicProperty
                {
                    Name = $"语言:{Language.Key}",
                    Value = Language.Value
                });
            }
        }

        #region 属性
        ComicSearchElementResult _SearchResult;
        public ComicSearchElementResult SearchResult
        {
            get => _SearchResult;
            set => SetProperty(ref _SearchResult, value);
        }
        ComicViewResult _ViewResult;
        public ComicViewResult ViewResult
        {
            get => _ViewResult;
            set => SetProperty(ref _ViewResult, value);
        }
        ObservableCollection<ComicProperty> _Properties;
        public ObservableCollection<ComicProperty> Properties
        {
            get => _Properties;
            set => SetProperty(ref _Properties, value);
        }
        #endregion

        #region 命令
        public DelegateCommand<string> CategoryAction => new(input => GoBack(input));
        public DelegateCommand ViewAction => new(() => GoView());
        #endregion

        #region 方法
        async void GoBack(string input)
        {
            await Shell.Current.GoToAsync($"..", new Dictionary<string, object> { { "Key", input } });
        }
        async void GoView()
        {
            await Shell.Current.GoToAsync(nameof(ComicWatchView), new Dictionary<string, object> { { "Key", ViewResult.Realviews } });
        }
        #endregion
    }
}

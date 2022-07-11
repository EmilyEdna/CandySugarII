﻿using CandySugar.Controls.Views.LovelViews;
using CandySugar.Library;
using Sdk.Component.Lovel.sdk.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.ViewModels.LovelViewModels
{
    public class LovelContentViewModel : BaseViewModel
    {
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Title = query["Title"].ToString();
            EleResult = query["Data"] as LovelCategoryElementResult;
            var temp = (query["Result"] as LovelContentResult);

            if (!temp.Content.IsNullOrEmpty())
            {
                Views = new ObservableCollection<string>(temp.Content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(t => $"\r\n\t\t\t\t{t}"));
                Content = true;
                Image = false;
            }
            else
            {
                Pic = new ObservableCollection<string>(temp.Image);
                Image = true;
                Content = false;
            }
        }
        #region 属性
        string _Title;
        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }

        bool _Content;
        public bool Content
        {
            get => _Content;
            set => SetProperty(ref _Content, value);
        }

        bool _Image;
        public bool Image
        {
            get => _Image;
            set => SetProperty(ref _Image, value);
        }

        ObservableCollection<string> _Views;
        public ObservableCollection<string> Views
        {
            get => _Views;
            set => SetProperty(ref _Views, value);
        }
        ObservableCollection<string> _Pic;
        public ObservableCollection<string> Pic
        {
            get => _Pic;
            set => SetProperty(ref _Pic, value);
        }
        LovelCategoryElementResult _EleResult;
        public LovelCategoryElementResult EleResult
        {
            get => _EleResult;
            set => SetProperty(ref _EleResult, value);
        }
        #endregion

        #region 命令
        public DelegateCommand BackAction => new(async () =>
        {
            await Shell.Current.GoToAsync(nameof(LovelDetailView), new Dictionary<string, object> { { "Route",EleResult} });
        });
        #endregion
    }
}
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CandySugar.LightNovel.ViewModels
{
    public class IndexViewModel : PropertyChangedBase
    {
        public IndexViewModel()
        {
            Test = "123";
        }


        public void SearchHandler(string keyword)
        {
            Test = keyword;
        }

        #region Property
        private string _Test;
        public string Test
        {
            get => _Test;
            set => SetAndNotify(ref _Test, value);
        }
        #endregion
    }
}

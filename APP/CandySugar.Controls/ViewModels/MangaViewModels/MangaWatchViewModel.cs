using CandySugar.Library;
using Jint.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.ViewModels.MangaViewModels
{
    public class MangaWatchViewModel : BaseViewModel
    {
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Source = new ObservableCollection<ImageSource>();
            (query["Result"] as List<byte[]>).ForEach(item =>
            {
                Source.Add(ImageSource.FromStream(() => new MemoryStream(item)));
            });
        }


        #region 属性
        ObservableCollection<ImageSource> _Source;
        public ObservableCollection<ImageSource> Source
        {
            get => _Source;
            set => SetProperty(ref _Source, value);
        }
        #endregion
    }
}

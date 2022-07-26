using Sdk.Component.Music.sdk.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls.ViewModels.MusicViewModels
{
    public class MusicAlbumViewModel : BaseViewModel
    {
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            AlbumResult = new(query["Data"] as List<MusicSongElementResult>);
        }

        #region 属性
        ObservableCollection<MusicSongElementResult> _AlbumResult;
        public ObservableCollection<MusicSongElementResult> AlbumResult
        {
            get => _AlbumResult;
            set => SetProperty(ref _AlbumResult, value);
        }
        #endregion

        #region 命令
        public DelegateCommand<MusicSongElementResult> AddPlayAction => new(input => {
        });
        #endregion
    }
}

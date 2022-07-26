using Sdk.Component.Music.sdk.ViewModel;
using Sdk.Component.Music.sdk;
using Sdk.Component.Music.sdk.ViewModel.Response;
using Sdk.Component.Music.sdk.ViewModel.Enums;
using Sdk.Component.Music.sdk.ViewModel.Request;

namespace CandySugar.Controls.ViewModels.MusicViewModels
{
    public class MusicDetailViewModel : BaseViewModel
    {
        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            DetailResult = query["Data"] as MusicSheetDetailRootResult;
            PlayId = query["Key"].ToString();
        }

        #region 属性
        public string PlayId { get; set; }
        MusicSheetDetailRootResult _DetailResult;
        public MusicSheetDetailRootResult DetailResult
        {
            get => _DetailResult;
            set => SetProperty(ref _DetailResult, value);
        }
        #endregion

        #region 命令
        public DelegateCommand<MusicSongElementResult> AddPlayAction => new(input => { 
        });
        #endregion
    }
}

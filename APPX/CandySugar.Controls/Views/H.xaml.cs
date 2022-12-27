
using Esprima;

namespace CandySugar.Controls;

public partial class H : CandyUIView
{
    HViewModel ViewModel;
    public H()
    {
        InitializeComponent();
        Lbl.Text= string.Empty;
        this.Loaded += (_, _) =>
        {
            ViewModel = this.BindingContext as HViewModel;

            ViewModel.PlayService.PositionChanged += PlayService_PositionChanged;
            ViewModel.PlayService.IsPlayingChanged += PlayService_IsPlayingChanged;
        };

    }
    private void PlayService_IsPlayingChanged(object sender, EventArgs e)
    {
        var play = (sender as PlayService);
        Lbl.Text = "/";
        Songs.Text = play.CurrentMusic.Name;
        Header.Dispatcher.Dispatch(() =>
        {
            Header.Source = play.IsPlaying ? "pause.png" : "play.png";
            Header.GestureRecognizers.Clear();
            TapGestureRecognizer tap = new TapGestureRecognizer
            {
                Command = ViewModel.PlayOrPauseCommand,
            };
            Header.GestureRecognizers.Add(tap);
        });
    }
    bool _IsPlayProgressDragging;
    private void PlayService_PositionChanged(object sender, EventArgs e)
    {
        var play = (sender as PlayService);
        MainThread.BeginInvokeOnMainThread(() =>
        {
            LblInfo.Text = $"{play.CurrentPosition.Position.Minutes:D2}:{play.CurrentPosition.Position.Seconds:D2}";
           
            LblDuration.Text = $"{play.CurrentPosition.Duration.Minutes:D2}:{play.CurrentPosition.Duration.Seconds:D2}";
            if (!_IsPlayProgressDragging)
            {
                Progress.Value = play.CurrentPosition.PlayProgress;
            }
        });
    }

    private async void ProgressCompleted(object sender, EventArgs e)
    {
        if (ViewModel.PlayService.CurrentMusic != null)
        {
            var sliderPlayProgress = sender as Slider;
            if (sliderPlayProgress != null)
            {
                var positionMillisecond = ViewModel.PlayService.CurrentPosition.Duration.TotalMilliseconds * sliderPlayProgress.Value;
                await ViewModel.PlayService.SetPlayPosition(positionMillisecond);
            }
        }
        _IsPlayProgressDragging = false;
    }

    private void ProgressStarted(object sender, EventArgs e)
    {
        _IsPlayProgressDragging = true;
    }
}
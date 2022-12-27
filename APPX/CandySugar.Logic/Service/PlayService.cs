using CandySugar.Library.Common.Audio;
using Esprima;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic
{
    public class PlayService : IPlayService
    {
        readonly INativeAudioService _AudioService;
        readonly IService _Service;
        System.Timers.Timer _PlayProgress;
        public bool IsPlaying { get; set; }
        public HRootEntity CurrentMusic { get; set; } = null;
        public MusicPosition CurrentPosition { get; set; } = new MusicPosition();
        public event EventHandler? IsPlayingChanged;
        public event EventHandler? PositionChanged;
        public PlayService(INativeAudioService AudioService, IService Service)
        {
            _Service = Service;
            _AudioService = AudioService;

            _PlayProgress = new System.Timers.Timer();
            _PlayProgress.Interval = 1000;
            _PlayProgress.Elapsed += PlayProgress;
            _PlayProgress.Start();

            _AudioService.PlayFinished -= PlayFinishedAndFailed;
            _AudioService.PlayFailed -= PlayFinishedAndFailed;

            _AudioService.Played -= PlayAndPause;
            _AudioService.Paused -= PlayAndPause;
            _AudioService.SkipToNext -= Next;
            _AudioService.SkipToPrevious -= Previous;

            _AudioService.PlayFinished += PlayFinishedAndFailed;
            _AudioService.PlayFailed += PlayFinishedAndFailed;

            _AudioService.Played += PlayAndPause;
            _AudioService.Paused += PlayAndPause;
            _AudioService.SkipToNext += Next;
            _AudioService.SkipToPrevious += Previous;
        }

        private void PlayProgress(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!IsPlaying)
            {
                return;
            }

            CurrentPosition.Position = TimeSpan.FromMilliseconds(_AudioService.CurrentPositionMillisecond);
            CurrentPosition.Duration = TimeSpan.FromMilliseconds(_AudioService.CurrentDurationMillisecond);
            CurrentPosition.PlayProgress = _AudioService.CurrentPositionMillisecond / _AudioService.CurrentDurationMillisecond;

            PositionChanged?.Invoke(this, EventArgs.Empty);
        }

        async Task InternalPlayPauseAsync(bool isPlaying, double position)
        {
            if (isPlaying)
            {
                await InternalPlayAsync(position);
            }
            else
            {
                await InternalPauseAsync();
            }
        }
        async Task InternalPauseAsync()
        {
            await _AudioService.PauseAsync();
            IsPlaying = false;
        }
        async Task InternalPlayAsync(double positionMillisecond = 0)
        {
            await _AudioService.PlayAsync(positionMillisecond);
            IsPlaying = true;
        }

        #region Event
        async void PlayFinishedAndFailed(object obj, EventArgs args)
        {
            await Next();
        }
        async void Next(object obj, EventArgs args)
        {
            await Next();
        }

        async void Previous(object obj, EventArgs args)
        {
            await Previous();
        }

        async void PlayAndPause(object obj, EventArgs args)
        {
            await PlayAsync(CurrentMusic);
        }
        #endregion

        public async Task PlayAsync(HRootEntity input)
        {
            if (File.Exists(input.Route) == false)
            {
                "歌曲缓存文件不存在".OpenToast();
                return;
            }
            var isOtherMusic = CurrentMusic?.Id != input.Id;
            var isPlaying = isOtherMusic || !_AudioService.IsPlaying;
            var position = isOtherMusic ? 0 : CurrentPosition.Position.TotalMilliseconds;

            //当前播放
            CurrentMusic = input;
            if (isOtherMusic)
            {
                if (_AudioService.IsPlaying)
                {
                    await InternalPauseAsync();
                }

                await _AudioService.InitializeAsync(input.Route, new AudioMetadata(RawResources.Head, input.Name, input.ArtistName, input.AlbumName));

                await InternalPlayPauseAsync(isPlaying, position);
            }
            else
            {
                await InternalPlayPauseAsync(isPlaying, position);
            }
            IsPlayingChanged?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// 上一首
        /// </summary>
        public async Task Previous()
        {
            HRootEntity previousMusic = null;
            var data = await _Service.HQuery();
            var index = data.FindIndex(0, t => t.Id == CurrentMusic.Id);
            if (data.Count == 1)
            {
                await PlayAsync(data[index]);
            }
            else
            {
                if (index == 0 || index - 1 < 0)
                {
                    previousMusic = data.LastOrDefault();
                    CurrentMusic = null;
                    await PlayAsync(previousMusic);
                }
                else {
                    previousMusic = data[index - 1];
                    CurrentMusic = null;
                    await PlayAsync(previousMusic);
                }
            }
        }

        /// <summary>
        /// 下一首
        /// </summary>
        public async Task Next()
        {
            HRootEntity nextMusic = null;
            var data = await _Service.HQuery();
            var index = data.FindIndex(0, t => t.Id == CurrentMusic.Id);
            if (data.Count == 1)
            {
                await PlayAsync(data[index]);
            }
            else
            {
                if (index == data.Count|| index + 1 >= data.Count)
                {
                    nextMusic = data.FirstOrDefault();
                    CurrentMusic = null;
                    await PlayAsync(nextMusic);
                }
                else
                {
                    nextMusic = data[index + 1];
                    CurrentMusic = null;
                    await PlayAsync(nextMusic);
                }
            }
        }

        public async Task SetPlayPosition(double positionMillisecond)
        {
            await InternalPlayAsync(positionMillisecond);
            IsPlayingChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

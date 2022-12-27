using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Logic
{
    public interface IPlayService
    {
        HRootEntity CurrentMusic { get; set; }
        MusicPosition CurrentPosition { get; set; }
        event EventHandler? IsPlayingChanged;
        event EventHandler? PositionChanged;
        Task PlayAsync(HRootEntity input);
        Task SetPlayPosition(double positionMillisecond);
    }
}

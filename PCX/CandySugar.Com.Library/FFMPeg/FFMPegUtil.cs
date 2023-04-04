using CliWrap;
using Serilog;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Com.Library.FFMPeg
{
    public static class FFMPegUtil
    {
        public static async Task<bool> Mp3ToHighMP3(this string mp3File, string catalog)
        {
            StringBuilder Info = new StringBuilder();

            var cmd = await Cli.Wrap(CommonHelper.FFMPEG)
                    .WithArguments($" -i {Path.Combine(catalog, mp3File)} -ab 320k -acodec libmp3lame  -y {Path.Combine(catalog, $"[High]{mp3File}")}")
                     .WithStandardErrorPipe(PipeTarget.ToStringBuilder(Info))
                     .ExecuteAsync();
            Log.Logger.Information(Info.ToString());
            return cmd.ExitCode == 0;
        }
    }
}

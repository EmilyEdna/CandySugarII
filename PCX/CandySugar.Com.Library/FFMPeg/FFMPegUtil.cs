using CandySugar.Com.Library.FileDown;
using Serilog;
using System.Diagnostics;
using System.IO;
using XExten.Advance.StaticFramework;

namespace CandySugar.Com.Library.FFMPeg
{
    public static class FFMPegUtil
    {

        public static bool Mp3ToHighMP3(this string mp3File, string catalog)
        {
            Process process = new Process();
            process.StartInfo.FileName = CommonHelper.FFMPEG;
            process.StartInfo.Arguments = $" -i {Path.Combine(catalog, mp3File)} -ab 320k -acodec libmp3lame  -y {Path.Combine(catalog, $"[High]{mp3File}")}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.ErrorDataReceived += (sender, e) =>
            {
                Process p = sender as Process;
                // 在ffmpeg发生错误的时候才输出信息
                if (p.HasExited && p.ExitCode == 1)
                {
                    Log.Logger.Information(e.Data);
                }
            };
            process.Start();
            process.BeginErrorReadLine();   // 开始异步读取
            process.WaitForExit();    // 等待转码完成

            if (process.ExitCode == 0)
            {
                Log.Logger.Information("ffmpeg 转码成功！");           
                return true;
            }
            else
            {
                Log.Logger.Error("ffmpeg 程序发生错误，转码失败！");
                return false;
            }
        }

    }
}

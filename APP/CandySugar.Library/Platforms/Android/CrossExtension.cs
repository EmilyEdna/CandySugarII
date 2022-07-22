using CandySugar.Library.AndroidCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Env = Android.OS.Environment;


namespace CandySugar.Library.Platforms.Android
{
    public class CrossExtension: ICrossExtension
    {
        public string AndriodPath => Env.GetExternalStoragePublicDirectory(Env.DirectoryDownloads).AbsoluteFile + "";
    }
}

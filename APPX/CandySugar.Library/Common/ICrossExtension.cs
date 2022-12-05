using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Common
{
    /// <summary>
    /// 跨平台拓展
    /// </summary>
    public interface ICrossExtension
    {
        static ICrossExtension Instance=>  new Lazy<ICrossExtension>(() =>
            {
#if ANDROID
                return new Library.Platforms.Android.CrossExtension();
#else
                return null;
#endif
            }, LazyThreadSafetyMode.PublicationOnly).Value;
        string AndriodPath { get; }
        void InstallApk();
    }
}

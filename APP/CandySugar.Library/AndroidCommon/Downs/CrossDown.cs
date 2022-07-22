using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.AndroidCommon.Downs
{
    public class CrossDown
    {
        private static Lazy<IDownManager> Implementation = new Lazy<IDownManager>(() => {
#if ANDROID
            return new Library.Platforms.Android.CrossDown.DownManager();
#else
            return null;
#endif
        }, LazyThreadSafetyMode.PublicationOnly);
        /// <summary>
        /// The platform-implementation
        /// </summary>
        public static IDownManager Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
                }
                return ret;
            }
        }
    }
}

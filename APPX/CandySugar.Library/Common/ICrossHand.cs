using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library.Common
{
    /// <summary>
    /// 重力感应
    /// </summary>
    public interface ICrossHand
    {
        static ICrossHand Instance => new Lazy<ICrossHand>(() =>
        {
#if ANDROID
                return new Library.Platforms.Android.CrossHand();
#else
            return null;
#endif
        }, LazyThreadSafetyMode.PublicationOnly).Value;

        void RegistEvent();
        void UnRegistEvent();
    }
}

using CandySugar.Library;
using Sdk.Component.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Controls
{
    public class DataCenter
    {

        /// <summary>
        /// Sdk请求方式
        /// </summary>
        /// <returns></returns>
        public static SdkImpl ImplType()
        {
            if (DataBus.QueryModule == 1) return SdkImpl.Multi;
            else if (DataBus.QueryModule == 2) return SdkImpl.Rest;
            else if (DataBus.QueryModule == 3) return SdkImpl.RPC;
            else return SdkImpl.User;
        }
        /// <summary>
        /// 图片方法
        /// </summary>
        /// <returns></returns>
        public static string ImageType()
        {

            if (DataBus.Module == 1) return string.Empty;
            else if (DataBus.Module == 2) return $"rating:safe";
            else if (DataBus.Module == 3) return $"rating:questionable";
            else return $"rating:explicit";
        }
        public static void RegistFunc()
        {
            ImageDep.Funcs = new((key, type) =>
            {
                if (type == 1)
                {

                }
                if (type == 2)
                {
                
                }
                return new byte[0];
            });
        }
    }
}

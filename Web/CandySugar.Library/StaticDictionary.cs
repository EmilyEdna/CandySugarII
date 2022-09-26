using CandySugar.Library.Entity;
using Sdk.Component.Anime.sdk.ViewModel.Enums;
using Sdk.Component.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Library
{
    public class StaticDictionary
    {
        public static UserAttachEntity UserAttachEntity { get; set; }
        public static SdkImpl ImplType()
        {
            if (UserAttachEntity == null) return SdkImpl.Multi;
            if (UserAttachEntity.RequestType == 1) return SdkImpl.Multi;
            else if (UserAttachEntity.RequestType == 2) return SdkImpl.Rest;
            else return SdkImpl.RPC;
        }
        public static int Cache() => UserAttachEntity == null ? 5 : UserAttachEntity.Cache;
        public static AnimeSourceEnum AnimeSource() => UserAttachEntity.AnimeSource == 0 ? AnimeSourceEnum.SBDM : AnimeSourceEnum.YSJDM;
    }
}

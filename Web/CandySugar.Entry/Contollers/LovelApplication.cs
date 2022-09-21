using CandySugar.Library.Entity.Lovel;
using CandySugar.Library.Logic.IService;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;


namespace CandySugar.Entry.Contollers
{
    /// <summary>
    /// 轻小说
    /// </summary>
    [ApiDescriptionSettings("轻小说", Tag = "轻小说", SplitCamelCase = false), NonUnify, Route("/api/lovel")]
    public class LovelApplication : IDynamicApiController
    {
        readonly ILovelService LovelService;
        public LovelApplication(ILovelService LovelService)
        {
            this.LovelService = LovelService;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<LovelInitEntity>> Init() => await LovelService.Init();
    }
}

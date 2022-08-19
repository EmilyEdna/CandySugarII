using CandySugar.Library.Logic.IService;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;

namespace CandySugar.Entry.Contollers
{
    [ApiDescriptionSettings("小说", Tag = "小说", SplitCamelCase = false), NonUnify, Route("/api/novel")]
    public class NovelApplication : IDynamicApiController
    {
        readonly INovelService NovelService;
        public NovelApplication(INovelService NovelService)
        {
            this.NovelService = NovelService;
        }
        [HttpGet]
        public async Task<List<object>> Init() => await NovelService.Init();


    }
}

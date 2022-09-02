using CandySugar.Library.Entity.Novel;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using System.Web;

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
        public async Task<List<NovelInitEntity>> Init() => await NovelService.Init();
        [HttpGet]
        public async Task<List<NovelSearchEntity>> Search(string input) => await NovelService.Search(input);
        [HttpGet]
        public async Task<PageOutDto<List<NovelCategoryEntity>>> Category(string input, int page) => await NovelService.Category(HttpUtility.UrlDecode(input), page);
        [HttpGet]
        public async Task<NovelDetailEntity> Detail(string input, int page) => await NovelService.Detail(HttpUtility.UrlDecode(input), page);
        [HttpGet]
        public async Task<NovelContentEntity> Content(string input) => await NovelService.Content(HttpUtility.UrlDecode(input));
    }
}

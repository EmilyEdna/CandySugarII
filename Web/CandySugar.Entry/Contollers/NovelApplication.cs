using CandySugar.Library.Entity.Novel;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace CandySugar.Entry.Contollers
{
    /// <summary>
    /// 小说
    /// </summary>
    [ApiDescriptionSettings("小说", Tag = "小说", SplitCamelCase = false), NonUnify, Route("/api/novel")]
    public class NovelApplication : IDynamicApiController
    {
        readonly INovelService NovelService;
        public NovelApplication(INovelService NovelService)
        {
            this.NovelService = NovelService;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<List<NovelInitEntity>>> Init() => CandyResult<List<NovelInitEntity>>.Result(await NovelService.Init());
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="input"></param>
        /// <param name="Forced">强制拉取</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<List<NovelSearchEntity>>> Search(string input, bool Forced = false) => CandyResult<List<NovelSearchEntity>>.Result(await NovelService.Search(input, Forced));
        /// <summary>
        /// 分类
        /// </summary>
        /// <param name="input"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<PageOutDto<List<NovelCategoryEntity>>>> Category(string input, int page) => CandyResult<PageOutDto<List<NovelCategoryEntity>>>.Result(await NovelService.Category(HttpUtility.UrlDecode(input), page));
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="input"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<NovelDetailEntity>> Detail(string input, int page) => CandyResult<NovelDetailEntity>.Result(await NovelService.Detail(HttpUtility.UrlDecode(input), page));
        /// <summary>
        /// 内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<NovelContentEntity>> Content(string input) => CandyResult<NovelContentEntity>.Result(await NovelService.Content(HttpUtility.UrlDecode(input)));
    }
}

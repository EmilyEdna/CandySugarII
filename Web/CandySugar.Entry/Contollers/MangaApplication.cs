using CandySugar.Library.Entity.Manga;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Web;

namespace CandySugar.Entry.Contollers
{
    /// <summary>
    /// 漫画
    /// </summary>
    [ApiDescriptionSettings("漫画", Tag = "漫画", SplitCamelCase = false), NonUnify, Route("/api/manga")]
    public class MangaApplication : IDynamicApiController
    {
        readonly IMangaService MangaService;
        public MangaApplication(IMangaService MangaService)
        {
            this.MangaService = MangaService;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<List<MangaInitEntity>>> Init() => CandyResult<List<MangaInitEntity>>.Result(await MangaService.Init());
        /// <summary>
        /// 分类
        /// </summary>
        /// <param name="input"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<PageOutDto<List<MangaGlobalEntity>>>> Category(string input, int page) => CandyResult<PageOutDto<List<MangaGlobalEntity>>>.Result(await MangaService.Category(HttpUtility.UrlDecode(input), page));
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="input"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<PageOutDto<List<MangaGlobalEntity>>>> Search(string input, int page) => CandyResult<PageOutDto<List<MangaGlobalEntity>>>.Result(await MangaService.Search(input, page));
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<List<MangaDetailEntity>>> Detail(string input) => CandyResult<List<MangaDetailEntity>>.Result(await MangaService.Detail(HttpUtility.UrlDecode(input)));
    }
}

using CandySugar.Library.Entity.Anime;
using CandySugar.Library.ViewModel.AniDto;
using CandySugar.Library.ViewModel;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.Logic.Service;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Web;

namespace CandySugar.Entry.Contollers
{
    /// <summary>
    /// 动漫
    /// </summary>
    [ApiDescriptionSettings("动漫", Tag = "动漫", SplitCamelCase = false), NonUnify, Route("/api/anime")]
    public class AnimeApplication : IDynamicApiController
    {
        readonly IAnimeService AnimeService;
        public AnimeApplication(IAnimeService AnimeService)
        {
            this.AnimeService = AnimeService;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<InitDto>> Init() => CandyResult<InitDto>.Result(await AnimeService.Init());
        /// <summary>
        /// 分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CandyResult<PageOutDto<List<AnimeGlobalEntity>>>> Category(CateDto input) => CandyResult<PageOutDto<List<AnimeGlobalEntity>>>.Result(await AnimeService.Category(input));
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="key"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<PageOutDto<List<AnimeGlobalEntity>>>> Search(string key, int page = 1) => CandyResult<PageOutDto<List<AnimeGlobalEntity>>>.Result(await AnimeService.Search(key, page));
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<List<AnimeDetailEntity>>> Detail(string input) => CandyResult<List<AnimeDetailEntity>>.Result(await AnimeService.Detail(HttpUtility.UrlDecode(input)));
        /// <summary>
        /// 观看
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<AnimePlayEntity>> Watch(string input) => CandyResult<AnimePlayEntity>.Result(await AnimeService.Watch(HttpUtility.UrlDecode(input)));
    }
}

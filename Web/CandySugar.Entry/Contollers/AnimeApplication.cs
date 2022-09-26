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
        public async Task<InitDto> Init() => await AnimeService.Init();
        /// <summary>
        /// 分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageOutDto<List<AnimeGlobalEntity>>> Category(CateDto input)=> await AnimeService.Category(input);
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="key"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageOutDto<List<AnimeGlobalEntity>>> Search(string key, int page=1)=>await AnimeService.Search(key, page);
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<AnimeDetailEntity>> Detail(string input)=> await AnimeService.Detail(HttpUtility.UrlDecode(input));
        /// <summary>
        /// 观看
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<AnimePlayEntity> Watch(string input)=> await AnimeService.Watch(HttpUtility.UrlDecode(input));
    }
}

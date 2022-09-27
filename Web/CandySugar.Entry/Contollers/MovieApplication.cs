using CandySugar.Library.Entity.Movie;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace CandySugar.Entry.Contollers
{
    /// <summary>
    /// 电影
    /// </summary>
    [ApiDescriptionSettings("电影", Tag = "电影", SplitCamelCase = false), NonUnify, Route("/api/movie")]
    public class MovieApplication : IDynamicApiController
    {
        readonly IMovieService MovieService;
        public MovieApplication(IMovieService MovieService)
        {
            this.MovieService = MovieService;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<List<MovieInitEntity>>> Init() => CandyResult<List<MovieInitEntity>>.Result(await MovieService.Init());
        /// <summary>
        /// 分类
        /// </summary>
        /// <param name="input"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<PageOutDto<List<MovieGlobalEntity>>>> Category(string input, int page) => CandyResult<PageOutDto<List<MovieGlobalEntity>>>.Result(await MovieService.Category(HttpUtility.UrlDecode(input), page));
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="input"></param>
        /// <param name="searchId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<PageOutDto<List<MovieGlobalEntity>>>> Search(string input, int searchId, int page) => CandyResult<PageOutDto<List<MovieGlobalEntity>>>.Result(await MovieService.Search(input, searchId, page));
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<List<MovieDetailEntity>>> Detail(string input) => CandyResult<List<MovieDetailEntity>>.Result(await MovieService.Detail(HttpUtility.UrlDecode(input)));
        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<MoviePlayEntity>> Play(string input) => CandyResult<MoviePlayEntity>.Result(await MovieService.Play(HttpUtility.UrlDecode(input)));
    }
}

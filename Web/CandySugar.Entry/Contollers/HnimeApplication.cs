using CandySugar.Library.Entity.Hnime;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel.HniDto;
using CandySugar.Library.ViewModel;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Web;

namespace CandySugar.Entry.Contollers
{
    [ApiDescriptionSettings("ACG", Tag = "ACG", SplitCamelCase = false), NonUnify, Route("/api/hnime")]
    public class HnimeApplication : IDynamicApiController
    {
        readonly IHnimeService HnimeService;
        public HnimeApplication(IHnimeService HnimeService)
        {
            this.HnimeService = HnimeService;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<List<HnimeInitEntity>>> Init() => CandyResult<List<HnimeInitEntity>>.Result(await HnimeService.Init());
        /// <summary>
        /// 分类
        /// </summary>
        /// <param name="input"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<PageOutDto<List<HnimeGlobalEntity>>>> Category(string input, int page) => CandyResult<PageOutDto<List<HnimeGlobalEntity>>>.Result(await HnimeService.Category(HttpUtility.UrlDecode(input), page));
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CandyResult<PageOutDto<List<HnimeGlobalEntity>>>> Search(SearchDto input) => CandyResult<PageOutDto<List<HnimeGlobalEntity>>>.Result(await HnimeService.Search(input));
        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="input"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<List<HnimePlayEntity>>> Play(string input, string name) => CandyResult<List<HnimePlayEntity>>.Result(await HnimeService.Play(input,name));
    }
}

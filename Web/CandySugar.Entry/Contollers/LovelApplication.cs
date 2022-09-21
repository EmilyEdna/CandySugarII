using CandySugar.Library.Entity.Lovel;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using System.Web;

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
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="input"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageOutDto<List<LovelSearchEntity>>> Search(string input, int page)=> await LovelService.Search(input,page);
        /// <summary>
        /// 分类
        /// </summary>
        /// <param name="input"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageOutDto<List<LovelCategoryEntity>>> Category(string input, int page) => await LovelService.Category(HttpUtility.UrlDecode(input), page);
    }
}

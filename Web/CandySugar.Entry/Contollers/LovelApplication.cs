using CandySugar.Library.Entity.Lovel;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using Furion;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using XExten.Advance.StaticFramework;

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
        public async Task<PageOutDto<List<LovelSearchEntity>>> Search(string input, int page) => await LovelService.Search(input, page);
        /// <summary>
        /// 分类
        /// </summary>
        /// <param name="input"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageOutDto<List<LovelCategoryEntity>>> Category(string input, int page) => await LovelService.Category(HttpUtility.UrlDecode(input), page);
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<LovelDetailEntity> Detail(string input) => await LovelService.Detail(HttpUtility.UrlDecode(input));
        /// <summary>
        /// 章节
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<LovelViewEntity>> View(string input) => await LovelService.View(HttpUtility.UrlDecode(input));
        /// <summary>
        /// 内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<LovelContentEntity> Content(string input) => await LovelService.Content(HttpUtility.UrlDecode(input));
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Download(string input)
        {
            if (!File.Exists(Path.Combine(App.WebHostEnvironment.WebRootPath, $"{input}.txt")))
            {
                var res = await LovelService.Download(input);
                var route = SyncStatic.CreateFile(Path.Combine(App.WebHostEnvironment.WebRootPath, $"{input}.txt"));
                var file = SyncStatic.WriteFile(res, route);
                return new FileStreamResult(new FileStream(file, FileMode.Open), "application/octet-stream") { FileDownloadName = $"{input}.txt" };
            }
            else
                return new FileStreamResult(new FileStream(Path.Combine(App.WebHostEnvironment.WebRootPath, $"{input}.txt"), FileMode.Open), "application/octet-stream") { FileDownloadName = $"{input}.txt" };
        }
    }
}

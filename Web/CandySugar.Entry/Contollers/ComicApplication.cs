using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel.MicDto;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;

namespace CandySugar.Entry.Contollers
{
    /// <summary>
    /// Comic
    /// </summary>
    [ApiDescriptionSettings("Comic", Tag = "Comic", SplitCamelCase = false), NonUnify, Route("/api/comic")]
    public class ComicApplication : IDynamicApiController
    {
        readonly IComicService ComicService;
        public ComicApplication(IComicService ComicService)
        {
            this.ComicService = ComicService;
        }

        /// <summary>
        /// 浏览
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ComicViewDto> View(ComicDto input) => await ComicService.View(input);
    }
}

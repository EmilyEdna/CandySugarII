using CandySugar.Library.Logic.IService;
using Furion.DependencyInjection;
using Sdk.Component.Novel.sdk;
using Sdk.Component.Novel.sdk.ViewModel;
using Sdk.Component.Plugins;
using Sdk.Component.Novel.sdk.ViewModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdk.Component.Novel.sdk.ViewModel.Response;
using Furion.FriendlyException;

namespace CandySugar.Library.Logic.Service
{
    public class NovelService : DbContext, INovelService, IScoped
    {
        public async Task<List<NovelInitCategoryResult>> Init()
        {
            try
            {
                var data = await NovelFactory.Novel(opt =>
                    {
                        opt.RequestParam = new Input
                        {
                            ImplType = SdkImpl.Multi,
                            CacheSpan = 5,
                            NovelType = NovelEnum.Init,
                        };
                    }).RunsAsync();
                return data.CateInitResults;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
    }
}

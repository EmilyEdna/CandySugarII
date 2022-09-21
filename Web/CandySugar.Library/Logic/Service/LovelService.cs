using CandySugar.Library.Entity.Lovel;
using Sdk.Component.Lovel.sdk.ViewModel;
using CandySugar.Library.Logic.IService;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Sdk.Component.Lovel.sdk;
using Sdk.Component.Lovel.sdk.ViewModel.Enums;
using Sdk.Component.Lovel.sdk.ViewModel.Request;
using XExten.Advance.LinqFramework;
using CandySugar.Library.Entity.Novel;
using CandySugar.Library.ViewModel;
using SqlSugar;
using Polly.Caching;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CandySugar.Library.Logic.Service
{
    public class LovelService : DbContext, ILovelService, IScoped
    {
        public async Task<List<LovelInitEntity>> Init()
        {
            try
            {
                var res = await Scope().Queryable<LovelInitEntity>().ToListAsync();
                if (res.Count > 0)
                    return res;
                var data = await LovelFactory.Lovel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        ImplType = StaticDictionary.ImplType(),
                        CacheSpan = StaticDictionary.Cache(),
                        LovelType = LovelEnum.Init
                    };
                }).RunsAsync();
                var entity = data.InitResults.Select(t => new LovelInitEntity
                {
                    CategoryName = t.Name,
                    CollectRoute = t.Route
                }).ToList();
                await Scope().Insertable(entity).CallEntityMethod(t => t.Create(true)).ExecuteCommandAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<PageOutDto<List<LovelSearchEntity>>> Search(string input, int page)
        {
            try
            {
                RefAsync<int> total = 0;
                var model = await Scope().Queryable<LovelSearchEntity>().Where(t => t.BookName.Contains(input)).ToPageListAsync(page, 20, total);
                if (model.Count > 0)
                    return new PageOutDto<List<LovelSearchEntity>>
                    {
                        Total = total,
                        Data = model
                    };

                var data = await LovelFactory.Lovel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        LovelType = LovelEnum.Search,
                        ImplType = StaticDictionary.ImplType(),
                        CacheSpan= StaticDictionary.Cache(),
                        Search = new LovelSearch
                        {
                            KeyWord = input,
                            Page = page,
                            SearchType = LovelSearchEnum.ArticleName
                        }
                    };
                }).RunsAsync();
                if (data.SearchResult.ElementResults.Count <= 0)
                {
                    return new PageOutDto<List<LovelSearchEntity>>
                    {
                        Total = total,
                        Data = model
                    };
                }
                var entity = data.SearchResult.ElementResults.ToMapest<List<LovelSearchEntity>>();
                var bookName = Scope().Queryable<LovelSearchEntity>().Where(t => t.BookName.Contains(input)).Select(t => t.BookName).ToList();
                entity = entity.Where(t => !bookName.Contains(t.BookName)).ToList();
                if (entity.Count != 0)
                    await Scope().Insertable(entity).CallEntityMethod(t => t.Create(true)).ExecuteCommandAsync();
                return new PageOutDto<List<LovelSearchEntity>>
                {
                    Total = bookName.Count + entity.Count,
                    Data = entity
                };
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        //public async Task<bool> Category()
        //{

        //}
        //public async Task<bool> Detail()
        //{

        //}
        //public async Task<bool> View()
        //{

        //}
        //public async Task<bool> Content()
        //{

        //}
        //public async Task<bool> Download()
        //{

        //}
    }
}

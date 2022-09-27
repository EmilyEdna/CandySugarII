using CandySugar.Library.Entity.Hnime;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using CandySugar.Library.ViewModel.HniDto;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Sdk.Component.Hnime.sdk;
using Sdk.Component.Hnime.sdk.ViewModel;
using Sdk.Component.Hnime.sdk.ViewModel.Enums;
using Sdk.Component.Hnime.sdk.ViewModel.Request;
using SqlSugar;
using XExten.Advance.LinqFramework;

namespace CandySugar.Library.Logic.Service
{
    public class HnimeService : DbContext, IHnimeService, IScoped
    {
        public async Task<List<HnimeInitEntity>> Init()
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    var res = await Scope().Queryable<HnimeInitEntity>().ToListAsync();
                    if (res.Count > 0) return res;
                    return null;
                });
                if (res != null) return res;
                var data = await HnimeFactory.Hnime(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        HnimeType = HnimeEnum.Init,
                        ImplType = StaticDictionary.ImplType(),
                        CacheSpan = StaticDictionary.Cache(),
                        Init = new HnimeInit { InitLabel = false }
                    };
                }).RunsAsync();
                var entity = data.InitResults.ToMapest<List<HnimeInitEntity>>();
                var keys = await Scope().Queryable<HnimeInitEntity>().Select(t => t.Route).ToListAsync();
                var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.Route)).ToList() : entity;
                if (wait.Count > 0) await Scope().Insertable(wait).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<PageOutDto<List<HnimeGlobalEntity>>> Category(string input, int page)
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    RefAsync<int> total = 0;
                    var res = await Scope().Queryable<HnimeGlobalEntity>().Where(t => t.IsSearch == false).Where(t => t.InfoRoute == input).ToPageListAsync(page, 20, total);
                    if (res.Count > 0) return new PageOutDto<List<HnimeGlobalEntity>>
                    {
                        Data = res,
                        Total = total
                    };
                    return null;
                });
                if (res != null) return res;
                var data = await HnimeFactory.Hnime(opt =>
                 {
                     opt.RequestParam = new Input
                     {
                         HnimeType = HnimeEnum.Category,
                         ImplType = StaticDictionary.ImplType(),
                         CacheSpan = StaticDictionary.Cache(),
                         Category = new HnimeCategory
                         {
                             Route = input
                         }
                     };
                 }).RunsAsync();
                var entity = data.SearchResult.ElementResult.ToMapest<List<HnimeGlobalEntity>>();
                var keys = await Scope().Queryable<HnimeGlobalEntity>().Where(t => t.IsSearch == false).Where(t => t.InfoRoute == input).Select(t => t.Watch).ToListAsync();
                var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.Watch)).ToList() : entity;
                if (wait.Count > 0)
                {
                    wait.ForEach(t =>
                    {
                        t.IsSearch = false;
                        t.InfoRoute = input;
                    });
                    await Scope().Insertable(wait).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
                }
                return new PageOutDto<List<HnimeGlobalEntity>>
                {
                    Data = entity,
                    Total = data.SearchResult.Total
                };
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<PageOutDto<List<HnimeGlobalEntity>>> Search(SearchDto input)
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    RefAsync<int> total = 0;
                    var res = await Scope().Queryable<HnimeGlobalEntity>().Where(t => t.IsSearch == true).Where(t => t.Title.Contains(input.Keyword)).ToPageListAsync(input.Page, 20, total);
                    if (res.Count > 0) return new PageOutDto<List<HnimeGlobalEntity>>
                    {
                        Data = res,
                        Total = total
                    };
                    return null;
                });
                if (res != null) return res;
                var data = await HnimeFactory.Hnime(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        HnimeType = HnimeEnum.Search,
                        ImplType = StaticDictionary.ImplType(),
                        CacheSpan = StaticDictionary.Cache(),
                        Search = new HnimeSearch
                        {
                            Brands = input.Brands,
                            HnimeType = input.HnimeType,
                            KeyWord = input.Keyword,
                            Page = input.Page,
                            Tags = input.Tags
                        }
                    };
                }).RunsAsync();
                var entity = data.SearchResult.ElementResult.ToMapest<List<HnimeGlobalEntity>>();
                var keys = await Scope().Queryable<HnimeGlobalEntity>().Where(t => t.IsSearch == true)
                    .Where(t => t.Title.Contains(input.Keyword)).Select(t => t.Watch).ToListAsync();
                var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.Watch)).ToList() : entity;
                if (wait.Count > 0)
                {
                    wait.ForEach(t =>
                    {
                        t.IsSearch = true;
                    });
                    await Scope().Insertable(wait).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
                }
                return new PageOutDto<List<HnimeGlobalEntity>>
                {
                    Data = entity,
                    Total = data.SearchResult.Total
                };
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<List<HnimePlayEntity>> Play(string input, string name)
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    var res = await Scope().Queryable<HnimePlayEntity>().Where(t => t.Title.Equals(name)).ToListAsync();
                    if (res.Count > 0) return res;
                    return null;
                });
                if (res != null) return res;
                var data = await HnimeFactory.Hnime(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        HnimeType = HnimeEnum.Watch,
                        ImplType = StaticDictionary.ImplType(),
                        CacheSpan = StaticDictionary.Cache(),
                        Play = new HnimePlay
                        {
                            Route = input
                        }
                    };
                }).RunsAsync();
                var  entity= data.PlayResults.ToMapest<List<HnimePlayEntity>>();
                var keys = await Scope().Queryable<HnimePlayEntity>().Select(t=>t.Title).ToListAsync();
                var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.Title)).ToList() : entity;
                if (wait.Count > 0) 
                    await Scope().Insertable(wait).CallEntityMethod(t=>t.Create(true)).ExecuteReturnEntityAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
    }
}

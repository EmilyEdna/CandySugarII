using CandySugar.Library.Entity.Manga;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sdk.Component.Manga.sdk;
using Sdk.Component.Manga.sdk.ViewModel;
using Sdk.Component.Manga.sdk.ViewModel.Enums;
using Sdk.Component.Manga.sdk.ViewModel.Request;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Library.Logic.Service
{
    public class MangaService : DbContext, IMangaService, IScoped
    {
        public async Task<List<MangaInitEntity>> Init()
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    var res = await Scope().Queryable<MangaInitEntity>().ToListAsync();
                    if (res.Count > 0) return res;
                    return null;
                });
                var data = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        MangaType = MangaEnum.Init,
                        CacheSpan = StaticDictionary.Cache(),
                        ImplType = StaticDictionary.ImplType()
                    };
                }).RunsAsync();
                var entity = data.CateInitResults.ToMapest<List<MangaInitEntity>>();
                var keys = await Scope().Queryable<MangaInitEntity>().Where(t => entity.Select(x => x.Route).Contains(t.Route)).Select(t => t.Route).ToListAsync();
                var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.Route)).ToList() : entity;
                if (wait.Count > 0)
                    await Scope().Insertable(wait).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<PageOutDto<List<MangaGlobalEntity>>> Category(string input, int page)
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    RefAsync<int> total = 0;
                    var res = await Scope().Queryable<MangaGlobalEntity>().Where(t => t.CategoryRoute == input).Where(t => t.IsSearch == false).ToPageListAsync(page, 20, total);
                    if (res.Count > 0) return new PageOutDto<List<MangaGlobalEntity>>
                    {
                        Data = res,
                        Total = total
                    };
                    return null;
                });
                if (res != null) return res;
                var data = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        MangaType = MangaEnum.Category,
                        CacheSpan = StaticDictionary.Cache(),
                        ImplType = StaticDictionary.ImplType(),
                        Category = new MangaCategory
                        {
                            Page = page,
                            Route = input
                        }
                    };
                }).RunsAsync();
                var entity = data.CategoryResult.ElementResults.ToMapest<List<MangaGlobalEntity>>();
                //TODO:去重
                var keys = await Scope().Queryable<MangaGlobalEntity>().Where(t => t.CategoryRoute == input).Select(t => t.Route).ToListAsync();
                var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.Route)).ToList() : entity;
                if (wait.Count > 0)
                {
                    wait.ForEach(t =>
                    {
                        t.CategoryRoute = input;
                        t.IsSearch = false;
                    });
                    await Scope().Insertable(wait).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
                }
                return new PageOutDto<List<MangaGlobalEntity>>
                {
                    Data = entity,
                    Total = data.CategoryResult.Total
                };
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<PageOutDto<List<MangaGlobalEntity>>> Search(string input, int page)
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    RefAsync<int> total = 0;
                    var res = await Scope().Queryable<MangaGlobalEntity>().Where(t => t.Name.Contains(input)).Where(t => t.IsSearch == true).ToPageListAsync(page, 20, total);
                    if (res.Count > 0) return new PageOutDto<List<MangaGlobalEntity>>
                    {
                        Data = res,
                        Total = total
                    };
                    return null;
                });
                if (res != null) return res;
                var data = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        MangaType = MangaEnum.Search,
                        CacheSpan = StaticDictionary.Cache(),
                        ImplType = StaticDictionary.ImplType(),
                        Search = new MangaSearch
                        {
                            Page = page,
                            KeyWord = input
                        }
                    };
                }).RunsAsync();
                var entity = data.SearchResult.ElementResults.ToMapest<List<MangaGlobalEntity>>();
                //TODO:去重
                var keys = await Scope().Queryable<MangaGlobalEntity>().Where(t => t.Name.Contains(input)).Where(t => t.IsSearch == true).Select(t => t.Route).ToListAsync();
                var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.Route)).ToList() : entity;
                if (wait.Count > 0)
                {
                    wait.ForEach(t => t.IsSearch = true);
                    await Scope().Insertable(wait).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
                }
                return new PageOutDto<List<MangaGlobalEntity>>
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
        public async Task<List<MangaDetailEntity>> Detail(string input) 
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    var res = await Scope().Queryable<MangaDetailEntity>().Where(t => t.InfoRoute == input).ToListAsync();
                    if (res.Count > 0) return res;
                    return null;
                });
                if (res != null) return res;
                var data = await MangaFactory.Manga(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        MangaType = MangaEnum.Detail,
                        CacheSpan = StaticDictionary.Cache(),
                        ImplType = StaticDictionary.ImplType(),
                        Detail = new  MangaDetail
                        {
                            Route = input
                        }
                    };
                }).RunsAsync();
                var entity = data.ChapterResults.ToMapest<List<MangaDetailEntity>>();
                //TODO:去重
                var keys = await Scope().Queryable<MangaDetailEntity>().Where(t => t.InfoRoute == input).Select(t=>t.Route).ToListAsync();
                var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.Route)).ToList() : entity;
                if (wait.Count > 0) 
                {
                    wait.ForEach(t => t.InfoRoute = input);
                    await Scope().Insertable(wait).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex);
            }
        }
    }
}

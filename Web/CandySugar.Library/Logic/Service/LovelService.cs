using CandySugar.Library.Entity.Lovel;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using ImTools;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sdk.Component.Lovel.sdk;
using Sdk.Component.Lovel.sdk.ViewModel;
using Sdk.Component.Lovel.sdk.ViewModel.Enums;
using Sdk.Component.Lovel.sdk.ViewModel.Request;
using SqlSugar;
using XExten.Advance.LinqFramework;

namespace CandySugar.Library.Logic.Service
{
    public class LovelService : DbContext, ILovelService, IScoped
    {
        public async Task<List<LovelInitEntity>> Init()
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    var res = await Scope().Queryable<LovelInitEntity>().ToListAsync();
                    if (res.Count > 0)
                        return res;
                    return null;
                });
                if (res != null) return res;

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
                //TODO:去重
                var keys = await Scope().Queryable<LovelInitEntity>().Where(t => entity.Select(x => x.CollectRoute).Contains(t.CollectRoute)).Select(t => t.CollectRoute).ToListAsync();
                var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.CollectRoute)).ToList() : entity;
                if (wait.Count > 0)
                    await Scope().Insertable(wait).CallEntityMethod(t => t.Create(true)).ExecuteCommandAsync();
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
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    RefAsync<int> total = 0;
                    var model = await Scope().Queryable<LovelSearchEntity>().Where(t => t.BookName.Contains(input)).ToPageListAsync(page, 20, total);
                    if (model.Count > 0)
                        return new PageOutDto<List<LovelSearchEntity>>
                        {
                            Total = total,
                            Data = model
                        };
                    return null;
                });
                if (res != null) return res;

                var data = await LovelFactory.Lovel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        LovelType = LovelEnum.Search,
                        ImplType = StaticDictionary.ImplType(),
                        CacheSpan = StaticDictionary.Cache(),
                        Search = new LovelSearch
                        {
                            KeyWord = input,
                            Page = page,
                            SearchType = LovelSearchEnum.ArticleName
                        }
                    };
                }).RunsAsync();
                var entity = data.SearchResult.ElementResults.ToMapest<List<LovelSearchEntity>>();
                //TODO:去重
                var keys = Scope().Queryable<LovelSearchEntity>().Where(t => t.BookName.Contains(input)).Select(t => t.BookName).ToList();
                var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.BookName)).ToList() : entity;
                if (wait.Count > 0)
                    await Scope().Insertable(wait).CallEntityMethod(t => t.Create(true)).ExecuteCommandAsync();
                return new PageOutDto<List<LovelSearchEntity>>
                {
                    Total = data.SearchResult.Total,
                    Data = entity
                };
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<PageOutDto<List<LovelCategoryEntity>>> Category(string input, int page)
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    RefAsync<int> total = 0;
                    var model = await Scope().Queryable<LovelCategoryEntity>().Where(t => t.CategoryRoute == input).ToPageListAsync(page, 20, total);
                    if (model.Count > 0)
                        return new PageOutDto<List<LovelCategoryEntity>>
                        {
                            Total = total,
                            Data = model
                        };
                    return null;
                });
                if (res != null) return res;
                var data = await LovelFactory.Lovel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        LovelType = LovelEnum.Category,
                        ImplType = StaticDictionary.ImplType(),
                        CacheSpan = StaticDictionary.Cache(),
                        Category = new LovelCategory
                        {
                            Route = input,
                            Page = page
                        }
                    };
                }).RunsAsync();

                var entity = data.CategoryResult.ElementResults.ToMapest<List<LovelCategoryEntity>>();
                //TODO:去重
                var keys = Scope().Queryable<LovelCategoryEntity>().Where(t => t.CategoryRoute == input).Select(t => t.BookName).ToList();
                var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.BookName)).ToList() : entity;
                if (wait.Count > 0)
                {
                    wait.ForEach(item =>
                    {
                        item.CategoryRoute = input;
                    });
                    await Scope().Insertable(wait).CallEntityMethod(t => t.Create(true)).ExecuteCommandAsync();
                }
                return new PageOutDto<List<LovelCategoryEntity>>
                {
                    Total = data.CategoryResult.Total,
                    Data = entity
                };
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<LovelDetailEntity> Detail(string input)
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    var res = await Scope().Queryable<LovelDetailEntity>().Where(t => t.DetailRoute == input).FirstAsync();
                    if (res != null) return res;
                    return null;
                });
                if (res != null) return res;
                var data = await LovelFactory.Lovel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        ImplType = StaticDictionary.ImplType(),
                        CacheSpan = StaticDictionary.Cache(),
                        LovelType = LovelEnum.Detail,
                        Detail = new LovelDetail
                        {
                            Route = input
                        }
                    };
                }).RunsAsync();
                var entity = data.DetailResult.ToMapest<LovelDetailEntity>();
                entity.DetailRoute = input;
                //TODO:去重
                var keys = await Scope().Queryable<LovelDetailEntity>().FirstAsync(t => t.Route == entity.Route);
                if (keys == null)
                    return await Scope().Insertable(entity).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
                else return entity;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<List<LovelViewEntity>> View(string input)
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    var res = await Scope().Queryable<LovelViewEntity>().Where(t => t.ViewRoute == input).ToListAsync();
                    if (res.Count > 0) return res;
                    return null;
                });
                if (res != null) return res;
                var data = await LovelFactory.Lovel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        LovelType = LovelEnum.View,
                        ImplType = StaticDictionary.ImplType(),
                        CacheSpan = StaticDictionary.Cache(),
                        View = new LovelView
                        {
                            Route = input
                        }
                    };
                }).RunsAsync();
                var entity = data.ViewResult.ToMapest<List<LovelViewEntity>>();
                //TODO:去重
                var keys = Scope().Queryable<LovelViewEntity>().Select(t => t.BookName).ToList();
                var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.BookName)).ToList() : entity;
                if (wait.Count > 0)
                {
                    wait.ForEach(item =>
                    {
                        item.ViewRoute = input;
                    });
                    await Scope().Insertable(wait).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<LovelContentEntity> Content(string input)
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    var res = await Scope().Queryable<LovelContentEntity>().Where(t => t.ChapterRoute == input).FirstAsync();
                    if (res != null) return res;
                    return null;
                });
                if (res != null) return res;
            
                var data = await LovelFactory.Lovel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        LovelType = LovelEnum.Content,
                        ImplType = StaticDictionary.ImplType(),
                        CacheSpan = StaticDictionary.Cache(),
                        Content = new LovelContent
                        {
                            ChapterRoute = input
                        }
                    };
                }).RunsAsync();
                var entity = data.ContentResult.ToMapest<LovelContentEntity>();
                entity.ChapterRoute = input;
                entity.Picture = data.ContentResult.Image != null && data.ContentResult.Image.Count > 0 ? string.Join(",", data.ContentResult.Image) : null;
                var keys = Scope().Queryable<LovelContentEntity>().FirstAsync(t => t.ChapterRoute == input);
                if (keys == null)
                    return await Scope().Insertable(entity).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
                else return entity;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<byte[]> Download(string input)
        {
            try
            {
                var view = await Scope().Queryable<LovelViewEntity>().FirstAsync(t => t.BookName == input && t.IsDown == true);
                var data = await LovelFactory.Lovel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        LovelType = LovelEnum.Download,
                        ImplType = StaticDictionary.ImplType(),
                        Down = new LovelDown
                        {
                            UId = view.ChapterRoute.AsInt(),
                            BookName = view.BookName
                        }
                    };
                }).RunsAsync();
                return data.DownResult.Bytes;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
    }
}

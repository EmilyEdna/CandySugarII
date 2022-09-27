using CandySugar.Library.Entity.Novel;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using NStandard;
using Sdk.Component.Novel.sdk;
using Sdk.Component.Novel.sdk.ViewModel;
using Sdk.Component.Novel.sdk.ViewModel.Enums;
using Sdk.Component.Novel.sdk.ViewModel.Request;
using SqlSugar;
using XExten.Advance.LinqFramework;

namespace CandySugar.Library.Logic.Service
{
    public class NovelService : DbContext, INovelService, IScoped
    {
        public async Task<List<NovelInitEntity>> Init()
        {
            try
            {
                var res = await Force(async obj =>
                  {
                      if (obj) return null;
                      var res = await Scope().Queryable<NovelInitEntity>().ToListAsync();
                      if (res.Count > 0)
                          return res;
                      return null;
                  });
                if (res != null) return res;
                var data = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        ImplType = StaticDictionary.ImplType(),
                        CacheSpan = StaticDictionary.Cache(),
                        NovelType = NovelEnum.Init,
                    };
                }).RunsAsync();
                var entity = data.CateInitResults.ToMapest<List<NovelInitEntity>>();
                //TODO:去重
                var keys = await Scope().Queryable<NovelInitEntity>().ToListAsync();
                if (keys.Count == 0)
                    await Scope().Insertable(entity).CallEntityMethod(t => t.Create(true)).ExecuteCommandAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<List<NovelSearchEntity>> Search(string input)
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    var res = await Scope().Queryable<NovelSearchEntity>().Where(t => t.BookName.Contains(input) || t.Author.Contains(input)).ToListAsync();
                    if (res.Count > 0)
                        return res;
                    return null;
                });
                if (res != null) return res;
                var data = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        ImplType = StaticDictionary.ImplType(),
                        NovelType = NovelEnum.Search,
                        CacheSpan = StaticDictionary.Cache(),
                        Search = new NovelSearch
                        {
                            KeyWord = input
                        }
                    };
                }).RunsAsync();
                var entity = data.SearchResults.ToMapest<List<NovelSearchEntity>>();
                //TODO:去重
                var keys = await Scope().Queryable<NovelSearchEntity>().Select(t => t.BookName).ToListAsync();
                var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.BookName)).ToList() : entity;
                if (wait.Count > 0)
                    await Scope().Insertable(wait).CallEntityMethod(t => t.Create(true)).ExecuteCommandAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<PageOutDto<List<NovelCategoryEntity>>> Category(string input, string type, int page)
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    RefAsync<int> total = 0;
                    var model = await Scope().Queryable<NovelCategoryEntity>().Where(t => t.CategoryType == type).ToPageListAsync(page, 20, total);
                    if (model.Count > 0)
                        return new PageOutDto<List<NovelCategoryEntity>>
                        {
                            Total = total,
                            Data = model
                        };
                    else return null;
                });
                if (res != null) return res;

                var data = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        ImplType = StaticDictionary.ImplType(),
                        NovelType = NovelEnum.Category,
                        CacheSpan = StaticDictionary.Cache(),
                        Category = new NovelCategory
                        {
                            Page = page,
                            CategoryRoute = input
                        }
                    };
                }).RunsAsync();

                var entity = data.CategoryResult.ElementResults.ToMapest<List<NovelCategoryEntity>>();
                //TODO:去重
                var keys = await Scope().Queryable<NovelCategoryEntity>().ToListAsync();
                var wait = entity.Where(t => !keys.Select(x => x.Author).Contains(t.Author)).Where(t => !keys.Select(x => x.BookName).Contains(t.BookName)).ToList();
                if (wait.Count > 0)
                {
                    wait.ForEach(t =>
                    {
                        t.CategoryType = type;
                    });
                    await Scope().Insertable(wait).CallEntityMethod(t => t.Create(true)).ExecuteCommandAsync();
                }
                return new PageOutDto<List<NovelCategoryEntity>>
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
        public async Task<NovelDetailEntity> Detail(string input, int page)
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    var key = await Scope().Queryable<NovelDetailEntity>().Where(t => t.InfoRoute == input).FirstAsync();
                    if (key != null)
                    {
                        key.Chapter = await Scope().Queryable<NovelChapterEntity>().Where(t => t.KeyId == key.Id).ToListAsync();
                        return key;
                    }
                    return null;
                });
                if (res != null) return res;

                var data = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        ImplType = StaticDictionary.ImplType(),
                        NovelType = NovelEnum.Detail,
                        CacheSpan = StaticDictionary.Cache(),
                        Detail = new NovelDetail
                        {
                            Page = page,
                            DetailRoute = input
                        }
                    };
                }).RunsAsync();


                var detail = data.DetailResult.ToMapest<NovelDetailEntity>();
                var chapter = data.DetailResult.ElementResults.ToMapest<List<NovelChapterEntity>>();
                //TODO:去重
                var keys = await Scope().Queryable<NovelDetailEntity>().Where(t => t.InfoRoute == input).FirstAsync();
                if (keys == null)
                {
                    detail.InfoRoute = input;
                    var entity = await Scope().Insertable(detail).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
                    var key = await Scope().Queryable<NovelChapterEntity>().Where(t => t.KeyId == entity.Id).ToListAsync();
                    if (key.Count > 0)
                    {
                        var wait = chapter.Where(t => !key.Select(x => x.ChapterRoute).Contains(t.ChapterRoute)).ToList();
                        await Scope().Insertable(wait).CallEntityMethod(t => t.Create(entity.Id)).ExecuteReturnEntityAsync();
                    }
                    else
                        await Scope().Insertable(chapter).CallEntityMethod(t => t.Create(entity.Id)).ExecuteReturnEntityAsync();
                }
                detail.Chapter = chapter;
                return detail;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<NovelContentEntity> Content(string input)
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    var res = await Scope().Queryable<NovelContentEntity>()
                    .Where(t => t.Route.Contains(input) || t.NextChapter.Contains(input) || t.NextPage.Contains(input) || t.PreviousChapter.Contains(input) || t.PreviousPage.Contains(input))
                    .FirstAsync();
                    if (res != null) return res;
                    return null;
                });
                if (res != null) return res;

                var data = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        ImplType = StaticDictionary.ImplType(),
                        NovelType = NovelEnum.View,
                        CacheSpan = StaticDictionary.Cache(),
                        View = new NovelView
                        {
                            Route = input
                        }
                    };
                }).RunsAsync();
                var model = data.ContentResult.ToMapest<NovelContentEntity>();
                model.Route = input;
                //TODO:去重
                var keys = await Scope().Queryable<NovelContentEntity>().Where(t => t.Route == input).FirstAsync();
                if (keys == null)
                    await Scope().Insertable(model).CallEntityMethod(t => t.Create(true)).ExecuteCommandAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
    }
}

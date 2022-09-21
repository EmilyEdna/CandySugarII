using CandySugar.Library.Entity.Novel;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NStandard;
using Polly.Caching;
using Sdk.Component.Novel.sdk;
using Sdk.Component.Novel.sdk.ViewModel;
using Sdk.Component.Novel.sdk.ViewModel.Enums;
using Sdk.Component.Novel.sdk.ViewModel.Request;
using Sdk.Component.Plugins;
using SqlSugar;
using System.Linq;
using XExten.Advance.LinqFramework;

namespace CandySugar.Library.Logic.Service
{
    public class NovelService : DbContext, INovelService, IScoped
    {
        public async Task<List<NovelInitEntity>> Init()
        {
            try
            {
                var res = await Scope().Queryable<NovelInitEntity>().ToListAsync();
                if (res.Count > 0)
                    return res;
                var data = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        ImplType = StaticDictionary.ImplType(),
                        NovelType = NovelEnum.Init,
                    };
                }).RunsAsync();
                var entity = data.CateInitResults.ToMapest<List<NovelInitEntity>>();
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
                var key = await Scope().Queryable<NovelSeachKeyEntity>().Where(t => t.Key == input).FirstAsync();
                if (key != null)
                {
                    return await Scope().Queryable<NovelSearchEntity>().Where(t => t.KeyId == key.Id).ToListAsync();
                }
                var data = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        ImplType = StaticDictionary.ImplType(),
                        NovelType = NovelEnum.Search,
                        Search = new NovelSearch
                        {
                            KeyWord = input
                        }
                    };
                }).RunsAsync();
                var entity = data.SearchResults.ToMapest<List<NovelSearchEntity>>();
                var res = await Scope().Insertable(new NovelSeachKeyEntity { Key = input }).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
                await Scope().Insertable(entity).CallEntityMethod(t => t.SetKeyCreate(res.Id)).ExecuteCommandAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<PageOutDto<List<NovelCategoryEntity>>> Category(string input, int page)
        {
            try
            {
                var key = await Scope().Queryable<NovelCategoryKeyEntity>().Where(t => t.Key == input && t.Current >= page).FirstAsync();
                if (key != null)
                {
                    RefAsync<int> total = 0;
                    var model = await Scope().Queryable<NovelCategoryEntity>().Where(t => t.KeyId == key.Id).ToPageListAsync(page, 20, total);
                    return new PageOutDto<List<NovelCategoryEntity>>
                    {
                        Total = total,
                        Data = model
                    };
                }
                var data = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        ImplType = StaticDictionary.ImplType(),
                        NovelType = NovelEnum.Category,
                        Category = new NovelCategory
                        {
                            Page = page,
                            CategoryRoute = input
                        }
                    };
                }).RunsAsync();
                NovelCategoryKeyEntity res = await Scope().Queryable<NovelCategoryKeyEntity>().Where(t => t.Key == input).FirstAsync();
                if (data.CategoryResult != null && page <= data.CategoryResult.Total)
                {
                    if (res != null)
                        await Scope().Updateable<NovelCategoryKeyEntity>().SetColumns(t => t.Current == page).Where(t => t.Id == res.Id).ExecuteCommandAsync();
                    else
                        res = await Scope().Insertable(new NovelCategoryKeyEntity { Key = input, Total = data.CategoryResult.Total, Current = page }).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
                    //去重
                    var entity = data.CategoryResult.ElementResults.ToMapest<List<NovelCategoryEntity>>();
                    var models = Scope().Queryable<NovelCategoryEntity>().ToList();
                    entity = entity.Where(t => !models.Select(x => x.BookName).Contains(t.BookName)).Where(t => !models.Select(x => x.Author).Contains(t.Author)).ToList();
                    await Scope().Insertable(entity).CallEntityMethod(t => t.SetKeyCreate(res.Id)).ExecuteCommandAsync();
                    if (models.Count != 0) models.AddRange(entity);
                    return new PageOutDto<List<NovelCategoryEntity>>
                    {
                        Total = models.Count == 0 ? entity.Count : models.Count,
                        Data = models.Count == 0 ? entity : models.Skip((page - 1) * 20).Take(20).ToList()
                    };
                }
                else throw new Exception("超出目录范围");
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
                var key = await Scope().Queryable<NovelDetailKeyEntity>().Where(t => t.Key == input && t.Current >= page).FirstAsync();
                if (key != null)
                {
                    var model = await Scope().Queryable<NovelDetailEntity>().Where(t => t.KeyId == key.Id).Includes(t => t.Chapter).FirstAsync();
                    return model;
                }

                var data = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        ImplType = StaticDictionary.ImplType(),
                        NovelType = NovelEnum.Detail,
                        Detail = new NovelDetail
                        {
                            Page = page,
                            DetailRoute = input
                        }
                    };
                }).RunsAsync();

                NovelDetailKeyEntity res = await Scope().Queryable<NovelDetailKeyEntity>().Where(t => t.Key == input).FirstAsync();
                if (res != null)
                    await Scope().Updateable<NovelDetailKeyEntity>().SetColumns(t => t.Current == page).Where(t => t.Id == res.Id).ExecuteCommandAsync();
                else
                    res = await Scope().Insertable(new NovelDetailKeyEntity { Key = input, Total = data.DetailResult.Total, Current = page }).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();

                var detail = data.DetailResult.ToMapest<NovelDetailEntity>();
                var chapter = data.DetailResult.ElementResults.ToMapest<List<NovelChapterEntity>>();
                var temp = await Scope().Queryable<NovelDetailEntity>().Where(t => t.BookName == detail.BookName && t.Author == detail.Author).FirstAsync();
                if (temp == null)
                    temp = await Scope().Insertable(detail).CallEntityMethod(t => t.SetKeyCreate(res.Id)).ExecuteReturnEntityAsync();
                else
                    await Scope().Updateable<NovelDetailEntity>().SetColumns(t => t.Total == detail.Total)
                        .SetColumns(t => t.LastUpdateTime == detail.LastUpdateTime)
                        .Where(t => t.Id == temp.Id).ExecuteCommandAsync();

                await Scope().Insertable(chapter).CallEntityMethod(t => t.SetNavCreate(temp.Id)).ExecuteCommandAsync();
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
                var res = await Scope().Queryable<NovelContentEntity>()
                      .Where(t => t.Route.Contains(input) || t.NextChapter.Contains(input) || t.NextPage.Contains(input) || t.PreviousChapter.Contains(input) || t.PreviousPage.Contains(input))
                      .FirstAsync();
                if (res != null) return res;
                var data = await NovelFactory.Novel(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        ImplType = StaticDictionary.ImplType(),
                        NovelType = NovelEnum.View,
                        View = new NovelView
                        {
                            Route = input
                        }
                    };
                }).RunsAsync();
                var model = data.ContentResult.ToMapest<NovelContentEntity>();
                model.Route = input;
                await Scope().Insertable(model).CallEntityMethod(t=>t.Create(true)).ExecuteCommandAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
    }
}

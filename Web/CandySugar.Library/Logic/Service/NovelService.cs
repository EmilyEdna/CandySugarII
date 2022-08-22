using CandySugar.Library.Entity.Novel;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using Furion.DependencyInjection;
using Furion.FriendlyException;
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
                        ImplType = SdkImpl.Multi,
                        NovelType = NovelEnum.Init,
                    };
                }).RunsAsync();
                var entity = data.CateInitResults.ToMapest<List<NovelInitEntity>>();
                await Scope().Insertable(entity).CallEntityMethod(t => t.Create()).ExecuteCommandAsync();
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
                        ImplType = SdkImpl.Multi,
                        NovelType = NovelEnum.Search,
                        Search = new NovelSearch
                        {
                            KeyWord = input
                        }
                    };
                }).RunsAsync();
                var entity = data.SearchResults.ToMapest<List<NovelSearchEntity>>();
                var res = await Scope().Insertable(new NovelSeachKeyEntity { Key = input }).CallEntityMethod(t => t.Create()).ExecuteReturnEntityAsync();
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
                        ImplType = SdkImpl.Multi,
                        NovelType = NovelEnum.Category,
                        Category = new NovelCategory
                        {
                            Page = page,
                            CategoryRoute = input
                        }
                    };
                }).RunsAsync();
                NovelCategoryKeyEntity res = await Scope().Queryable<NovelCategoryKeyEntity>().Where(t => t.Key == input).FirstAsync();
                if (data.CategoryResult != null&& page<=data.CategoryResult.Total)
                {
                    if (res != null)
                        await Scope().Updateable<NovelCategoryKeyEntity>().SetColumns(t => t.Current == page).Where(t => t.Id == res.Id).ExecuteCommandAsync();
                    else
                        res = await Scope().Insertable(new NovelCategoryKeyEntity { Key = input, Total = data.CategoryResult.Total, Current = page }).CallEntityMethod(t => t.Create()).ExecuteReturnEntityAsync();
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
    }
}

using CandySugar.Library.Entity.Movie;
using CandySugar.Library.Logic.IService;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Sdk.Component.Movie.sdk.ViewModel;
using Sdk.Component.Movie.sdk;
using Sdk.Component.Movie.sdk.ViewModel.Enums;
using Sdk.Component.Movie.sdk.ViewModel.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;
using CandySugar.Library.ViewModel;
using SqlSugar;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CandySugar.Library.Logic.Service
{
    public class MovieService : DbContext, IMovieService, IScoped
    {
        public async Task<List<MovieInitEntity>> Init()
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    var res = await Scope().Queryable<MovieInitEntity>().ToListAsync();
                    if (res.Count > 0) return res;
                    return null;
                });
                if (res != null) return res;
                var data = await MovieFactory.Movie(opt =>
                 {
                     opt.RequestParam = new Input
                     {
                         MovieType = MovieEnum.Init,
                         ImplType = StaticDictionary.ImplType(),
                         CacheSpan = StaticDictionary.Cache()
                     };
                 }).RunsAsync();
                var entity = data.InitResults.ToMapest<List<MovieInitEntity>>();
                var keys = await Scope().Queryable<MovieInitEntity>().Select(t => t.Route).ToListAsync();
                var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.Route)).ToList() : entity;
                if (wait.Count > 0)
                    await Scope().Insertable(wait).ExecuteReturnEntityAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<PageOutDto<List<MovieGlobalEntity>>> Category(string input, int page)
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    RefAsync<int> total = 0;
                    var res = await Scope().Queryable<MovieGlobalEntity>().Where(t => t.IsSearch == false).Where(t => t.InfoRoute == input).ToPageListAsync(page, 20, total);
                    if (res.Count > 0) return new PageOutDto<List<MovieGlobalEntity>>
                    {
                        Data = res,
                        Total = total
                    };
                    return null;
                });
                if (res != null) return res;

                var data = await MovieFactory.Movie(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        MovieType = MovieEnum.Category,
                        ImplType = StaticDictionary.ImplType(),
                        CacheSpan = StaticDictionary.Cache(),
                        Category = new MovieCategory
                        {
                            Page = page,
                            Route = input
                        }
                    };
                }).RunsAsync();
                var entity = data.RootResult.ElementResults.ToMapest<List<MovieGlobalEntity>>();
                var keys = await Scope().Queryable<MovieGlobalEntity>().Where(t => t.IsSearch == false).Where(t => t.InfoRoute == input).Select(t => t.Route).ToListAsync();
                var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.Route)).ToList() : entity;
                if (wait.Count > 0)
                {
                    wait.ForEach(t =>
                    {
                        t.InfoRoute = input;
                        t.IsSearch = false;
                        t.SearchId = -1;
                    });
                    await Scope().Insertable(wait).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
                }
                return new PageOutDto<List<MovieGlobalEntity>>
                {
                    Data = entity,
                    Total = data.RootResult.Total
                };
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<PageOutDto<List<MovieGlobalEntity>>> Search(string input, int searchId, int page)
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    RefAsync<int> total = 0;
                    var res = await Scope().Queryable<MovieGlobalEntity>().Where(t => t.IsSearch == true).Where(t => t.Title.Contains(input) || t.SearchId == searchId).ToPageListAsync(page, 20, total);
                    if (res.Count > 0) return new PageOutDto<List<MovieGlobalEntity>>
                    {
                        Data = res,
                        Total = total
                    };
                    return null;
                });
                if (res != null) return res;
                var data = await MovieFactory.Movie(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        MovieType = MovieEnum.Search,
                        ImplType = StaticDictionary.ImplType(),
                        CacheSpan = StaticDictionary.Cache(),
                        Search = new MovieSearch
                        {
                            Page = page,
                            KeyWord = input,
                            SearchId = searchId
                        }
                    };
                }).RunsAsync();
                var entity = data.RootResult.ElementResults.ToMapest<List<MovieGlobalEntity>>();
                var keys = await Scope().Queryable<MovieGlobalEntity>().Where(t => t.IsSearch == true).Where(t => t.Title.Contains(input) || t.SearchId == searchId).Select(t => t.Route).ToListAsync();
                var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.Route)).ToList() : entity;
                if (wait.Count > 0)
                {
                    wait.ForEach(t =>
                    {
                        t.InfoRoute = input;
                        t.IsSearch = true;
                        t.SearchId = data.RootResult.SearchId;
                    });
                    await Scope().Insertable(wait).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
                }
                return new PageOutDto<List<MovieGlobalEntity>>
                {
                    Data = entity,
                    Total = data.RootResult.Total
                };
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<List<MovieDetailEntity>> Detail(string input)
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    var res = await Scope().Queryable<MovieDetailEntity>().Where(t => t.DetailRoute == input).ToListAsync();
                    if (res.Count > 0) return res;
                    return null;
                });
                if (res != null) return res;
                var data = await MovieFactory.Movie(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        MovieType = MovieEnum.Detail,
                        ImplType = StaticDictionary.ImplType(),
                        CacheSpan = StaticDictionary.Cache(),
                        Detail = new MovieDetail
                        {
                            Route = input
                        }
                    };
                }).RunsAsync();
                var entity = data.DetailResults.ToMapest<List<MovieDetailEntity>>();
                var keys = await Scope().Queryable<MovieDetailEntity>().Where(t => t.DetailRoute == input).Select(t => t.Route).ToListAsync();
                var wait = keys.Count > 0 ? entity.Where(t => keys.Contains(t.Route)).ToList() : entity;
                if (wait.Count > 0) await Scope().Insertable(entity).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
        public async Task<MoviePlayEntity> Play(string input)
        {
            try
            {
                var res = await Force(async obj =>
                {
                    if (obj) return null;
                    var res = await Scope().Queryable<MoviePlayEntity>().FirstAsync(t => t.InfoRoute == input);
                    if (res != null) return res;
                    return null;
                });
                if (res != null) return res;
                var data = await MovieFactory.Movie(opt =>
                {
                    opt.RequestParam = new Input
                    {
                        MovieType = MovieEnum.Watch,
                        ImplType = StaticDictionary.ImplType(),
                        CacheSpan = StaticDictionary.Cache(),
                        Play = new MoviePlay
                        {
                            Route = input
                        }
                    };
                }).RunsAsync();
                var entity = data.PlayResult.ToMapest<MoviePlayEntity>();
                var keys = await Scope().Queryable<MoviePlayEntity>().FirstAsync(t => t.Route == entity.Route);
                if (keys == null)
                {
                    keys.InfoRoute = input;
                    await Scope().Insertable(keys).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
        }
    }
}

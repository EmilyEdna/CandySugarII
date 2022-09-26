using CandySugar.Library.Entity.Anime;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using CandySugar.Library.ViewModel.AniDto;
using Furion.DependencyInjection;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sdk.Component.Anime.sdk;
using Sdk.Component.Anime.sdk.ViewModel;
using Sdk.Component.Anime.sdk.ViewModel.Enums;
using Sdk.Component.Anime.sdk.ViewModel.Request;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ubiety.Dns.Core;
using XExten.Advance.LinqFramework;

namespace CandySugar.Library.Logic.Service
{
    public class AnimeService : DbContext, IAnimeService, IScoped
    {

        public async Task<InitDto> Init()
        {
            var res = new InitDto
            {
                Area = typeof(AnimeAreaEnum).GetEnumNames().ToList(),
                Letters = typeof(AnimeLetterEnum).GetEnumNames().ToList(),
                Types = typeof(AnimeTypeEnum).GetEnumNames().ToList(),
                Years = new List<string>(),
            };
            res.Years.Add("全部");
            var year = DateTime.Now.Year - 1995;
            for (int index = 0; index <= year; index++)
            {
                res.Years.Add((1995 + index).ToString());
            }

            if (StaticDictionary.AnimeSource() == AnimeSourceEnum.YSJDM) return res;

            var read = await Scope().Queryable<AnimeInitEntity>().ToListAsync();
            if (read.Count > 0)
            {
                res.Init = read;
                return res;
            }
            var data = await AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    ImplType = StaticDictionary.ImplType(),
                    AnimeType = AnimeEnum.Init,
                    CacheSpan = StaticDictionary.Cache(),
                    SourceType = StaticDictionary.AnimeSource()
                };
            }).RunsAsync();
            var entity = data.InitResult.RecResults.ToMapest<List<AnimeInitEntity>>();
            await Scope().Insertable(entity).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
            res.Init = entity;
            return res;
        }
        public async Task<PageOutDto<List<AnimeGlobalEntity>>> Category(CateDto input)
        {
            var res = await Force(async obj =>
            {
                if (obj) return null;
                RefAsync<int> total = 0;
                var res = await Scope().Queryable<AnimeGlobalEntity>()
                    .Where(t => t.Source == (int)StaticDictionary.AnimeSource())
                    .Where(t => t.IsSearch == false)
                    .ToPageListAsync(input.Page, StaticDictionary.AnimeSource() == 0 ? 16 : 36, total);
                if (res.Count > 0)
                {
                    return new PageOutDto<List<AnimeGlobalEntity>>
                    {
                        Data = res,
                        Total = total,
                    };
                }
                return null;
            });
            if (res != null) return res;

            var Model = StaticDictionary.AnimeSource() == AnimeSourceEnum.SBDM ? new AnimeCategory
            {
                Route = input?.Route,
                LetterType = Enum.Parse<AnimeLetterEnum>(input?.Letter),
                Page = input.Page,
            } : new AnimeCategory
            {
                Area = Enum.Parse<AnimeAreaEnum>(input.Area),
                LetterType = Enum.Parse<AnimeLetterEnum>(input?.Letter),
                Page = input.Page,
                Type = Enum.Parse<AnimeTypeEnum>(input.Type),
                Year = input.Year,
            };
            var data = await AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    ImplType = StaticDictionary.ImplType(),
                    AnimeType = input.Route.IsNullOrEmpty() ? AnimeEnum.Category : AnimeEnum.CategoryType,
                    SourceType = StaticDictionary.AnimeSource(),
                    CacheSpan = StaticDictionary.Cache(),
                    Category = Model
                };
            }).RunsAsync();
            var entity = data.SeachResult.ElementResult.ToMapest<List<AnimeGlobalEntity>>();
            //TODO：去重
            var keys = await Scope().Queryable<AnimeGlobalEntity>().Where(t => t.IsSearch == false)
                .Where(t => t.Source == (int)StaticDictionary.AnimeSource())
                .Where(t => entity.Select(x => x.Route).Contains(t.Route)).Select(t => t.Route).ToListAsync();
           var  wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.Route)).ToList() : entity;
            if (wait.Count > 0)
                await Scope().Insertable(wait).CallEntityMethod(t => t.Create((int)StaticDictionary.AnimeSource(), false)).ExecuteReturnEntityAsync();
            return new PageOutDto<List<AnimeGlobalEntity>>
            {
                Data = entity,
                Total = data.SeachResult.Total
            };
        }
        public async Task<PageOutDto<List<AnimeGlobalEntity>>> Search(string key, int page)
        {
            var res = await Force(async obj =>
            {
                if (obj) return null;
                RefAsync<int> total = 0;
                var res = await Scope().Queryable<AnimeGlobalEntity>()
                    .Where(t => t.Source == -1)
                    .Where(t => t.Name.Contains(key))
                    .Where(t => t.IsSearch == true)
                    .ToPageListAsync(page, StaticDictionary.AnimeSource() == 0 ? 16 : 36, total);
                if (res.Count > 0)
                {
                    return new PageOutDto<List<AnimeGlobalEntity>>
                    {
                        Data = res,
                        Total = total,
                    };
                }
                else return null;
            });
            if (res != null) return res;
            var data = await AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    ImplType = StaticDictionary.ImplType(),
                    AnimeType = AnimeEnum.Search,
                    SourceType = StaticDictionary.AnimeSource(),
                    CacheSpan = StaticDictionary.Cache(),
                    Search = new AnimeSearch
                    {
                        KeyWord = key,
                        Page = page
                    }
                };
            }).RunsAsync();
            var entity = data.SeachResult.ElementResult.ToMapest<List<AnimeGlobalEntity>>();
            //TODO：去重
            var keys = await Scope().Queryable<AnimeGlobalEntity>().Where(t => t.IsSearch == true)
                .Where(t => t.Source == -1)
                .Where(t => entity.Select(x => x.Route).Contains(t.Route)).Select(t => t.Route).ToListAsync();
            var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.Route)).ToList() : entity;
            if (wait.Count > 0)
                await Scope().Insertable(wait).CallEntityMethod(t => t.Create(-1, true)).ExecuteReturnEntityAsync();
            return new PageOutDto<List<AnimeGlobalEntity>>
            {
                Data = entity,
                Total = data.SeachResult.Total
            };
        }
        public async Task<List<AnimeDetailEntity>> Detail(string input)
        {
            var res = await Force(async obj =>
            {
                if (obj) return null;
                var res = await Scope().Queryable<AnimeDetailEntity>().Where(t => t.DetailRoute.Equals(input)).ToListAsync();
                if (res.Count > 0) return res;
                else return null;
            });
            if (res != null) return res;

            var data = await AnimeFactory.Anime(opt =>
              {
                  opt.RequestParam = new Input
                  {
                      ImplType = StaticDictionary.ImplType(),
                      SourceType = StaticDictionary.AnimeSource(),
                      CacheSpan = StaticDictionary.Cache(),
                      AnimeType = AnimeEnum.Detail,
                      Detail = new AnimeDetail
                      {
                          Route = input
                      }
                  };
              }).RunsAsync();

            var entity = data.DetailResults.ToMapest<List<AnimeDetailEntity>>();
            entity.ForEach(item => { item.DetailRoute = input; });
            var keys = Scope().Queryable<AnimeDetailEntity>().Where(t => entity.Select(x => x.WatchRoute).Contains(t.WatchRoute)).Select(t => t.WatchRoute).ToList();
            var wait = keys.Count > 0 ? entity.Where(t => !keys.Contains(t.WatchRoute)).ToList() : entity;
            if (wait.Count > 0)
                await Scope().Insertable(wait).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
            return entity;
        }
        public async Task<AnimePlayEntity> Watch(string input)
        {
            var res = await Force(async obj =>
            {
                if (obj) return null;
                var res = await Scope().Queryable<AnimeDetailEntity>().Where(t => t.WatchRoute == input && t.IsDownURL == false).FirstAsync();
                var query = await Scope().Queryable<AnimePlayEntity>().Where(t => t.CollectName == res.CollectName && t.AnimeName == res.Name).FirstAsync();
                if (query != null)
                    return query;
                else return null;
            });
            if (res != null) return res;

            var data = await AnimeFactory.Anime(opt =>
            {
                opt.RequestParam = new Input
                {
                    ImplType = StaticDictionary.ImplType(),
                    SourceType = StaticDictionary.AnimeSource(),
                    CacheSpan = StaticDictionary.Cache(),
                    AnimeType = AnimeEnum.Watch,
                    WatchPlay = new AnimeWatch
                    {
                        Route = input,
                        CollectName = res.CollectName
                    }
                };
            }).RunsAsync();
            var entity = data.PlayResult.ToMapest<AnimePlayEntity>();
            //TODO：去重
            var keys = await Scope().Queryable<AnimePlayEntity>().FirstAsync(t => t.PlayURL == entity.PlayURL);
            if (keys == null)
                return await Scope().Insertable(entity).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();
            return entity;
            
        }
    }
}

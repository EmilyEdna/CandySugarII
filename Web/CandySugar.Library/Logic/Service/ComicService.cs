using CandySugar.Library.Entity.Comic;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel.MicDto;
using Furion.DependencyInjection;
using Sdk.Component.Comic.sdk;
using Sdk.Component.Comic.sdk.ViewModel;
using Sdk.Component.Comic.sdk.ViewModel.Enums;
using Sdk.Component.Comic.sdk.ViewModel.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Library.Logic.Service
{
    public class ComicService : DbContext, IComicService, IScoped
    {
        public async Task<ComicViewDto> View(ComicDto input)
        {
            ComicViewDto Result = input.ToMapest<ComicViewDto>();
            var ViewEntity = await Scope().Queryable<ComicViewEntity>().FirstAsync(t => t.ViewRoute == input.Route);
            if (ViewEntity != null)
            {
                Result.Author = ViewEntity.Author?.ToModel<Dictionary<string, string>>() ?? new Dictionary<string, string>();
                Result.Group = ViewEntity.Group?.ToModel<Dictionary<string, string>>() ?? new Dictionary<string, string>();
                Result.Parodies = ViewEntity.Parodies?.ToModel<Dictionary<string, string>>() ?? new Dictionary<string, string>();
                Result.Language = ViewEntity.Language?.ToModel<Dictionary<string, string>>() ?? new Dictionary<string, string>();
                Result.Category = ViewEntity.Category?.ToModel<Dictionary<string, string>>() ?? new Dictionary<string, string>();
                Result.Tag = ViewEntity.Tag?.ToModel<Dictionary<string, string>>() ?? new Dictionary<string, string>();
                Result.UpdateDate = ViewEntity.UpdateDate;
                Result.Previews = Scope().Queryable<ComicViewsEntity>().Where(t => t.ViewId == ViewEntity.Id).Where(t => t.IsReal == false).Select(t => t.Route).ToList();
                Result.Realviews = Scope().Queryable<ComicViewsEntity>().Where(t => t.ViewId == ViewEntity.Id).Where(t => t.IsReal == true).Select(t => t.Route).ToList();
            }
            else
            {
                ComicViewEntity Entity = new ComicViewEntity();
                Entity.Name = input.Name;
                Entity.Cover = input.Cover;
                Entity.ViewRoute = input.Route;

                var data = await ComicFactory.Comic(opt =>
                  {
                      opt.RequestParam = new Input
                      {
                          ImplType = StaticDictionary.ImplType(),
                          ComicType = ComicEnum.View,
                          View = new ComicView
                          {
                              Route = input.Route
                          }
                      };
                  }).RunsAsync();
                Entity.Group = data.ViewResult.Group.Count > 0 ? data.ViewResult.Group.ToJson() : null;
                Entity.Author = data.ViewResult.Author.Count > 0 ? data.ViewResult.Author.ToJson() : null;
                Entity.Parodies = data.ViewResult.Parodies.Count > 0 ? data.ViewResult.Parodies.ToJson() : null;
                Entity.Language = data.ViewResult.Language.Count > 0 ? data.ViewResult.Language.ToJson() : null;
                Entity.Category = data.ViewResult.Category.Count > 0 ? data.ViewResult.Category.ToJson() : null;
                Entity.Tag = data.ViewResult.Tag.Count > 0 ? data.ViewResult.Tag.ToJson() : null;
                Entity.UpdateDate = data.ViewResult.UpdateDate;

                var Main = await Scope().Insertable(Entity).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();

                List<ComicViewsEntity> ViewsEntity = new List<ComicViewsEntity>();
                data.ViewResult.Previews.ForEach(node =>
                {
                    ViewsEntity.Add(new ComicViewsEntity
                    {
                         IsReal=false,
                         ViewId= Main.Id,
                         Route=node
                    });
                });
                data.ViewResult.Realviews.ForEach(node =>
                {
                    ViewsEntity.Add(new ComicViewsEntity
                    {
                        IsReal = true,
                        ViewId = Main.Id,
                        Route = node
                    });
                });

                await Scope().Insertable(ViewsEntity).CallEntityMethod(t => t.Create(true)).ExecuteReturnEntityAsync();

                Result = data.ViewResult.ToMapest<ComicViewDto>();
                Result.Name = input.Name;
                Result.Cover = input.Cover;
                Result.Route = input.Route;
            }
            return Result;
        }
    }
}

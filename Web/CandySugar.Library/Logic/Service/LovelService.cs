using CandySugar.Library.Entity.Lovel;
using Sdk.Component.Lovel.sdk.ViewModel;
using CandySugar.Library.Logic.IService;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Sdk.Component.Lovel.sdk;
using Sdk.Component.Lovel.sdk.ViewModel.Enums;
using Sdk.Component.Lovel.sdk.ViewModel.Request;
using XExten.Advance.LinqFramework;

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
                        LovelType = LovelEnum.Init,
                        Login = new LovelLogin
                        {
                            Account = "kilydoll365",
                            Password = "sion8550"
                        }
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
    }
}

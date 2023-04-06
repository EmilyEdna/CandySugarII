using CandySugar.Library.Entity;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using CandySugar.Library.ViewModel.SysDto;
using Furion;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Sdk.Core;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Library.Logic.Service
{
    public class SysService : DbContext, ISysService, IScoped
    {
        public async Task<PageOutDto<List<UserEntity>>> GetUser(GetUserDto input)
        {
            RefAsync<int> total = 0;
            var data = await Scope().Queryable<UserEntity>()
                   .Where(t => t.UserName.Contains(input.UserName))
                   .ToPageListAsync(input.PageIndex, input.PageSize, total);
            return new PageOutDto<List<UserEntity>>
            {
                Data = data,
                Total = total.Value
            };
        }

        public async Task<UserEntity> UserLogin(UserLoginDto input)
        {
            var data = await Scope().Queryable<UserEntity>()
                 .Where(t => t.UserName.Contains(input.Account) || t.Email.Contains(input.Account))
                 .Where(t => t.Password == input.Password)
                 .Where(t => t.Status == true)
                 .FirstAsync();
            if (data != null)
            {
                SdkLicense.Register(new SdkLicenseModel
                {
                    Account = "EmilyEdna",
                    Password = DateTime.Now.ToString("yyyyMMdd")
                });
                return data;
            }
            return null;
        }

        public async Task<bool> UserRegist(UserRegistDto input)
        {
            var target = input.ToMapest<UserEntity>();
            target.Status = true;
            return (await Scope().Insertable(target).CallEntityMethod(t => t.Create(true)).ExecuteCommandAsync()) > 0;
        }

        public async Task<bool> RemoveUser(List<Guid> input)
        {
            return (await Scope().Deleteable<UserEntity>(t => input.Contains(t.Id)).ExecuteCommandAsync()) > 0;
        }

        public async Task<bool> UserStatus(Guid Id, bool Status)
        {
            return (await Scope().Updateable<UserEntity>().SetColumns(t => t.Status == Status).Where(t => t.Id == Id).ExecuteCommandAsync()) > 0;
        }

        public async Task<UserAttachDto> UserOption(UserAttachDto input)
        {
            var Id = App.User?.FindFirstValue("UserId");
            if (Id.IsNullOrEmpty()) throw Oops.Oh("401");
            var data = await Scope().Queryable<UserAttachEntity>().FirstAsync(t => t.Id == Guid.Parse(Id));
            if (data == null)
            {
                var entity = input.ToMapest<UserAttachEntity>();
                entity.Id = Guid.Parse(Id);
                StaticDictionary.UserAttachEntity = await Scope().Insertable(entity).CallEntityMethod(t => t.Create(false)).ExecuteReturnEntityAsync();
                return StaticDictionary.UserAttachEntity.ToMapest<UserAttachDto>();
            }
            else
            {
                input.GetType().GetProperties().ForEnumerEach(item =>
                {
                    data.GetType().GetProperty(item.Name).SetValue(data, item.GetValue(input));
                });
                StaticDictionary.UserAttachEntity = data;
                await Scope().Updateable(data).Where(t => t.Id == data.Id).ExecuteCommandAsync();
                return StaticDictionary.UserAttachEntity.ToMapest<UserAttachDto>();
            }
        }
    }
}

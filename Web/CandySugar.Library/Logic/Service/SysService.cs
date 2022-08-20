﻿using CandySugar.Library.Entity;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using CandySugar.Library.ViewModel.SysDto;
using Furion.DependencyInjection;
using Sdk.Core;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Library.Logic.Service
{
    public class SysService : DbContext, ISysService, IScoped
    {
        public async Task<PageOutDto<List<UserEntity>>> GetUser(GetUserDto input)
        {
            RefAsync<int> total =0;
            var data = await Scope().Queryable<UserEntity>()
                   .Where(t => t.UserName.Contains(input.UserName))
                   .ToPageListAsync(input.PageIndex, input.PageSize, total);
            return new PageOutDto<List<UserEntity>>
            {
                Data = data,
                Total = total.Value
            };
        }

        public async Task<bool> UserLogin(UserLoginDto input)
        {
            var data = await Scope().Queryable<UserEntity>()
                 .Where(t => t.UserName.Contains(input.Account) || t.Email.Contains(input.Account))
                 .Where(t => t.Password == input.Password)
                 .Where(t => t.Status == 1)
                 .FirstAsync();
            if (data != null)
                return SdkLicense.Register(new SdkLicenseModel
                {
                    Account = "EmilyEdna",
                    Password = DateTime.Now.ToString("yyyyMMdd")
                });
            else return false;
        }

        public async Task<bool> UserRegist(UserRegistDto input)
        {
            var target = input.ToMapest<UserEntity>();
            target.Status = 1;
            return (await Scope().Insertable(target).CallEntityMethod(t => t.Create()).ExecuteCommandAsync()) > 0;
        }
    }
}

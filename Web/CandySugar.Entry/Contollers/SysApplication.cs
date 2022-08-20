using CandySugar.Library.Entity;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using CandySugar.Library.ViewModel.SysDto;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;

namespace CandySugar.Entry.Contollers
{
    [ApiDescriptionSettings("系统", Tag = "系统", SplitCamelCase = false), NonUnify, Route("/api/sys")]
    public class SysApplication : IDynamicApiController
    {
        readonly ISysService SysService;
        public SysApplication(ISysService SysService)
        {
            this.SysService = SysService;
        }

        [HttpPost]
        public async Task<bool> UserLogin(UserLoginDto input) => await SysService.UserLogin(input);
        [HttpPost]
        public async Task<bool> UserRegist(UserRegistDto input) => await SysService.UserRegist(input);
        [HttpPost]
        public async Task<PageOutDto<List<UserEntity>>> GetUser(GetUserDto input) => await SysService.GetUser(input);
    }
}

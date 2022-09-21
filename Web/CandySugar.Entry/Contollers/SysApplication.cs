using CandySugar.Library.Entity;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using CandySugar.Library.ViewModel.SysDto;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;

namespace CandySugar.Entry.Contollers
{
    /// <summary>
    /// 系统
    /// </summary>
    [ApiDescriptionSettings("系统", Tag = "系统", SplitCamelCase = false), NonUnify, Route("/api/sys")]
    public class SysApplication : IDynamicApiController
    {
        readonly ISysService SysService;
        public SysApplication(ISysService SysService)
        {
            this.SysService = SysService;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> UserLogin(UserLoginDto input) => await SysService.UserLogin(input);
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> UserRegist(UserRegistDto input) => await SysService.UserRegist(input);
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageOutDto<List<UserEntity>>> GetUser(GetUserDto input) => await SysService.GetUser(input);
        //删除
        [HttpPost]
        public async Task<bool> RemoveUser(List<Guid> input) => await SysService.RemoveUser(input);
        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> UserStatus(Guid Id, bool Status) => await SysService.UserStatus(Id, Status);
    }
}

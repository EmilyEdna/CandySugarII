using CandySugar.Library.Entity;
using CandySugar.Library.Logic.IService;
using CandySugar.Library.ViewModel;
using CandySugar.Library.ViewModel.SysDto;
using Furion.DataEncryption;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CandySugar.Entry.Contollers
{
    /// <summary>
    /// 系统
    /// </summary>
    [ApiDescriptionSettings("系统", Tag = "系统", SplitCamelCase = false), NonUnify, Route("/api/sys"), AllowAnonymous]
    public class SysApplication : IDynamicApiController
    {
        readonly ISysService SysService;
        readonly IHttpContextAccessor HttpContext;
        public SysApplication(ISysService SysService, IHttpContextAccessor HttpContext)
        {
            this.SysService = SysService;
            this.HttpContext = HttpContext;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> UserLogin(UserLoginDto input)
        {
            var result = await SysService.UserLogin(input);
            if (result)
            {
                var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>()
                {
                    { "Account", input.Account },
                    { "Password",input.Password }
                });
                var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);
                HttpContext.HttpContext.Response.Headers["access-token"] = accessToken;
                HttpContext.HttpContext.Response.Headers["x-access-token"] = refreshToken;
                return true;
            }
            else return false;
        }
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
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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

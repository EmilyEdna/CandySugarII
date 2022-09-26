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
        public async Task<CandyResult<bool>> UserLogin(UserLoginDto input)
        {
            var result = await SysService.UserLogin(input);
            if (result!=null)
            {
                var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>()
                {
                    { "UserId",result.Id },
                    { "Account", result.UserName }
                });
                var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);
                HttpContext.HttpContext.Response.Headers["access-token"] = accessToken;
                HttpContext.HttpContext.Response.Headers["x-access-token"] = refreshToken;
                return CandyResult<bool>.Result(true);
            }
            else return CandyResult<bool>.Result(false);
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CandyResult<bool>> UserRegist(UserRegistDto input) => CandyResult<bool>.Result(await SysService.UserRegist(input));
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CandyResult<PageOutDto<List<UserEntity>>>> GetUser(GetUserDto input) => CandyResult<PageOutDto<List<UserEntity>>>.Result(await SysService.GetUser(input));
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CandyResult<bool>> RemoveUser(List<Guid> input) => CandyResult<bool>.Result(await SysService.RemoveUser(input));
        /// <summary>
        /// 状态
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CandyResult<bool>> UserStatus(Guid Id, bool Status) => CandyResult<bool>.Result(await SysService.UserStatus(Id, Status));
        /// <summary>
        /// 用户设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CandyResult<UserAttachDto>> UserOption(UserAttachDto input) => CandyResult<UserAttachDto>.Result(await SysService.UserOption(input));
    }
}

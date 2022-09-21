using Furion.Authorization;
using Furion.DataEncryption;
using Furion.LinqBuilder;
using Microsoft.AspNetCore.Authorization;
using Sdk.Core;
using XExten.Advance.StaticFramework;

namespace CandySugar.Entry
{
    /// <summary>
    /// 权限认证
    /// </summary>
    public class JwtHandler : AppAuthorizeHandler
    {
        /// <summary>
        /// 重写 Handler 添加自动刷新收取逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            // 自动刷新 token
            if (JWTEncryption.AutoRefreshToken(context, context.GetCurrentHttpContext()))
            {
                await AuthorizeHandleAsync(context);
            }
            else context.Fail();    // 授权失败
        }

        /// <summary>
        /// 验证管道，也就是验证核心代码
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public override Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
        {
            var auth = context.User.Claims.ToDictionary(t => t.Type, t => t.Value);
            if (auth.ContainsKey("Account") && auth.ContainsKey("UserId"))
            {
                if (!auth["Account"].IsNullOrEmpty() && !auth["UserId"].IsNullOrEmpty() && DateTime.Now <= SyncStatic.ConvertStamptime(auth["exp"]))
                {
                    SdkLicense.Register(new SdkLicenseModel
                    {
                        Account = "EmilyEdna",
                        Password = DateTime.Now.ToString("yyyyMMdd")
                    });
                    return base.PipelineAsync(context, httpContext);
                }
                return Task.FromResult(false);
            }
            return Task.FromResult(false);
        }
    }
}

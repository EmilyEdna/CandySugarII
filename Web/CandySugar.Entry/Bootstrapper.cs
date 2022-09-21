using CandySugar.Library;
using Furion;

namespace CandySugar.Entry
{
    /// <summary>
    /// 启动项
    /// </summary>
    public class Bootstrapper : AppStartup
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsAccessor();
            services.AddJwt<JwtHandler>(null, null, null,true);
            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            }).AddInjectWithUnifyResult();
        }
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="lifetime"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCorsAccessor();
            app.UseInject(string.Empty);

            DbContext.Instance.InitTables();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

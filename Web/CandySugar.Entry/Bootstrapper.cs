using CandySugar.Library;
using Furion;

namespace CandySugar.Entry
{
    public class Bootstrapper : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            }).AddInjectWithUnifyResult();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseInject(string.Empty);

            DbContext.Instance.InitTables();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

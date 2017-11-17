using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportWheelOfFate.Api.Controllers;
using SupportWheelOfFate.Core.Generator;
using SupportWheelOfFate.Core.Repository;

namespace SupportWheelOfFate.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IWheelUsherRepository, WheelUsherRepository>();
            services.AddTransient<IEngineersGenerator, EngineersGenerator>();
            services.AddTransient<WheelController, WheelController>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvcWithDefaultRoute();
//            app.UseMvc();
        }
    }
}

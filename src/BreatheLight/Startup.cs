using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using BreatheLight.Controllers;
using BreatheLight.Core.Data;
using BreatheLight.Core.Implements;
using BreatheLight.Core.Interfaces;
using BreatheLight.Implements;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UpPwm.Implements;
using UpPwm.Interfaces;

namespace BreatheLight
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IConfiguration _appConfiguration;
        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration config, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            _appConfiguration = config;
            _hostingEnvironment = env;
            _loggerFactory = loggerFactory;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMemoryCache();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var connection = _appConfiguration["ConnectionString"]?.ToString();
            if (connection == null)
                throw new ArgumentNullException("ConnectionString of appsettings.json");
            services.AddDbContext<LightDbContext>(options =>
            {
                if (bool.TryParse(_appConfiguration["IsUseInMemoryDatabase"], out bool isMemDb) && isMemDb)
                {
                    options.UseInMemoryDatabase("light");
                }
                else
                {
                    options.UseSqlite(connection);
                }
            });

            #region 手动注册
            // services.AddScoped<IPwmRegulator, PwmRegulator>();
            // services.AddScoped<ISystemCommander, SystemCommander>();

            // services.AddSingleton<ITimeMonitor, TimeMonitor>();
            // services.AddScoped<ILightRegulator, LightRegulator>();
            // services.AddScoped<ILightSequenceDbPersistence, LightSequenceDbPersistence>();
            #endregion

            #region autofac自动注册

            var builder = new ContainerBuilder();//实例化 AutoFac  容器   

            var assemblys = new List<string>() { "UpPwm", "BreatheLight.Core" }.Select(t => Assembly.Load(t)).ToArray();
            builder.RegisterAssemblyTypes(assemblys)
                .Where(w => !w.Name.Contains("TimeMonitor"))
                .AsImplementedInterfaces();

            builder.RegisterType<TimeMonitor>().As<ITimeMonitor>().SingleInstance();

            builder.Populate(services);
            ApplicationContainer = builder.Build();

            ApplicationContainer.Resolve<ITimeMonitor>().Start();
            var sp = new AutofacServiceProvider(ApplicationContainer);//第三方IOC接管 core内置DI容器

            return sp;
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

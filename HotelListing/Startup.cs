using HotelListing.Common;
using HotelListing.Data;
using HotelListing.Data.UnitOfWork;
using HotelListing.Dtos.Configurations;
using HotelListing.Services.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing
{
    public class Startup
    {
        private readonly SiteSettings _siteSettings;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _siteSettings = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));

            services.AddControllers(options =>
            {
                // بصورت پیشفرض تمامی کنترلرها و اکشن ها نیاز به احراز هوییت خواهند داشت
                // مگر اینکه خلافش ثابت بشه از طریق اتریبیوت AllowAnnonymous
                // حالا روی هر اکشن هم میتونیم اتریبیوت اوتورایز رو اعمال کنیم با رول های موردنظرمون
                //options.Filters.Add(new AuthorizeFilter());
            }).AddNewtonsoftJson(options =>
            {
                // این خط کد برای این لازمه چون چرخه روابط بین کلاس هارو ایگنور میکنه
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddDbContextPool<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("HotelListing"));
            });

            services.ConfigureIdentity();
            services.AddJwtAuthentication(_siteSettings.JwtSettings);

            services.AddCors(cors =>
            {
                cors.AddPolicy("AllowAll",
                    po => po.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IJwtService), typeof(JwtService));
            services.AddAutoMapper(typeof(MapperInitializer));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelListing", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelListing v1"));

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

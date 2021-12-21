using DocumentLibrary.Application.DBHelper.MySql;
using DocumentLibrary.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using System.IO.Compression;
using DocumentLibrary.Application.DBHelper;
using DocumentLibrary.Application.Helper;
using DocumentLibrary.Application.Interface;
using DocumentLibrary.Application.Profile;
using DocumentLibrary.MySql.Models;

namespace DocumentManagerSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DocumentProfile));
            services.AddCors();

            services
                .AddScoped<IDocumentDbHelper, DocumentDbHelper>()
                .AddScoped<IDocumentVersionHistoryDbHelper, DocumentVersionHistoryDbHelper>()
                .AddScoped<IFileHelper, StageFileHelper>();

            var mySqlVersion = new Version(8, 0, 19);
            services.AddEntityFrameworkMySql();
            services.AddDbContextPool<AccountDbContext>(
                options => options.UseMySql("server=127.0.0.1;port=3306;database=account;uid=root;password=Aa123456",
                    mySqlOptions => {
                        mySqlOptions
                            .ServerVersion(mySqlVersion, ServerType.MySql)
                            .CharSet(CharSet.Utf8Mb4)
                            .MigrationsAssembly("EgkLibrary.InfoDB");
                    }
                )).AddDbContext<DocumentDbContext>(
                options => options.UseMySql("server=127.0.0.1;port=3306;database=demo;uid=root;password=Aa123456",
                    mySqlOptions => {
                        mySqlOptions
                            .ServerVersion(mySqlVersion, ServerType.MySql)
                            .CharSet(CharSet.Utf8Mb4);
                    }
                ));

            services.AddControllersWithViews()
                    .AddNewtonsoftJson(options => {
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddResponseCompression(options => {
                options.Providers.Add<BrotliCompressionProvider>();
                options.EnableForHttps = true;
            });

            services.Configure<BrotliCompressionProviderOptions>(options => {
                options.Level = CompressionLevel.NoCompression;
            });

            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors(builder =>
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            );

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapFallbackToController("Index", "Home");
            });
        }
    }
}

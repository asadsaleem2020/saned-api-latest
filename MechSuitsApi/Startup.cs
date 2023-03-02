
using System.IO;
using System.Text;
using MechSuitsApi.Classes;
using MechSuitsApi.Classes.Auth;
using MechSuitsApi.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;

namespace MechSuitsApi
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

            var key = "fdsaf9ikj%&%@66G^@7)(KJHH&^";
            services.AddControllers();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddSingleton<IJwtAuthManager>(new JwtAuthManager(key));
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: MyAllowSpecificOrigins,
            //                      policy =>
            //                      {
            //                          policy.WithOrigins("http://localhost:1010/").AllowAnyHeader().AllowAnyMethod();
            //                      });
            //});
            services.AddCors(c =>
                      {
                          c.AddPolicy("AllowOrigin", options => options.WithOrigins("https://localhost:4200/ ", "https://localhost:5000/ ", "https://localhost:1010/ ", "http://localhost:8080 always", "http://182.176.113.125:8004/", "http://202.166.169.158:8020/")
                          .AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
                      });
            //services.AddCors(c =>
            //{
            //    c.AddPolicy("AllowOrigin", options => options.WithOrigins("https://localhost:4200/ ", "https://localhost:5000/", "https://localhost:1010/", "http://localhost:8080 always", "http://182.176.113.125:8004/", "http://202.166.169.158:8020/").AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            //});
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MecERP")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
            services.AddHttpContextAccessor();
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddAuthorization(options =>
            {
                options.AddPolicy("user", policy => policy.RequireClaim("Roles"));
                options.AddPolicy("sale", policy => policy.RequireClaim("Roles"));
                options.AddPolicy("admin", policy => policy.RequireClaim("Roles"));
                options.AddPolicy("superadmin", policy => policy.RequireClaim("Roles"));
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
          //  if (env.IsDevelopment())
          //  {
                app.UseDeveloperExceptionPage();
           // }

            app.UseRouting();
         


            app.UseCors(options => options.WithOrigins("https://localhost:4200/ ", "https://localhost:5000/ ", "https://localhost:1010/ ", "http://localhost:8080 always", "http://182.176.113.125:8004/", "http://202.166.169.158:8020/").AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            //app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

//using MechSuitsApi.Classes;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using MechSuitsApi.Interfaces;
//using MechSuitsApi.Classes.Auth;
//using Microsoft.OpenApi.Models;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using Wkhtmltopdf.NetCore;
//using Microsoft.AspNetCore.Http.Features;
//using Microsoft.Extensions.FileProviders;
//using Microsoft.AspNetCore.Http;
//using System.IO;

//namespace MechSuitsApi
//{
//    public class Startup
//    {
//        public Startup(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        public IConfiguration Configuration { get; }

//        // This method gets called by the runtime. Use this method to add services to the container.
//        public void ConfigureServices(IServiceCollection services)
//        {

//            var key = "fdsaf9ikj%&%@66G^@7)(KJHH&^";
//            services.AddControllers();


//         //   services.AddSwaggerGen(c =>
//          //  {
//           //     c.SwaggerDoc("v1", new OpenApiInfo
//           //     {
//            //        Title = "railo API",
//            //        Version = "v1",
//            ///        Description = "Description for the API goes here.",
//            //        Contact = new OpenApiContact
//              //      {
//               //         Name = "Ankush Jain",
//              //          Email = string.Empty,
//             //           Url = new Uri("https://coderjony.com/"),
//            //        },
//           //     });
//           // }
//           // );
//            services.AddAuthentication(x => {
//                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//            }).AddJwtBearer(x=> {
//                x.RequireHttpsMetadata = false;
//                x.SaveToken = true;
//                x.TokenValidationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
//                    ValidateIssuer = false,
//                    ValidateAudience = false
//                };
//            });
//            services.AddSingleton<IJwtAuthManager>(new JwtAuthManager(key)   );

//            services.AddCors(c =>
//            {
//                c.AddPolicy("AllowOrigin", options => options.WithOrigins("https://localhost:4200/ ", "https://localhost:5000/ ", "https://localhost:1010/ ", "http://localhost:8080 always", "http://182.176.113.125:8004/", "http://202.166.169.158:8020/")
//                .AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
//            });
//            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MecERP")));
//            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//            services.Configure<FormOptions>(o => {
//                o.ValueLengthLimit = int.MaxValue;
//                o.MultipartBodyLengthLimit = int.MaxValue;
//                o.MemoryBufferThreshold = int.MaxValue;
//            });
//             services.AddControllers().AddNewtonsoftJson(options =>
//     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
// );
//        }

//        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();

//            }

//            app.UseRouting();
//            //qasim addition starts
//            //    app.UseSwagger();
//            //  app.UseSwaggerUI(c =>
//            //  {
//            //      c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zomato API V1");
//            //
//            // To serve SwaggerUI at application's root page, set the RoutePrefix property to an empty string.
//            //       c.RoutePrefix = string.Empty;
//            //});


//            //qasim addition ends
//         app.UseCors(options => options.WithOrigins("https://localhost:4200/ ", "https://localhost:5000/ ", "https://localhost:1010/ ", "http://localhost:8080 always", "http://182.176.113.125:8004/", "http://202.166.169.158:8020/").AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
//            //WithOrigins("https://localhost:4200/", "http://localhost:8080", "http://182.176.113.125:8004/", "http://202.166.169.158:8020/" )

//            // app.UseStaticFiles();
//            //  app.UseStaticFiles(new StaticFileOptions()
//            //  {
//            //     FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
//            //     RequestPath = new PathString("/Resources")
//            //  });
//            app.UseAuthentication();
//            app.UseAuthorization();
//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapControllers();
//            });
//        }
//    }
//}

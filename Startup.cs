using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using ESPL.KP.Services;
using Microsoft.EntityFrameworkCore;
using ESPL.KP.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Diagnostics;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json.Serialization;
using System.Linq;
using AspNetCoreRateLimit;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ESPL.KP.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ESPL.KP.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using ESPL.KP.Helpers.Core;

namespace ESPL.KP
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            // register the DbContext on the container, getting the connection string from
            // appSettings (note: use this during development; in a production environment,
            // it's better to store the connection string in an environment variable)
            var connectionString = Configuration["connectionStrings:libraryDBConnectionString"];
            services.AddDbContext<LibraryContext>(o => o.UseSqlServer(connectionString));
            services.AddTransient<IdentityInitializer>();
            services.AddIdentity<ESPLUser, IdentityRole>().AddEntityFrameworkStores<LibraryContext>();
            services.Configure<IdentityOptions>(config =>
            {
                config.Cookies.ApplicationCookie.Events =
                    new CookieAuthenticationEvents()
                    {
                        OnRedirectToLogin = (ctx) =>
                        {
                            if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                            {
                                ctx.Response.StatusCode = 401;
                            }

                            return Task.CompletedTask;
                        },
                        OnRedirectToAccessDenied = (ctx) =>
                        {
                            if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                            {
                                ctx.Response.StatusCode = 403;
                            }

                            return Task.CompletedTask;
                        }
                    };
            });
            services.AddCors();
            services.AddMvc(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                // setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());

                var xmlDataContractSerializerInputFormatter =
                new XmlDataContractSerializerInputFormatter();
                xmlDataContractSerializerInputFormatter.SupportedMediaTypes
                    .Add("application/vnd.marvin.authorwithdateofdeath.full+xml");
                setupAction.InputFormatters.Add(xmlDataContractSerializerInputFormatter);

                var jsonInputFormatter = setupAction.InputFormatters
                .OfType<JsonInputFormatter>().FirstOrDefault();

                if (jsonInputFormatter != null)
                {
                    jsonInputFormatter.SupportedMediaTypes
                    .Add("application/vnd.marvin.author.full+json");
                    jsonInputFormatter.SupportedMediaTypes
                    .Add("application/vnd.marvin.authorwithdateofdeath.full+json");
                }

                var jsonOutputFormatter = setupAction.OutputFormatters
                    .OfType<JsonOutputFormatter>().FirstOrDefault();

                if (jsonOutputFormatter != null)
                {
                    jsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.marvin.hateoas+json");
                }

            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling =
                ReferenceLoopHandling.Ignore;
                // options.SerializerSettings.PreserveReferencesHandling =
                // PreserveReferencesHandling.Objects;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AT.C", policy => policy.RequireClaim("AT.C"));
                options.AddPolicy("AT.R", policy => policy.RequireClaim("AT.R"));
                options.AddPolicy("AT.U", policy => policy.RequireClaim("AT.U"));
                options.AddPolicy("AT.D", policy => policy.RequireClaim("AT.D"));
                options.AddPolicy("SystemAdmin", policy => policy.RequireClaim("SystemAdmin"));

                Config.GetAppModulesList().ToList().ForEach(name =>
                {
                    options.AddPolicy(string.Format("{0}.R", name), policy => policy.RequireClaim(string.Format("{0}.R", name)));
                    options.AddPolicy(string.Format("{0}.C", name), policy => policy.RequireClaim(string.Format("{0}.C", name)));
                    options.AddPolicy(string.Format("{0}.U", name), policy => policy.RequireClaim(string.Format("{0}.U", name)));
                    options.AddPolicy(string.Format("{0}.D", name), policy => policy.RequireClaim(string.Format("{0}.D", name)));
                });
            });

            // register the repository
            services.AddScoped<ILibraryRepository, LibraryRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IUrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>()
                .ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            services.AddTransient<ITypeHelperService, TypeHelperService>();

            services.AddHttpCacheHeaders(
                (expirationModelOptions)
                =>
                {
                    expirationModelOptions.MaxAge = 600;
                },
                (validationModelOptions)
                =>
                {
                    validationModelOptions.AddMustRevalidate = true;
                });

            services.AddMemoryCache();

            services.Configure<IpRateLimitOptions>((options) =>
            {
                options.GeneralRules = new System.Collections.Generic.List<RateLimitRule>()
                {
                    new RateLimitRule()
                    {
                        Endpoint = "*",
                        Limit = 1000,
                        Period = "5m"
                    },
                    new RateLimitRule()
                    {
                        Endpoint = "*",
                        Limit = 200,
                        Period = "10s"
                    }
                };
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, LibraryContext libraryContext,
            IdentityInitializer identitySeeder)
        {
            loggerFactory.AddConsole();

            loggerFactory.AddDebug(LogLevel.Information);

            //  loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider());

            loggerFactory.AddNLog();

            app.UseCors(cfg =>
            {
                cfg.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500,
                                exceptionHandlerFeature.Error,
                                exceptionHandlerFeature.Error.Message);
                        }

                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");

                    });
                });
            }

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ESPL.KP.Entities.Author, ESPL.KP.Models.AuthorDto>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    $"{src.FirstName} {src.LastName}"))
                    .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                    src.DateOfBirth.GetCurrentAge(src.DateOfDeath)));
                cfg.CreateMap<ESPL.KP.Entities.Book, ESPL.KP.Models.BookDto>();
                cfg.CreateMap<ESPL.KP.Models.AuthorForCreationDto, ESPL.KP.Entities.Author>();
                cfg.CreateMap<ESPL.KP.Models.AuthorForCreationWithDateOfDeathDto, ESPL.KP.Entities.Author>();
                cfg.CreateMap<ESPL.KP.Models.BookForCreationDto, ESPL.KP.Entities.Book>();
                cfg.CreateMap<ESPL.KP.Models.BookForUpdateDto, ESPL.KP.Entities.Book>();
                cfg.CreateMap<ESPL.KP.Entities.Book, ESPL.KP.Models.BookForUpdateDto>();
                cfg.CreateMap<ESPL.KP.Entities.Core.AppModule, ESPL.KP.Models.Core.AppModuleDto>();
                cfg.CreateMap<ESPL.KP.Models.Core.AppModuleForCreationDto, ESPL.KP.Entities.Core.AppModule>();
                cfg.CreateMap<ESPL.KP.Entities.ESPLUser, ESPL.KP.Models.Core.ESPLUserDto>();

                cfg.CreateMap<ESPL.KP.Models.Core.ESPLUserForCreationDto, ESPL.KP.Entities.ESPLUser>();
                cfg.CreateMap<IdentityRole, ESPL.KP.Models.Core.ESPLRoleDto>();
                cfg.CreateMap<ESPL.KP.Models.Core.ESPLRoleForCreationDto, IdentityRole>();

                cfg.CreateMap<ESPL.KP.Entities.MstDepartment, ESPL.KP.Models.DepartmentDto>();
                cfg.CreateMap<ESPL.KP.Models.DepartmentForCreationDto, ESPL.KP.Entities.MstDepartment>();
                cfg.CreateMap<MstDepartment, DepartmentForCreationDto>();
                cfg.CreateMap<KP.Models.DepartmentForUpdationDto, ESPL.KP.Entities.MstDepartment>();
                cfg.CreateMap<ESPL.KP.Entities.MstDepartment, ESPL.KP.Models.DepartmentForUpdationDto>();

                cfg.CreateMap<ESPL.KP.Entities.MstOccurrenceType, ESPL.KP.Models.OccurrenceTypeDto>();
                cfg.CreateMap<ESPL.KP.Models.OccurrenceTypeForCreationDto, ESPL.KP.Entities.MstOccurrenceType>();
                cfg.CreateMap<ESPL.KP.Entities.MstOccurrenceType, ESPL.KP.Models.OccurrenceTypeForCreationDto>();
                cfg.CreateMap<KP.Models.OccurrenceTypeForUpdationsDto, ESPL.KP.Entities.MstOccurrenceType>();
                cfg.CreateMap<ESPL.KP.Entities.MstOccurrenceType, ESPL.KP.Models.OccurrenceTypeForUpdationsDto>();

                cfg.CreateMap<ESPL.KP.Entities.MstArea, ESPL.KP.Models.AreaDto>();
                cfg.CreateMap<ESPL.KP.Models.AreaForCreationDto, ESPL.KP.Entities.MstArea>();
                cfg.CreateMap<ESPL.KP.Entities.MstArea, ESPL.KP.Models.AreaForCreationDto>();
                cfg.CreateMap<KP.Models.AreaForUpdationDto, ESPL.KP.Entities.MstArea>();
                cfg.CreateMap<ESPL.KP.Entities.MstArea, ESPL.KP.Models.AreaForUpdationDto>();

                cfg.CreateMap<ESPL.KP.Entities.MstDesignation, ESPL.KP.Models.DesignationDto>();
                cfg.CreateMap<ESPL.KP.Models.DesignationForCreationDto, ESPL.KP.Entities.MstDesignation>();
                cfg.CreateMap<ESPL.KP.Entities.MstDesignation, ESPL.KP.Models.DesignationForCreationDto>();
                cfg.CreateMap<KP.Models.DesignationForUpdationDto, ESPL.KP.Entities.MstDesignation>();
                cfg.CreateMap<ESPL.KP.Entities.MstDesignation, ESPL.KP.Models.DesignationForUpdationDto>();

                cfg.CreateMap<ESPL.KP.Entities.MstOccurrenceBook, ESPL.KP.Models.OccurrenceBookDto>();
                // .ForMember(dest => dest.Area, opt => opt.MapFrom(src =>src.MstArea))
                // .ForMember(dest => dest.Department, opt => opt.MapFrom(src =>src.MstDepartment))
                // .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>src.MstStatus))
                // .ForMember(dest => dest.OccurrenceType, opt => opt.MapFrom(src =>src.MstOccurrenceType))
                // .ForMember(dest=>dest.Employee, opt => opt.MapFrom(src =>src.MstEmployee));
                cfg.CreateMap<ESPL.KP.Models.OccurrenceBookForCreationDto, ESPL.KP.Entities.MstOccurrenceBook>();
                cfg.CreateMap<ESPL.KP.Entities.MstOccurrenceBook, ESPL.KP.Models.OccurrenceBookForCreationDto>();
                cfg.CreateMap<ESPL.KP.Models.OccurrenceBookForUpdationDto, ESPL.KP.Entities.MstOccurrenceBook>();
                cfg.CreateMap<ESPL.KP.Entities.MstOccurrenceBook, ESPL.KP.Models.OccurrenceBookForUpdationDto>();

                cfg.CreateMap<ESPL.KP.Entities.MstShift, ESPL.KP.Models.ShiftDto>();
                cfg.CreateMap<ESPL.KP.Models.ShiftForCreationDto, ESPL.KP.Entities.MstShift>();
                cfg.CreateMap<ESPL.KP.Entities.MstShift, ESPL.KP.Models.ShiftForCreationDto>();
                cfg.CreateMap<KP.Models.ShiftForUpdationDto, ESPL.KP.Entities.MstShift>();
                cfg.CreateMap<ESPL.KP.Entities.MstShift, ESPL.KP.Models.ShiftForUpdationDto>();

                cfg.CreateMap<ESPL.KP.Entities.MstStatus, ESPL.KP.Models.StatusDto>();
                cfg.CreateMap<ESPL.KP.Models.StatusForCreationDto, ESPL.KP.Entities.MstStatus>();
                cfg.CreateMap<ESPL.KP.Entities.MstStatus, ESPL.KP.Models.StatusForCreationDto>();
                cfg.CreateMap<KP.Models.StatusForUpdationDto, ESPL.KP.Entities.MstStatus>();
                cfg.CreateMap<ESPL.KP.Entities.MstStatus, ESPL.KP.Models.StatusForUpdationDto>();


                cfg.CreateMap<ESPL.KP.Entities.MstEmployee, ESPL.KP.Models.EmployeeDto>();
                // .ForMember(dest => dest.Area, opt => opt.MapFrom(src =>src.MstArea))
                // .ForMember(dest => dest.Department, opt => opt.MapFrom(src =>src.MstDepartment))
                // .ForMember(dest => dest.Designation, opt => opt.MapFrom(src =>src.MstDesignation))
                // .ForMember(dest => dest.Shift, opt => opt.MapFrom(src =>src.MstShift))
                // .ForMember(dest => dest.OccurrenceBooks, opt => opt.MapFrom(src =>src.MstOccurrenceBooks));
                cfg.CreateMap<ESPL.KP.Models.EmployeeForCreationDto, ESPL.KP.Entities.MstEmployee>();
                cfg.CreateMap<ESPL.KP.Entities.MstEmployee, ESPL.KP.Models.EmployeeForCreationDto>();
                cfg.CreateMap<ESPL.KP.Models.EmployeeForUpdationDto, ESPL.KP.Entities.MstEmployee>();
                cfg.CreateMap<ESPL.KP.Entities.MstEmployee, ESPL.KP.Models.EmployeeForUpdationDto>();

                cfg.CreateMap<ESPL.KP.Entities.MstOccurrenceBook, ESPL.KP.Models.OccurrenceReportDto>()
                    .ForMember(dest => dest.Area, opt => opt.MapFrom(src =>
                    src.MstArea))
                    .ForMember(dest => dest.Offence, opt => opt.MapFrom(src =>
                    src.MstOccurrenceType))
                    .ForMember(dest => dest.Department, opt => opt.MapFrom(src =>
                    src.MstDepartment))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                    src.MstStatus));

                cfg.CreateMap<ESPL.KP.Entities.MstOccurrenceBook, ESPL.KP.Models.Statistics>();
                //    .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src =>
                //    src.MstStatus.StatusName));
            });

            identitySeeder.Seed().Wait();
            libraryContext.EnsureSeedDataForContext();
            app.UseIpRateLimiting();
            app.UseHttpCacheHeaders();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseIdentity();
            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["Tokens:Issuer"],
                    ValidAudience = Configuration["Tokens:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                    ValidateLifetime = true
                }
            });

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUi(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            
        }
    }
}

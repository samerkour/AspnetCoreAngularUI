using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Versioning;
using AutoWrapper;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using System.IO;
using STech.Infrastructure;
using STech.CrossCutting;
using MassTransit;
using STech.Infrastructure.Context;
using STech.Presentation.Api.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Is4.IdentityServer4.Shared.Configuration;
using Is4.IdentityServer4.Shared.Helpers;
using Is4.IdentityServer4.Shared.Authentication;
using System.IdentityModel.Tokens.Jwt;
using STech.Presentation.Api.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
//using STech.Presentation.Api.BackgroundServices;
using RabbitMQ.Client;
using STech.Presentation.Api.Contracts;
using STech.Presentation.Api.BackgroundServices;
using STech.Presentation.Api.Consumers;
using STech.Presentation.Api.Validations;

namespace STech.Presentation.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            HostingEnvironment = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var adminApiConfiguration = Configuration.GetSection(nameof(AdminApiConfiguration)).Get<AdminApiConfiguration>();
            services.AddSingleton(adminApiConfiguration);


            //SQL Server 
            services/*.AddEntityFrameworkSqlServer()*/.AddDbContext<PortEmployeesDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"), 
                        options => options.EnableRetryOnFailure());
                    options.EnableSensitiveDataLogging(true);
                }, ServiceLifetime.Scoped
            )
             .AddUnitOfWork<PortEmployeesDbContext>(); // Note the App prefix 



            //PostgreSQL 
            //services.AddDbContext<SmartCattleContext>(options => options.UseNpgsql(Configuration.GetConnectionString("PostgreSqlConnection")))
            //.AddUnitOfWork<PortEmployeesDbContext>(); // Note the App prefix 

            services.AddSingleton<AccountNumberValidation>();

            //enabling in memory caching
            //http://www.binaryintellect.net/articles/a7d9edfd-1f86-45f8-a668-64cc86d8e248.aspx
            services.AddMemoryCache();


            services.AddOptions();


            //Configure RabbitMQ and MassTransit
            var rabbitMqConfiguration = Configuration.GetSection("RabbitMqConfiguration").Get<RabbitMqConfiguration>();
            services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMqConfiguration"));
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                //Add a single consumer
                x.AddConsumer<EmployeeCreationConsumer>(typeof(EmployeeCreationConsumerDefinition));

                //// Add a single consumer by type
                //x.AddConsumer(typeof(SubmitOrderConsumer), typeof(SubmitOrderConsumerDefinition));

                //// Add all consumers in the specified assembly
                //x.AddConsumers(typeof(SubmitOrderConsumer).Assembly);

                //// Add all consumers in the namespace containing the specified type
                //x.AddConsumersFromNamespaceContaining<SubmitOrderConsumer>();


                // Add all consumers in the specified assembly
                var entryAssembly = Assembly.GetEntryAssembly();
                x.AddConsumers(entryAssembly);


                //configure RabbitMQ and MassTrasit
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqConfiguration.Host, rabbitMqConfiguration.Port, rabbitMqConfiguration.VirtualHost, h =>
                    {
                        h.Username(rabbitMqConfiguration.UserName);
                        h.Password(rabbitMqConfiguration.Password);

                    });

                    cfg.MessageTopology.SetEntityNameFormatter(new RabbitMqEntityNameFormatter());
                    cfg.ConfigureEndpoints(context);

                    cfg.Message<EmployeeCreated>(e => e.SetEntityName("Employeecreated")); // Set the name of the exchange

                    //cfg.Send<EmployeeCreated>(e =>
                    //{
                    //    // use ride type for the routing key
                    //    e.UseRoutingKeyFormatter(context => "banti.Employeecreated"); // route by ride type
                    //    e.UseCorrelationId(context => context.Id);
                    //});



                    //cfg.Publish<EmployeeCreated>(e =>
                    //{
                    //    //e.Durable = true;
                    //    //e.AutoDelete = false;
                    //    e.ExchangeType = ExchangeType.Direct;
                    //    //e.BindQueue("EmployeeCreatedEvents", "EmployeeCreatedEvents-Queue");
                    //}); // exchange type




                    //cfg.ReceiveEndpoint("EmployeeCreationEvents-Queue", x =>
                    //{
                    //    x.ConfigureConsumeTopology = false;
                    //    x.ClearMessageDeserializers();
                    //    x.UseRawJsonSerializer();
                    //    x.ConfigureConsumer<EmployeeCreatedConsumer>(context);

                    //    //x.Lazy = true;

                    //    //x.Bind("EmployeeCreatedAkedEvents", s =>
                    //    //{
                    //    //    //s.RoutingKey = "banti.Employeecreated";
                    //    //    s.ExchangeType = ExchangeType.Direct;
                    //    //});
                    //});





                });

            });



            ////Enable this code in case to send messages to MabbitMQ by using masstransit
            ////Register CreateEmployeeBackgroundService
            //services.AddSingleton(_ => new CreateEmployeeBackgroundService(
            //          _.GetRequiredService<IServiceScopeFactory>(),
            //          _.GetRequiredService<IBusControl>(),
            //          _.GetRequiredService<ILogger<CreateEmployeeBackgroundService>>())
            //        );

            //services.AddHostedService<BackgroundServiceStarter<CreateEmployeeBackgroundService>>();







            //Add SignalR
            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
            });





            //Configure Http Client to make requests to external urls
            services.AddHttpClient();
            services.AddHttpClient("ExternalService", c =>
            {
                c.BaseAddress = new Uri(Configuration.GetValue<string>("ExternalServiceConfiguration:BaseAddress"));
                //c.DefaultRequestHeaders.Add("Authorization", "bearer " + Configuration.GetSection("appSettings").GetSection("AccessToken").Value);
                //c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                //c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
                //c.DefaultRequestHeaders.Add("Content-Type", "application/form-data");
            });


            services.AddHttpContextAccessor();
            services.AddAdminApiCors(adminApiConfiguration);

            //services.AddDataProtection()
            //    .SetApplicationName("Skoruba.IdentityServer4")
            //    .PersistKeysToDbContext<IdentityServerDataProtectionDbContext>();



            // Add email senders which is currently setup for SendGrid and SMTP
            services.AddEmailSenders(Configuration);



            //services.AddScoped<ControllerExceptionFilterAttribute>();

            //// Add authentication services
            //RegisterAuthentication(services);


            //Configure Redis as distributed cashe.
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{Configuration.GetValue<string>("RedisCache:Connection")},password={Configuration.GetValue<string>("RedisCache:Password")}";
                options.InstanceName = $"{Configuration.GetValue<string>("RedisCache:InstanceName")}";
            });



            //Configuer Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = adminApiConfiguration.IdentityServerBaseUrl;
                    options.RequireHttpsMetadata = adminApiConfiguration.RequireHttpsMetadata;
                    options.Audience = adminApiConfiguration.OidcApiName;
                });

            // Add authorization services
            RegisterAuthorization(services);

        

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddControllers().AddDataAnnotationsLocalization(options => {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(SharedResource));
            }).AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );


            //Configure URL based versioning 
            //for another type of versioning refere to https://www.c-sharpcorner.com/article/api-versioning-in-asp-net-core-web-api/
            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
                x.ApiVersionReader = new UrlSegmentApiVersionReader();
                x.UseApiBehavior = true;
            });


            //Configuer API explorer
            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "VV";
                setup.SubstitutionFormat = "VV";
                setup.SubstituteApiVersionInUrl = true;
            });



            //Configuer swagger
            services.AddSwaggerGen(options =>
            {

                foreach (var apiVersion in adminApiConfiguration.ApiVersions)
                {
                    options.SwaggerDoc(
                           apiVersion.Version,
                           new OpenApiInfo
                           {
                               Title = adminApiConfiguration.ApiName,
                               Version = apiVersion.Version,
                               Description = apiVersion.Description,
                               //,TermsOfService = apiVersion.TermsOfService 
                               Contact = new OpenApiContact
                               {
                                   Name = "STech Technology",
                                   Email = "koursamer@gmail.com",
                                   //Url = new Uri("http://STech.ir/"),
                               },
                               License = new OpenApiLicense
                               {
                                   Name = "Employee API",
                                   //Url = new Uri("http://STech.ir/license"),
                               }
                           }
                    );

                    // Set the comments path for the Swagger JSON and UI.
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);

                }

                options.OperationFilter<AuthorizeCheckOperationFilter>();
                options.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
                options.EnableAnnotations();
                options.DocInclusionPredicate((version, desc) =>
                {
                    var versions = desc.CustomAttributes()
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    var maps = desc.CustomAttributes()
                        .OfType<MapToApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions)
                        .ToArray();

                    return versions.Any(v => $"v{v}" == version) && (maps.Length == 0 || maps.Any(v => $"v{v}" == version));
                });


                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,

                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {

                            AuthorizationUrl = new Uri($"{adminApiConfiguration.IdentityServerBaseUrl}/connect/authorize"),
                            TokenUrl = new Uri($"{adminApiConfiguration.IdentityServerBaseUrl}/connect/token"),
                            Scopes = new Dictionary<string, string> {
                                { adminApiConfiguration.OidcApiName, adminApiConfiguration.ApiName }
                            },
                        }
                    }
                });


            });

            //services.AddAuditEventLogging<AdminAuditLogDbContext, AuditLog>(Configuration);

            //services.AddIdSHealthChecks<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminIdentityDbContext, AdminLogDbContext, AdminAuditLogDbContext, IdentityServerDataProtectionDbContext>(Configuration, adminApiConfiguration);


        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AdminApiConfiguration adminApiConfiguration, IMemoryCache cache)
        {
            //app.AddForwardHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //c.SwaggerEndpoint(
                //$"{adminApiConfiguration.ApiBaseUrl}/swagger/v1/swagger.json", adminApiConfiguration.ApiName);
                foreach (var apiVersion in adminApiConfiguration.ApiVersions)
                {
                    c.SwaggerEndpoint(
                        $"{adminApiConfiguration.ApiBaseUrl}/swagger/{apiVersion.Version}/swagger.json", $"{apiVersion.Version}"
                        );
                }


                c.OAuthClientId(adminApiConfiguration.OidcSwaggerUIClientId);
                c.OAuthAppName(adminApiConfiguration.ApiName);
                //c.OAuthConfigObject.UsePkceWithAuthorizationCodeGrant = false;
                c.OAuthUsePkce();

            });




            var supportedCultures = new[] { "en-US", "fa-IR" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
            app.UseRequestLocalization(localizationOptions);




            //Refere to https://vmsdurano.com/autowrapper-prettify-your-asp-net-core-apis-with-meaningful-responses/
            // User https://github.com/proudmonkey/AutoWrapper Instead
            app.UseApiResponseAndExceptionWrapper(
                new AutoWrapperOptions
                {
                    ShowApiVersion = true,
                    ShowStatusCode = true,
                    IsApiOnly = true
                });

            app.UseRouting();
            app.UseAuthentication();
            app.UseCors();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                //endpoints.MapHub<Employee.ApiHub>("/hubs/Employee.Api");
                //endpoints.MapHealthChecks("/health", new HealthCheckOptions
                //{
                //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                //});
            });
        }


        public class ReplaceVersionWithExactValueInPathFilter : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                var paths = new OpenApiPaths();
                foreach (var path in swaggerDoc.Paths)
                {
                    paths.Add(path.Key.Replace("v{version}", swaggerDoc.Info.Version), path.Value);
                }
                swaggerDoc.Paths = paths;
            }
        }

        //public virtual void RegisterDbContexts(IServiceCollection services)
        //{
        //    services.AddDbContexts<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminLogDbContext, AdminAuditLogDbContext, IdentityServerDataProtectionDbContext>(Configuration);
        //}

        //public virtual void RegisterAuthentication(IServiceCollection services)
        //{
        //    services.AddApiAuthentication<AdminIdentityDbContext, UserIdentity, UserIdentityRole>(Configuration);
        //}

        public virtual void RegisterAuthorization(IServiceCollection services)
        {
            services.AddAuthorizationPolicies();
        }

        public virtual void UseAuthentication(IApplicationBuilder app)
        {
            app.UseAuthentication();
        }
    }
}

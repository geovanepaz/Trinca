using System.Globalization;
using System.IO;
using Api.Configuration;
using Api.Filters;
using App.AutoMappers;
using AutoMapper;
using Core.AppSettings;
using Cross.IoC;
using FluentValidation.AspNetCore;
using Infra.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace Api
{
    public class Startup
    {
        private readonly bool _swaggerEnable;
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            _swaggerEnable = Configuration.GetSection("Swagger").Get<SwaggerSettings>().Ativo;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //mesma coisa que o MachineKey do .net framework
            services.AddDataProtection();

            services.AddWebApi(o => { o.OutputFormatters.Remove(new XmlDataContractSerializerOutputFormatter()); });

            services
                .AddMvc(o =>
                {
                    o.Filters.Add(typeof(ValidateModelAttribute));
                    o.Filters.Add(typeof(ApiExceptionFilter));
                })
                .AddJsonOptions(o => o.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .ConfigureApiBehaviorOptions(o => { o.SuppressModelStateInvalidFilter = true; })
                .AddFluentValidation(o => o.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.Configure<ApiBehaviorOptions>(options => options.SuppressInferBindingSourcesForParameters = true);

            services.AddCors();

            services.AddVersionedApiExplorer(o => { o.GroupNameFormat = "'v'VVV"; });

            AutoMapperConfig.RegisterMappings();

            services.AddAutoMapper();

            services.AddApiVersioning();

            if (_swaggerEnable)
            {
                services.AddSwaggerGen(c =>
                {
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        c.SwaggerDoc(description.GroupName, new Info
                        {
                            Title = $"Trinca Api {description.ApiVersion}",
                            Version = description.ApiVersion.ToString()
                        });
                    }
                });
            }

            RegisterServices(services);

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IHttpContextAccessor accessor, IApiVersionDescriptionProvider provider)
        {
            // Definindo a cultura padrão: pt-BR
            var supportedCultures = new[] { new CultureInfo("pt-BR") };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR", "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            if (_swaggerEnable)
            {
                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());

                        options.DefaultModelExpandDepth(0);
                        options.DefaultModelsExpandDepth(-1);
                    }
                });
            }

            app.UseMvc();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            NativeInjectorBootStrapper.RegisterServices(services);
        }
    }
}
using System;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Voa.App.AutoMappers;
using Voa.Core.AppSettings;
using Voa.Core.Filters;
using Voa.Core.Interfaces.Services;
using Voa.Infra.Contexts;

namespace Voa.Robo
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<VoaContext>(opt => opt.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));

            services
                .AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseRedisStorage(Configuration.GetConnectionString("Redis")));

            services.AddHangfireServer();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile<DomainToViewModelMappingProfile>();
                mc.AddProfile<ViewModelToDomainMappingProfile>();
            });

            var mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            GlobalJobFilters.Filters.Add(new ExpirationTimeHangfireAttribute());
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 }); // Se der erro, não tenta novamente

            GlobalConfiguration.Configuration.UseBatches(TimeSpan.FromDays(1));

            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {
                Authorization = new[] { new NonAuthorizationFilter() }
            });

            // Uso de Crontabs - https://crontab.guru

            if (Configuration.GetSection("JobConfig:Concorrente").Get<JobConfig>().Habilitado)
            {
                RecurringJob.AddOrUpdate<IRoboService>("Concorrente_3", o => o.JobConcorrentes(), Cron.Daily(3, 00), TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IRoboService>("Concorrente_12", o => o.JobConcorrentes(), Cron.Daily(12, 00), TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IRoboService>("Concorrente_20", o => o.JobConcorrentes(), Cron.Daily(20, 00), TimeZoneInfo.Local);
            }

            if (Configuration.GetSection("JobConfig:Localizacao").Get<JobConfig>().Habilitado)
            {
                RecurringJob.AddOrUpdate<IRoboService>("Localizacao_4", o => o.JobLocalizacoes(), Cron.Daily(4, 00), TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IRoboService>("Localizacao_13", o => o.JobLocalizacoes(), Cron.Daily(13, 00), TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IRoboService>("Localizacao_21", o => o.JobLocalizacoes(), Cron.Daily(21, 00), TimeZoneInfo.Local);
            }

            if (Configuration.GetSection("JobConfig:Hashtag").Get<JobConfig>().Habilitado)
            {
                RecurringJob.AddOrUpdate<IRoboService>("Hashtag_5", o => o.JobHashtag(), Cron.Daily(5, 00), TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IRoboService>("Hashtag_14", o => o.JobHashtag(), Cron.Daily(14, 00), TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IRoboService>("Hashtag_22", o => o.JobHashtag(), Cron.Daily(22, 00), TimeZoneInfo.Local);
            }

            if (Configuration.GetSection("JobConfig:Seguidores").Get<JobConfig>().Habilitado)
            {
                RecurringJob.AddOrUpdate<IRoboService>("Seguidores_6", o => o.JobSeguidores(), Cron.Daily(6, 00), TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IRoboService>("Seguidores_15", o => o.JobSeguidores(), Cron.Daily(15, 00), TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IRoboService>("Seguidores_23", o => o.JobSeguidores(), Cron.Daily(23, 00), TimeZoneInfo.Local);
            }

            if (Configuration.GetSection("JobConfig:Seguindo").Get<JobConfig>().Habilitado)
            {
                RecurringJob.AddOrUpdate<IRoboService>("Seguindo_7", o => o.JobSeguindo(), Cron.Daily(7, 00), TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IRoboService>("Seguindo_16", o => o.JobSeguindo(), Cron.Daily(16, 00), TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IRoboService>("Seguindo_0", o => o.JobSeguindo(), Cron.Daily(0, 00), TimeZoneInfo.Local);
            }

            if (Configuration.GetSection("JobConfig:PararSeguir").Get<JobConfig>().Habilitado)
            {
                RecurringJob.RemoveIfExists("PararSeguir");
                RecurringJob.AddOrUpdate<IRoboService>("PararSeguir_8", o => o.JobPararSeguir(), Cron.Daily(8, 00), TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IRoboService>("PararSeguir_17", o => o.JobPararSeguir(), Cron.Daily(17, 00), TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IRoboService>("PararSeguir_1", o => o.JobPararSeguir(), Cron.Daily(1, 00), TimeZoneInfo.Local);
            }

            if (Configuration.GetSection("JobConfig:Postagem").Get<JobConfig>().Habilitado)
            {
                RecurringJob.RemoveIfExists("Postagem");
                RecurringJob.AddOrUpdate<IRoboService>("Postagem_9", o => o.JobPostagem(), Cron.Daily(9, 00), TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IRoboService>("Postagem_18", o => o.JobPostagem(), Cron.Daily(18, 00), TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IRoboService>("Postagem_2", o => o.JobPostagem(), Cron.Daily(2, 00), TimeZoneInfo.Local);
            }

            if (Configuration.GetSection("JobConfig:Chatbot").Get<JobConfig>().Habilitado)
            {
                RecurringJob.AddOrUpdate<IRoboService>("ChatbotCriarInbox", o => o.JobChatbotCriarInbox(),
                    Cron.MinuteInterval(Configuration.GetSection("JobConfig:Chatbot").Get<JobConfig>().IntervaloEmMinutos), TimeZoneInfo.Local);

                RecurringJob.AddOrUpdate<IRoboService>("ChatbotProcessarInbox", o => o.JobChatbotProcessarInbox(),
                    Cron.MinuteInterval(Configuration.GetSection("JobConfig:Chatbot").Get<JobConfig>().IntervaloEmMinutos), TimeZoneInfo.Local);
            }
        }
    }
}
using App.Interfaces;
using App.Services;
using AutoMapper;
using Core.Interfaces.Providers;
using Core.Interfaces.Repositories.Dapper;
using Core.Interfaces.Repositories.Sql;
using Core.Interfaces.Services;
using Core.Providers;
using Core.Services;
using Core.Validations.ViewModels.Evento;
using Core.Validations.ViewModels.Participante;
using Core.Validations.ViewModels.Usuario;
using Core.Validations.ViewModels.ValorParticipante;
using Core.ViewModels.Evento;
using Core.ViewModels.Evento.ValorParticipante;
using Core.ViewModels.Participante;
using Core.ViewModels.Usuario;
using FluentValidation;
using Infra.Contexts;
using Infra.Repositories.Dapper;
using Infra.Repositories.Sql;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Cross.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Dependências Microsoft
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Aplicação
            services.AddTransient<IParticipanteAppService, ParticipanteAppService>();
            services.AddTransient<IUsuarioAppService, UsuarioAppService>();
            services.AddTransient<IEventoAppService, EventoAppService>();

            services.AddScoped<IMapper>(o => new Mapper(o.GetRequiredService<IConfigurationProvider>(), o.GetService));

            // Validação
            services.AddScoped<IValidator<ValorParticipanteRequest>, ValorParticipanteValidator>();
            services.AddScoped<IValidator<ParticipanteRequest>, ParticipanteValidator>();
            services.AddScoped<IValidator<UsuarioRequest>, UsuarioValidator>();
            services.AddScoped<IValidator<EventoRequest>, EventoValidator>();

            // Domínio
            services.AddTransient<IEventoParticipante, EventoParticipanteService>();
            services.AddTransient<IEventoService, EventoService>();
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IParticipanteService, ParticipanteService>();
            services.AddTransient<ILogService, LogService>();

            services.AddTransient<IExcecaoService, ExcecaoService>();

            // Infra - Contextos, Helpers
            services.AddTransient<ITrincaContext, TrincaContext>();
            services.AddTransient<ITrincaLogContext, TrincaLogContext>();

            services.AddTransient<ISqlHelper, SqlHelper>();

            // Infra - Repositório Sql
            services.AddTransient<IValorParticipanteRepository, ValorParticipanteRepository>();
            services.AddTransient<IEventoRepository, EventoRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IParticipanteRepository, ParticipanteRepository>();
            services.AddTransient<ILogRepository, LogRepository>();

            // Infra - Repositório NoSql

            // Provider
            services.AddSingleton(typeof(IProvider<>), typeof(Provider<>));
        }
    }
}
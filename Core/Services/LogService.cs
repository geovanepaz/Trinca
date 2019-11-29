using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Sql;
using Core.Interfaces.Repositories.Sql;
using Core.Interfaces.Services;
using Cross.Util.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Core.Services
{
    public class LogService : ILogService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public LogService(IServiceScopeFactory serviceScopeFactory) => _serviceScopeFactory = serviceScopeFactory;

        public void Error(Exception exception, string message, object requestData = null)
        {
            Task.Factory.StartNew(() => ErrorAync(exception, message, requestData));
        }

        public void Warning(string message, object requestData = null)
        {
            Task.Factory.StartNew(() => WarningAsync(message, requestData));
        }

        private void ErrorAync(Exception exception, string message, object requestData = null)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var conteudo = new StringBuilder();

                if (requestData != null)
                {
                    conteudo.Append($"--- ENTRADA ---{Environment.NewLine}{requestData.GetType()} = {JsonConvert.SerializeObject(requestData)}{Environment.NewLine}");
                }

                conteudo.Append($"--- EVENTO ---{Environment.NewLine}{exception.Demystify().ToString().FirstFromSplit("--- End of inner exception stack trace ---").OnlyAscii()}");

                var log = new Log
                {
                    Application = "API_CHURRASCO",
                    ExceptionMessage = conteudo.ToString(),
                    Loglevel = "Error",
                    Message = message,
                    Tracetime = DateTime.Now,
                    Username = Environment.UserName,
                    Machinename = Environment.MachineName
                };

                // ServiceLocator
                var service = scope.ServiceProvider.GetService<ILogRepository>();

                service.Insert(log);
            }
        }

        private void WarningAsync(string message, object requestData = null)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var conteudo = new StringBuilder();

                if (requestData != null)
                {
                    conteudo.Append($"--- ENTRADA ---{Environment.NewLine}{requestData.GetType()} = {JsonConvert.SerializeObject(requestData)}{Environment.NewLine}");
                }

                conteudo.Append($"--- EVENTO ---{Environment.NewLine}{message.OnlyAscii()}");

                var log = new Log
                {
                    Application = "API_CHURRASCO",
                    ExceptionMessage = conteudo.ToString(),
                    Loglevel = "Warning",
                    Message = "RECURSO INDISPONIVEL OU DESLIGADO",
                    Tracetime = DateTime.Now,
                    Username = Environment.UserName,
                    Machinename = Environment.MachineName
                };

                // ServiceLocator
                var service = scope.ServiceProvider.GetService<ILogRepository>();

                service.Insert(log);
            }
        }
    }
}
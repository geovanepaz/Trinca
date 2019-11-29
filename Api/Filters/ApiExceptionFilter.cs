using System;
using System.Net;
using Core.Exceptions;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class ApiExceptionFilter : Attribute, IExceptionFilter
    {
        private readonly ILogService _logService;

        public ApiExceptionFilter(ILogService logService) => _logService = logService;

        public void OnException(ExceptionContext context)
        {
            if (!(context.Exception is Exception))
            {
                return;
            }

            switch (context.Exception)
            {
                case ServiceUnavailableException e:

                    _logService.Warning(context.Exception.Message, e.Arguments);

                    context.Result = new ObjectResult(new { Message = "A sua solicitação não pôde ser concluída. Por favor, tente novamente." }) { StatusCode = (int)HttpStatusCode.ServiceUnavailable };

                    break;

                case NotFoundException e:

                    _logService.Warning(context.Exception.Message, e.Arguments);

                    context.Result = new ObjectResult("") { StatusCode = (int)HttpStatusCode.NotFound };

                    break;

                case InternalErrorException e:

                    _logService.Error(context.Exception, context.Exception.Message, e.Arguments);

                    context.Result = new ObjectResult(new { Message = "Ocorreu uma instabilidade no sistema, mas já estamos resolvendo." }) { StatusCode = (int)HttpStatusCode.InternalServerError };

                    break;

                case UnauthorizedException _:

                    context.Result = new ObjectResult(new { context.Exception.Message }) { StatusCode = (int)HttpStatusCode.Unauthorized };

                    break;

                case Exception _:

                    _logService.Error(context.Exception, context.Exception.Message);

                    context.Result = new ObjectResult(new { Message = "Ocorreu uma instabilidade no sistema, mas já estamos resolvendo." }) { StatusCode = (int)HttpStatusCode.InternalServerError };

                    break;
            }

            context.Exception = null;
        }
    }
}
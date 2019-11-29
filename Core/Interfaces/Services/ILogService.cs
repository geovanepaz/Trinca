using System;

namespace Core.Interfaces.Services
{
    public interface ILogService
    {
        void Error(Exception exception, string message, object requestData = null);
        void Warning(string message, object requestData = null);
    }
}
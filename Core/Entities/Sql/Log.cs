using System;
using Core.Interfaces.Entities;

namespace Core.Entities.Sql
{
    public class Log : EntityBase<int>
    {
        public string Application { get; set; }
        public DateTime Tracetime { get; set; }
        public string Loglevel { get; set; }
        public string Message { get; set; }
        public string Machinename { get; set; }
        public string Username { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
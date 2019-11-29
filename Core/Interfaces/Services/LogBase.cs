using System;
using System.IO;

namespace Core.Interfaces.Services
{
    public abstract class LogBase
    {
        private string _diretorio;

        private static string DiretorioLogs => Path.Combine(Environment.CurrentDirectory, "Logs");

        public string Diretorio
        {
            get => _diretorio;
            set => _diretorio = Path.Combine(DiretorioLogs, value);
        }

        public string Arquivo(string info)
        {
            if (string.IsNullOrEmpty(Diretorio))
            {
                throw new ArgumentException("Diretório não definido");
            }

            if (!Directory.Exists(Diretorio))
                Directory.CreateDirectory(Diretorio);

            var path = Path.Combine(Diretorio, "voaapi_" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt");

            var id = new Random().Next(1, 999999999).ToString("X10");

            var format = $@"<LOG>{DateTime.Now} - {id}{Environment.NewLine}{info}</LOG>{Environment.NewLine}";

            try
            {
                File.AppendAllText(path, format);
                return id;
            }
            catch
            {
                return id;
            }
        }
    }
}
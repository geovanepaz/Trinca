using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Core.Interfaces.Repositories.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Infra.Repositories.Dapper
{
    public class SqlHelper : ISqlHelper
    {
        private readonly IDbConnection _connection;

        public SqlHelper(IConfiguration configuration, string contexto) => _connection = new SqlConnection(configuration.GetSection($"ConnectionStrings:{contexto}").Value);

        public int Exec(string storedProcedure, object param)
        {
            using (var conn = _connection)
            {
                conn.Open();
                var retorno = conn.Query<int>(storedProcedure, param, commandType: CommandType.StoredProcedure).First();
                return retorno;
            }
        }

        IEnumerable<T> ISqlHelper.Exec<T>(string storedProcedure, object param)
        {
            using (var conn = _connection)
            {
                conn.Open();
                var retorno = conn.Query<T>(storedProcedure, param, commandType: CommandType.StoredProcedure);
                return retorno;
            }
        }

        IEnumerable<T> ISqlHelper.Func<T>(string userFunction, object param)
        {
            using (var conn = _connection)
            {
                conn.Open();
                var parametros = string.Join(",", param.GetType().GetProperties().Select(x => x.GetValue(param, null)).ToArray());
                var query = string.Concat("SELECT * FROM ", userFunction, "(", parametros, ")");
                var retorno = conn.Query<T>(query, param);
                return retorno;
            }
        }

        IEnumerable<T> ISqlHelper.Query<T>(string query, object param)
        {
            using (var conn = _connection)
            {
                conn.Open();
                var retorno = conn.Query<T>(query, param);
                return retorno;
            }
        }

        T ISqlHelper.QuerySingle<T>(string query, object param = null)
        {
            using (var conn = _connection)
            {
                conn.Open();
                var retorno = conn.QueryFirstOrDefault<T>(query, param);
                return retorno;
            }
        }

        int ISqlHelper.Insert(string query, object param)
        {
            using (var conn = _connection)
            {
                conn.Open();
                var retorno = conn.Execute(query, param);
                return retorno;
            }
        }

        IEnumerable<TReturn> ISqlHelper.Query<TFirst, TSecond, TReturn>(string query, Func<TFirst, TSecond, TReturn> map, string splitOn, object param)
        {
            using (var conn = _connection)
            {
                conn.Open();
                var retorno = conn.Query(query, map, param, splitOn: splitOn);
                return retorno;
            }
        }

        IEnumerable<TReturn> ISqlHelper.Query<TFirst, TSecond, TReturn>(string query, Func<TFirst, TSecond, TReturn> map, object param)
        {
            using (var conn = _connection)
            {
                conn.Open();
                var retorno = conn.Query(query, map, param);
                return retorno;
            }
        }
    }
}
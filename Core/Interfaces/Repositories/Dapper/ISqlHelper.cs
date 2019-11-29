using System;
using System.Collections.Generic;

namespace Core.Interfaces.Repositories.Dapper
{
    public interface ISqlHelper
    {
        int Exec(string storedProcedure, object param = null);
        IEnumerable<T> Exec<T>(string storedProcedure, object param = null) where T : class;
        IEnumerable<T> Func<T>(string userFunction, object param) where T : class;
        IEnumerable<T> Query<T>(string query, object param = null) where T : class;
        IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string query, Func<TFirst, TSecond, TReturn> map, string splitOn, object param = null);
        IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string query, Func<TFirst, TSecond, TReturn> map, object param);
        T QuerySingle<T>(string query, object param = null);
        int Insert(string query, object param);
    }
}
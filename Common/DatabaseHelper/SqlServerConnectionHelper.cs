using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;   

namespace Common.DatabaseHelper
{
    public class SqlServerConnectionHelper : IDatabaseConnectionHelper
    {
        private readonly string _connectionString;

        public SqlServerConnectionHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string query, object param, CommandType commandType)
        {
            IEnumerable<TReturn> rows = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(); // Open connection first
                rows = await connection.QueryAsync<TReturn>(query, param, commandType: commandType); // Execute query
            }
            return rows;
        }
    }
}

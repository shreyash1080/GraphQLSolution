using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DatabaseHelper
{
    public interface IDatabaseConnectionHelper
    {

        Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string sqlQuery, object param,CommandType commandType);
    }
}

using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Diagnostics;

namespace Infrastructure.Repositories.Base
{
    public class SqlLoggingInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
        {
            LogSql(command);
            return base.NonQueryExecuting(command, eventData, result);
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            LogSql(command);
            return base.ReaderExecuting(command, eventData, result);
        }

        public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
        {
            LogSql(command);
            return base.ScalarExecuting(command, eventData, result);
        }

        private void LogSql(DbCommand command)
        {
            Debug.WriteLine(command.CommandText);
            foreach (DbParameter parameter in command.Parameters)
            {
                Debug.WriteLine($"{parameter.ParameterName}: {parameter.Value}");
            }
        }
    }
}

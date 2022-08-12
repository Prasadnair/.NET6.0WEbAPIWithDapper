using APIWithDapperTutorial.Data.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace APIWithDapperTutorial.Data.Context
{
    public class SchoolContext
    {
        private ConnectionStringOptions connectionStringOptions;

        public SchoolContext(IOptionsMonitor<ConnectionStringOptions> optionsMonitor)
        {
            connectionStringOptions=optionsMonitor.CurrentValue;
        }
        public IDbConnection CreateConnection() => new SqlConnection(connectionStringOptions.SqlConnection);
    }
}

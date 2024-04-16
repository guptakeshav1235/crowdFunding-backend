using DataStore.Abstraction.Utilities;
using System.Data;
using System.Data.SqlClient;

namespace DataStore.Implementation.Utilities
{
    public class DapperContext:IDapperContext
    {
        public IDbConnection CreateConnection()
            => new SqlConnection("Server=SS-KESHAVGUPTA\\SQLSERVER;Database=crowdFunding;Trusted_Connection=true");
    }
}

/*using DataStore.Abstraction.Utilities;
using System.Data;
using System.Data.SqlClient;

namespace DataStore.Implementation.Utilities
{
    public class DapperContext:IDapperContext
    {
        public IDbConnection CreateConnection()
            => new SqlConnection("Server=SS-KESHAVGUPTA\\SQLSERVER;Database=crowdFunding;Trusted_Connection=true");
    }
}*/

using DataStore.Abstraction.Utilities;
using System.Data;
using System.Data.SqlClient;

namespace DataStore.Implementation.Utilities
{
    public class DapperContext : IDapperContext
    {
        public IDbConnection CreateConnection()
            => new SqlConnection("Server=mydatabase.cdwe6smaqz79.ap-south-1.rds.amazonaws.com;Database=crowdFunding;User Id=Keshav;Password=demonslayer123$%#;");
    }
}
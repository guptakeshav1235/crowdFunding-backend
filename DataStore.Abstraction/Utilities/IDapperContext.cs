using System.Data;

namespace DataStore.Abstraction.Utilities
{
   public interface IDapperContext
    {
        public IDbConnection CreateConnection();
    }
}

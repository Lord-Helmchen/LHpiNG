using LHpiNG.Cardmarket;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.db
{
    public class SQLContext : EFContext 
    {
        public SQLContext() : base()
        {
            System.Data.Entity.Database.SetInitializer<SQLContext>(new DropCreateDatabaseIfModelChanges<SQLContext>());
        }

        public SQLContext(String connectionString) : base(connectionString)
        {
            System.Data.Entity.Database.SetInitializer<SQLContext>(new CreateDatabaseIfNotExists<SQLContext>());
        }


    }
}

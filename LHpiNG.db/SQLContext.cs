using LHpiNG.Cardmarket;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.db
{
    [Obsolete]
    public class SQLContext : DbContext, ILHpiDatabase
    {
        public SQLContext() : base()
        {
            Database.SetInitializer<SQLContext>(new CreateDatabaseIfNotExists<SQLContext>());
        }

        public SQLContext(String connectionString): base(connectionString)
        {
            Database.SetInitializer<SQLContext>(new CreateDatabaseIfNotExists<SQLContext>());
        }

        public DbSet<Expansion> Expansions { get; set; }
        public DbSet<Product> Producs { get; set; }
        public DbSet<PriceGuide> PriceGuides { get; set; }

        // ILHpiDatabase methods
        #region Cardmarket
        public ExpansionList LoadExpansionList()
        {
            throw new NotImplementedException();
        }
        public void SaveExpansionList(ExpansionList expansionList)
        {
            throw new NotImplementedException();
        }
        public Expansion LoadExpansion(Expansion expansion)
        {
            throw new NotImplementedException();
        }
        public void SaveExpansion(Expansion expansion)
        {
            throw new NotImplementedException();
        }
        public Product LoadProduct(Product product)
        {
            throw new NotImplementedException();
        }
        public void SaveProduct(Product product)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Album

        #endregion

    }
}

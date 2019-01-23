using LHpiNG.Cardmarket;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.CodeFirst;

namespace LHpiNG.db
{
    public class SQLContext : DbContext
    {
        public SQLContext(): base()
        {

        }

        public DbSet<ExpansionEntity> ExpansionEntities { get; set; }
        public DbSet<ProductEntity> ProductEntities { get; set; }
        public DbSet<CategoryEntity> CategoryEntities { get; set; }
        public DbSet<LocalizationEntity> LocalizationEntities { get; set; }
        public DbSet<PriceGuideEntity> PriceGuideEntities { get; set; }
        public DbSet<ReprintEntity> ReprintEntities { get; set; }
    }
}

using LHpiNG.Cardmarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LHpiNG.db
{
    public interface  ILHpiDatabase
    {
        //declare getters and setters as part of the interface to force me to remember to implement the fields
        DbSet<Expansion> Expansions { get; set; }
        DbSet<Product> Producs { get; set; }
        DbSet<PriceGuide> PriceGuides { get; set; }
        //public DbSet<CategoryEntity> CategoryEntities { get; set; }
        //public DbSet<LocalizationEntity> LocalizationEntities { get; set; }
        //public DbSet<ReprintEntity> ReprintEntities { get; set; }

        #region Cardmarket
        ExpansionList LoadExpansionList();
        void SaveExpansionList(ExpansionList expansionList);
        Expansion LoadExpansion(Expansion expansion);
        void SaveExpansion(Expansion expansion);
        Product LoadProduct(Product product);
        void SaveProduct(Product product);
        #endregion

        #region Album

        #endregion
    }
}
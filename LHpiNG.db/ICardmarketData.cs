using LHpiNG.Cardmarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LHpiNG.db
{
    public interface  ICardmarketData
    {
        //declare getters and setters as part of the interface to force me to remember to implement the fields
        DbSet<Expansion> Expansions { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<PriceGuide> PriceGuides { get; set; }
        //public DbSet<CategoryEntity> CategoryEntities { get; set; }
        //public DbSet<LocalizationEntity> LocalizationEntities { get; set; }
        //public DbSet<ReprintEntity> ReprintEntities { get; set; }

        ExpansionList LoadExpansionList();
        void SaveExpansionList(ExpansionList expansionList);
        Expansion LoadExpansion(Expansion expansion);
        void AddOrUpdateExpansion(Expansion expansion);
        IEnumerable<Product> LoadProducts(Expansion expansion);
        void SaveProducts(Expansion expansion);
        Product LoadProduct(Product product);
        void AddOrUpdateProduct(Product product);

    }
}
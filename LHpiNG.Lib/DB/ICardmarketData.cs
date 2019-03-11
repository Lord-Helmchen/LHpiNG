using LHpiNG.Cardmarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LHpiNG.DB
{
    public interface ICardmarketData
    {
        //declare getters and setters as part of the interface to force me to remember to implement the fields
        DbSet<Expansion> Expansions { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<PriceGuide> PriceGuides { get; set; }
        //public DbSet<CategoryEntity> CategoryEntities { get; set; }
        //public DbSet<LocalizationEntity> LocalizationEntities { get; set; }
        //public DbSet<ReprintEntity> ReprintEntities { get; set; }
        DbSet<State> State { get; set; }// holds last fetch date for expansion list

        ExpansionList LoadExpansionList();
        void SaveExpansionList(ExpansionList expansionList);
        void AddOrUpdateExpansion(Expansion expansion);
        void AddOrUpdatePriceGuide(PriceGuide priceGuide);
        void AddOrUpdateProduct(Product product);
    }
}
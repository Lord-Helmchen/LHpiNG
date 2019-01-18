using LHpiNG.Cardmarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Data.SQLite;
using System.Data.Entity;

namespace LHpiNG.db
{
    public abstract class Database
    {

        #region Cardmarket
        public abstract ExpansionList LoadExpansionList();
        public abstract void SaveExpansionList(ExpansionList expansionList);
        public abstract Expansion LoadExpansion(Expansion expansion);
        public abstract void SaveExpansion(Expansion expansion);
        public abstract Product LoadProduct(Product product);
        public abstract void SaveProduct(Product product);
        #endregion

        #region Album

        #endregion
    }
}

using LHpiNG.Cardmarket;
using System;
using System.Data.Entity;

namespace LHpiNG.db
{
    public class SQLDB : LHpiDatabase
    {

        public SQLDB()
        {

        }

        #region Cardmarket
        public override ExpansionList LoadExpansionList()
        {
            throw new NotImplementedException();
        }
        public override void SaveExpansionList(ExpansionList expansionList)
        {
            throw new NotImplementedException();
        }
        public override Expansion LoadExpansion(Expansion expansion)
        {
            throw new NotImplementedException();
        }
        public override void SaveExpansion(Expansion expansion)
        {
            throw new NotImplementedException();
        }
        public override Product LoadProduct(Product product)
        {
            throw new NotImplementedException();
        }
        public override void SaveProduct(Product product)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Album

        #endregion
    }
}

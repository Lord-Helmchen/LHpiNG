using LHpiNG.Cardmarket;
using System.Data.SQLite;
using System;
using System.Data.Entity;

namespace LHpiNG.db
{
    public class SQLiteDB : LHpiDatabase
    {
        public System.Data.SQLite.SQLiteConnection Connection { get; set; }
        internal string DBPath { get; set; }

        public SQLiteDB()
        {
            DBPath = String.Format("{1}\\{0}", "LHpiDB.sqlite", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            ConnectionString = new System.Data.SQLite.SQLiteConnectionStringBuilder()
            {
                DataSource = DBPath,
                ForeignKeys = true,
                FailIfMissing = true,
                Version = 3,

            }.ConnectionString;
            Connection = new SQLiteConnection(ConnectionString);
        }

        public void CreateTables()
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

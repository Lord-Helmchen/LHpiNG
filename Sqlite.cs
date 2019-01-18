using System;
using System.Collections.Generic;
using SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHpiNG.Cardmarket;

namespace LHpiNG.db
{
    public class Sqlite : Database
    {
        public SQLite.SQLiteConnection Connection { get; set; }
        public string ConnectionString { get; set; }
        private string DBPath { get; set; }

        public Sqlite()
        {
            DBPath = String.Format("{1}\\{0}", "LHpiDB.sqlite", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            //ConnectionString = new System.Data.SQLite.SQLiteConnectionStringBuilder() // not used for SQLite-netPCL, needed for System.Data.SQLite and EF6
            //{
            //    DataSource = DBPath,
            //    ForeignKeys = true,
            //    FailIfMissing = true,
            //    Version = 3,

            //}.ConnectionString;
            Connection = new SQLiteConnection(DBPath, SQLiteOpenFlags.ReadWrite);
        }

        public void CreateTables()
        {
            CreateTableResult result;
            result = Connection.CreateTable<ExpansionEntity>();
            result = Connection.CreateTable<ProductEntity>();
            result = Connection.CreateTable<CategoryEntity>();
            result = Connection.CreateTable<LocalizationEntity>();
            result = Connection.CreateTable<PriceGuideEntity>();
            result = Connection.CreateTable<ReprintEntity>();
            result = Connection.CreateTable<Product>();
            result = Connection.CreateTable<Expansion>();


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

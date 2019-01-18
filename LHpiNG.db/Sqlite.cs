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
            Connection.CreateTable<ExpansionEntity>();
            Connection.CreateTable<Expansion>();

        }
    } 
}

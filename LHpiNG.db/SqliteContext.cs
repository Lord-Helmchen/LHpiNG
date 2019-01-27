using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHpiNG.Cardmarket;
using SQLite.CodeFirst;

namespace LHpiNG.db
{
    //TODO compare with https://github.com/msallin/SQLiteCodeFirst/tree/master/SQLite.CodeFirst.Console

    public class SQLiteContext : DbContext, ILHpiDatabase
    {
        //public static string DBPath = String.Format("{1}\\{0}", "LHpiDB.sqlite", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
        public static string DBPath = String.Format("{1}\\{0}", "LHpiDB.sqlite", "D:\\devel\\VisualStudioProjects\\LHpiNG\\LHpiNG\\LHpiNG.db");


        public static string ConnectionString = new SQLiteConnectionStringBuilder
        {
            //DateTimeFormat = SQLiteDateFormats.ISO8601,
            //FailIfMissing = true,
            //ForeignKeys = true,
            DataSource = DBPath
        }.ConnectionString;

        public SQLiteContext() : base()
        {
        }
        public SQLiteContext(String connectionString) : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<SQLiteContext>(modelBuilder);
            var sqliteConnectionInitializer = new SqliteDropCreateDatabaseWhenModelChanges<SQLiteContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }

        public DbSet<Expansion> Expansions { get; set; }
        public DbSet<Product> Producs { get; set; }
        public DbSet<PriceGuide> PriceGuides { get; set; }

        // ILHpiDatabase methods
        #region Cardmarket
        public ExpansionList LoadExpansionList()
        {
            ExpansionList expansionList = new ExpansionList();
            expansionList.Expansions = Expansions.ToList();
            return expansionList;
        }
        public void SaveExpansionList(ExpansionList expansionList)
        {
            Expansions.AddRange(expansionList.Expansions);
            SaveChanges();
        }
        public Expansion LoadExpansion(Expansion expansion)
        {
            throw new NotImplementedException();
        }
        public void SaveExpansion(Expansion expansion)
        {
            Expansions.Add(expansion);
            SaveChanges();
        }
        public Product LoadProduct(Product product)
        {
            throw new NotImplementedException();
        }
        public void SaveProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> LoadProducts(Expansion expansion)
        {
            throw new NotImplementedException();
        }

        public void SaveProducts(Expansion expansion)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Album

        #endregion
    }
}

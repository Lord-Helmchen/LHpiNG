//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using LHpiNG.Cardmarket;

//namespace LHpiNG.db
//{
//    //TODO compare with https://github.com/msallin/SQLiteCodeFirst/tree/master/SQLite.CodeFirst.Console

//    public class SQLiteContext : EFContext
//    {
//        //public static string DBPath = String.Format("{1}\\{0}", "LHpiDB.sqlite", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
//        public static string DBPath = String.Format("{1}\\{0}", "LHpiDB.sqlite", "D:\\devel\\VisualStudioProjects\\LHpiNG\\LHpiNG\\LHpiNG.db");


//        public static string ConnectionString = new SQLiteConnectionStringBuilder
//        {
//            //DateTimeFormat = SQLiteDateFormats.ISO8601,
//            //FailIfMissing = true,
//            //ForeignKeys = true,
//            DataSource = DBPath
//        }.ConnectionString;

//        public SQLiteContext() : base()
//        {
//        }
//        public SQLiteContext(String connectionString) : base(connectionString)
//        {
//        }

//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            //var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<SQLiteContext>(modelBuilder);
//            var sqliteConnectionInitializer = new SqliteDropCreateDatabaseWhenModelChanges<SQLiteContext>(modelBuilder);
//            Database.SetInitializer(sqliteConnectionInitializer);
//        }
        
//    }
//}

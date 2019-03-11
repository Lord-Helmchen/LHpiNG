using LHpiNG.Cardmarket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.DB
{
    public class SQLContext : EFContext 
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LHpi;Integrated Security=True;");
            base.OnConfiguring(optionsBuilder);
        }

    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace LHpiNG.Cardmarket
{
    public class ReprintEntity
    {
        public int IdProduct { get; set; }
        public string Expansion { get; set; }
        public int ExpIcon { get; set; }

        [Key]
        public int Uid { get; set; }                // Entity Framework Primary Key

        public ReprintEntity()
        {

        }
        //constructor from API reply
        public ReprintEntity(string jsonEntity)
        {
            throw new NotImplementedException();
        }
    }
}

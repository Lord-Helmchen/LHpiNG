using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHpiNG.Cardmarket
{
    [NotMapped]
    public class CategoryEntity
    {
        [Key]
        public int IdCategory { get; set; }         // Category ID
        public string CategoryName { get; set; }    // Category's name

        public CategoryEntity()
        {

        }
        //constructor from API reply
        public CategoryEntity(string jsonEntity)
        {
            throw new NotImplementedException();
        }

    }
}

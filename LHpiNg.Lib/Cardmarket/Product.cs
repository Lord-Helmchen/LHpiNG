using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHpiNG.Cardmarket
{
    [Table("Products")]
    public class Product : ProductEntity
    {
        //TODO can we use ProductEntity.Website instead ?
        public string UrlSuffix { get; set; }


    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Cardmarket
{
    [Table("PriceGuides")]
    public class PriceGuide : PriceGuideProEntity
    {
        public DateTime FetchDate { get; set; }
        public PriceGuide PreviousPriceGuide { get; set; }
        public Product Product { get; set; }

        //public virtual PriceGuide PreviousPriceGuide { get; set; }
        //public int PreviousPriceGuideUid { get; set; }
        //public string ProductName { get; set; }
        //public string ExpansionName { get; set; }


        public PriceGuide() : base()
        {
        }
        //constructor from API reply
        public PriceGuide(string jsonEntity) : base(jsonEntity)
        {
            throw new NotImplementedException();
        }
        public PriceGuide(PriceGuideEntity entity) : base(entity)
        {
            if (entity is PriceGuide guide)
            {
                FetchDate = guide.FetchDate;
                PreviousPriceGuide = guide.PreviousPriceGuide;
                Product = guide.Product;
            }
        }

    }
}

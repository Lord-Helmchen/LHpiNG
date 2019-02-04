using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHpiNG.Cardmarket
{
    [Table("Products")]
    public class ProductEntity
    {
        //[Index]
        public int IdProduct { get; set; }                          // Product ID
        public int? IdMetaproduct { get; set; }                     // Metaproduct ID
        public int? CountReprints { get; set; }                     // Number of total products bundled by the metaproduct
        [Key, Column("Name",Order = 1)]
        public string EnName { get; set; }                          // Product's English name
        public IEnumerable<LocalizationEntity> Localization { get; set; } // localization entities for the product's name
        public CategoryEntity Category { get; set; }                // Category entity the product belongs to
        public string Website { get; set; }                         // URL to the product (relative to MKM's base URL)
        public string Image { get; set; }                           // Path to the product's image
        public string GameName { get; set; }                        // the game's name
        public string CategoryName { get; set; }                    // the category's name
        public int? Number { get; set; }                            // Number of product within the expansion (where applicable)
        public Rarity Rarity { get; set; }                          // Rarity of product (where applicable)
        [Key, Column(Order = 2)] 
        public string ExpansionName { get; set; }                   // Expansion's name 
        public IEnumerable<string> Links { get; set; }              // HATEOAS links
        /* The following information is only returned for responses that return the detailed product entity */
        [ForeignKey("ExpansionName")]// Foreign Key has to be a property name, not table column name
        public ExpansionEntity Expansion { get; set; }             // detailed expansion information (where applicable)
        public PriceGuideEntity PriceGuide { get; set; }            // Price guide entity '''(ATTN {get;set;} not returned for expansion requests)'''
        public ReprintEntity Reprint { get; set; }                  // Reprint entities for each similar product bundled by the metaproduct

        //[Key]
        //public int Uid { get; set; }                               // Entity Framework Primary Key

        public ProductEntity()
        {
            this.GameName = "Magic";
            this.Rarity = Rarity.None;
            this.Links = null;//not used yet
        }
        //constructor from API reply
        public ProductEntity(string jsonEntity)
        {
            throw new NotImplementedException();
        }
        //copy constructor
        public ProductEntity(ProductEntity entity)
        {
            IdProduct = entity.IdProduct;
            IdMetaproduct = entity.IdMetaproduct;
            CountReprints = entity.CountReprints;
            EnName = entity.EnName;
            Localization = entity.Localization;
            Category = entity.Category;
            Website = entity.Website;
            Image = entity.Image;
            GameName = entity.GameName;
            CategoryName = entity.CategoryName;
            Number = entity.Number;
            Rarity = entity.Rarity;
            ExpansionName = entity.ExpansionName;
            Links = entity.Links;
            Expansion = entity.Expansion;
            PriceGuide = entity.PriceGuide;
            Reprint = entity.Reprint;
    }
}
}

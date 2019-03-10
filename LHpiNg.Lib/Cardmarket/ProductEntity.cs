using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LHpiNG.Cardmarket
{
    [Table("Products")]
    public class ProductEntity : IEquatable<ProductEntity>
    {
        public int? IdProduct { get; set; }                          // Product ID
        public int? IdMetaproduct { get; set; }                     // Metaproduct ID
        public int? CountReprints { get; set; }                     // Number of total products bundled by the metaproduct
        //[Key, Column("Name",Order = 0)]
        public string EnName { get; set; }                          // Product's English name
        [NotMapped]
        public IEnumerable<LocalizationEntity> Localization { get; set; } // localization entities for the product's name
        [NotMapped]
        public CategoryEntity Category { get; set; }                // Category entity the product belongs to
        public string Website { get; set; }                         // URL to the product (relative to MKM's base URL)
        [NotMapped]
        public string Image { get; set; }                           // Path to the product's image
        [NotMapped]
        public string GameName { get; set; }                        // the game's name
        [NotMapped]
        public string CategoryName { get; set; }                    // the category's name
        public string Number { get; set; }                            // Number of product within the expansion (where applicable)
        public Rarity Rarity { get; set; }                          // Rarity of product (where applicable)
        public string ExpansionName { get; set; }                   // Expansion's name 
        [NotMapped]
        public IEnumerable<string> Links { get; set; }              // HATEOAS links
        /* The following information is only returned for responses that return the detailed product entity */
        public ExpansionEntity Expansion { get; set; }              // detailed expansion information (where applicable)
        [NotMapped]
        public PriceGuideEntity PriceGuide { get; set; }            // Price guide entity '''(ATTN {get;set;} not returned for expansion requests)'''
        [NotMapped]
        public ReprintEntity Reprint { get; set; }                  // Reprint entities for each similar product bundled by the metaproduct

        public ProductEntity()
        {
            this.GameName = "Magic";
            this.Rarity = Rarity.None;
            this.Links = Enumerable.Empty<string>();//not used yet
            this.CategoryName = "Singles";
            this.Localization = Enumerable.Empty<LocalizationEntity>();

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

        public bool Equals(ProductEntity other)
        {
            if (other == null) return false;
            return EnName == other.EnName
                && ExpansionName== other.ExpansionName;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as ProductEntity);
        }
        public override int GetHashCode()
        {
            return (EnName, ExpansionName).GetHashCode();
        }
    }
}

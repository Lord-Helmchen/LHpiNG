using System.Collections.Generic;
using SQLite;

namespace LHpiNG.Cardmarket
{
    public class ProductEntity
    {
        [PrimaryKey]
        public int IdProduct { get; set; }                          // Product ID
        [Indexed, Ignore]
        public int IdMetaproduct { get; set; }                      // Metaproduct ID
        public int CountReprints { get; set; }                      // Number of total products bundled by the metaproduct
        [Indexed]
        public string EnName { get; set; }                          // Product's English name
        [Ignore]
        public IEnumerable<LocalizationEntity> Localization { get; set; } // localization entities for the product's name
        [Ignore]
        public CategoryEntity Category { get; set; }                // Category entity the product belongs to
        public string Website { get; set; }                         // URL to the product (relative to MKM's base URL)
        [Ignore]
        public string Image { get; set; }                           // Path to the product's image
        [Ignore]
        public string GameName { get; set; }                        // the game's name
        [Ignore]
        public string CategoryName { get; set; }                    // the category's name
        public int Number { get; set; }                             // Number of product within the expansion (where applicable)
        public Rarity Rarity { get; set; }                          // Rarity of product (where applicable)
        [Ignore]
        public string ExpansionName { get; set; }                   // Expansion's name 
        [Ignore]
        public IEnumerable<string> Links { get; set; }              // HATEOAS links
        /* The following information is only returned for responses that return the detailed product entity */
        public ExpansionEntity Expansion { get; set; }              // detailed expansion information (where applicable)
        public PriceGuideEntity PriceGuide { get; set; }            // Price guide entity '''(ATTN {get;set;} not returned for expansion requests)'''
        [Ignore]
        public ReprintEntity Reprint { get; set; }                  // Reprint entities for each similar product bundled by the metaproduct
    }
}

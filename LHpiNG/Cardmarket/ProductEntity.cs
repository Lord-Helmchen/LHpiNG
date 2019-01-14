using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Cardmarket
{
    class ProductEntity
    {
        public int IdProduct { get; set; }                          // Product ID
        public int IdMetaproduct { get; set; }                      // Metaproduct ID
        public int CountReprints { get; set; }                      // Number of total products bundled by the metaproduct
        public string EnName { get; set; }                          // Product's English name
        public IEnumerable<LocalizationEntity> Localization { get; set; } // localization entities for the product's name
        public CategoryEntity Category { get; set; }                      // Category entity the product belongs to
        public string Website { get; set; }                         // URL to the product (relative to MKM's base URL)
        public string Image { get; set; }                           // Path to the product's image
        public string GameName { get; set; }                        // the game's name
        public string CategoryName { get; set; }                    // the category's name
        public int Number { get; set; }                             // Number of product within the expansion (where applicable)
        public Rarity Rarity { get; set; }                          // Rarity of product (where applicable)
        public string ExpansionName { get; set; }                   // Expansion's name 
        public IEnumerable<string> Links { get; set; }              // HATEOAS links
        /* The following information is only returned for responses that return the detailed product entity */
        public Expansion Expansion { get; set; }                    // detailed expansion information (where applicable)
        public PriceGuide PriceGuide { get; set; }                  // Price guide entity '''(ATTN {get;set;} not returned for expansion requests)'''
        public ReprintEntity Reprint { get; set; }                        // Reprint entities for each similar product bundled by the metaproduct

    }
}

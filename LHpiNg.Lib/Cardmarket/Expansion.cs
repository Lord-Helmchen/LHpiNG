using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Cardmarket
{
    public class Expansion : ExpansionEntity
    {
        public int ProductCount { get; set; }

        public DateTime ReleaseDateTime { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public string UrlSuffix { get; set; }

        public void ImportFromAPI()
        {
            throw new NotImplementedException();
        }
        public void LoadProductsFromDatabase()
        {
            throw new NotImplementedException();
        }
        public void SaveProductsToDatabase()
        {
            throw new NotImplementedException();
        }
        public void ImportProductsFromWeb()
        {
            throw new NotImplementedException();
        }

    }
}

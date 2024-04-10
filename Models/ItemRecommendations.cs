using CsvHelper;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;

namespace Intex.Models
{
    public class ItemRecommendations
    {
        public class Recommendation
        {
            public int ProductId { get; set; }
            public string productName { get; set; }
            public int rec1_ID { get; set; }
            public string rec1_Name { get; set; }
            public int rec2_ID { get; set; }
            public string rec2_Name { get; set; }
            public int rec3_ID { get; set;}
            public string rec3_Name { get; set;}
        }

        public class ProductService
        {
            public List<Product> LoadProductsFromCsv(string filePath)
            {
                var products = new List<Product>();

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    products = csv.GetRecords<Product>().ToList();
                }

                return products;
            }
        }
    }
}
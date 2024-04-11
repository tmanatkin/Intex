namespace Intex.Models
{
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public ItemRecommendation Recommendations { get; set; }
        //public string GetImgLink(int productId)
        //{
        //    var product = Products.FirstOrDefault(p => p.ProductId == productId);
        //    return product != null ? product.ImgLink : string.Empty;
        //}
    }
}

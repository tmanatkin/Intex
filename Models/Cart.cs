using Microsoft.Build.Evaluation;
namespace Intex.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();
        public virtual void AddItem(Product prod, int quantity)
        {
            CartLine? line = Lines
              .Where(p => p.Product.ProductId == prod.ProductId)
              .FirstOrDefault();
            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    Product = prod,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public virtual void RemoveLine(Product prod) =>
            Lines.RemoveAll(l =>
                l.Product.ProductId == prod.ProductId);
        public decimal ComputeTotalValue() =>
            (decimal)Lines.Sum(e => e.Product.Price * e.Quantity);
        public virtual void Clear() => Lines.Clear();
    }
        public class CartLine
        {
            public int CartLineId { get; set; }
            public Product Product { get; set; }
            public int Quantity { get; set; }
        }
    }


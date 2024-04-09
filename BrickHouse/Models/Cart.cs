namespace BrickHouse.Models
{
    public class Cart
    {
        // store a list of lines - the = new says if we didn't have one built before then build a new one
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        //pass in Product and quantity
        public virtual void AddItem(Product p, int quantity)
        {
            // build an instance where it's equal to the ProductID
            CartLine? line = Lines
                .Where(x => x.Product.ProductId == p.ProductId)
                .FirstOrDefault();

            if (line == null) 
            {
                Lines.Add(new CartLine()
                {
                    Product = p,
                    Quantity = quantity
                });
            }
            else 
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product p) => Lines.RemoveAll(x => x.Product.ProductId == p.ProductId);

        public virtual void Clear() => Lines.Clear();

        public virtual decimal CalculateTotal() => Lines.Sum(x => 25 * x.Quantity);
/*        {
            // The lambda function essentill does this
            var blah = Lines.Sum(x => 25 * x.Quantity);

            return blah
        }*/

        public class CartLine
        {
            public int CartlineID { get; set; }
            public Product Product { get; set; }
            public int Quantity { get; set; }
        }
    }
}

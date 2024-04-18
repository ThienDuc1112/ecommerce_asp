namespace FinalProject.ViewModels.Cart
{
    public class GetCart
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        public string ImageProduct { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
        public string Category { get; set; }
    }
}

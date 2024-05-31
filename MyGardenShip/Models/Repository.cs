using MyGardenShip.Data;

namespace MyGardenShip.Models
{
    public class Repository
    {
        private static readonly List<Product> _products = new();
        static Repository()
        {
            _products = new List<Product>
            {
                new Product(){ Name = "Banana",Amount=100,Id=12421,Location="Turkiye",Image="https://images.immediate.co.uk/production/volatile/sites/30/2017/01/Bunch-of-bananas-67e91d5.jpg?quality=90&webp=true&resize=300,272"},
                new Product(){ Name = "Apple",Amount=50,Id=23232,Location="Germany",Image="https://5.imimg.com/data5/AK/RA/MY-68428614/apple.jpg"},
                new Product(){ Name = "Watermelon",Amount=100,Id=24221,Location="Bharat",Image="https://media.istockphoto.com/id/1142119394/photo/whole-and-slices-watermelon-fruit-isolated-on-white-background.jpg?s=612x612&w=0&k=20&c=A5XnLyeI_3mwkCNadv-QLU4jzgNux8kUPfIlDvwT0jo="},
            };
        }
        public static List<Product> Products
        {
            get { return _products; }
        }
        public static Product? GetById(int id)
        {
            return _products.FirstOrDefault(c => c.Id == id);
        }
    }
}
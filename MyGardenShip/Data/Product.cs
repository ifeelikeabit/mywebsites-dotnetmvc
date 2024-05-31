using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGardenShip.Data
{
    public class Product
    {
        public string? Email { get; set; }
        public int Id { get; set; }
        public int Amount { get; set; }
        public string? Currency { get; set; }
        public string? Name { get; set; }
        public int? LifeTime { get; set; }
        public string? Type { get; set; }
        public int? PricePer { get; set; }
        public string? ProducerNameSurname { get; set; }
        public string? ProducerPhone { get; set; }
        public string? ShopName { get; set; }

        public string? Location { get; set; }
        public string? Image { get; set; }
    }
}
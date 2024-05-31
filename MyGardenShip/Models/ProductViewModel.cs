using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MyGardenShip.Data;

namespace MyGardenShip.Models
{
    public class ProductViewModel
    {
        public AppUser User;
        public required string Action;
        public required Product Product;
    }
}
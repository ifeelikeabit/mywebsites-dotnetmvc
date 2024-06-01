using System.ComponentModel.DataAnnotations;
using bordro.Data;

namespace bordro.Models
{



    public class Bordro
    {
        public long id { get; set; }
        public int? kesintiler { get; set; }
        public int? day { get; set; }
        public int? brüt { get; set; }
        public int? bordro { get; set; }
        public Employee? employee { get; set; }

        public bool isCalculate = false;

    }
}
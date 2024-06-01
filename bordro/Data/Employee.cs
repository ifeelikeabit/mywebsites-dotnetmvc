using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace bordro.Data
{
    public class Employee
    {
        [Key]
        [Required]
        [Range(1000000000, 9999999999, ErrorMessage = "Vergi numarası 10 haneli bir sayı olmalıdır.")]
        public long TaxNumber { get; set; }

        public string? CompanyName { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string FullName
        {
            get
            {
                return this.Name + " " + this.Surname;
            }
        }
        public DateOnly EntryDate { get; set; }


        public int DailysSalary { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace bordro.Data
{
    public class AppUser : IdentityUser
    {
        public ICollection<Employee>? Employees { get; set; }

        public DateOnly Since {get;set;}

    }
}
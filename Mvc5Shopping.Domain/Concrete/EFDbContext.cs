using Mvc5Shopping.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc5Shopping.Domain.Concrete
{
    class EFDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MRP.Models;

namespace MRP.Data
{
    public class MRPDbContext : DbContext
    {
        public MRPDbContext(DbContextOptions<MRPDbContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Product> Product { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<Inventory> Inventory { get; set; }

        public DbSet<BOM> BOM { get; set; }
    }
}

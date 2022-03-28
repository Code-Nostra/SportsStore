using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
        public DbSet<Product> Products { get; set; }//представляет набор сущностей, хранящихся в базе данных

        public DbSet<Order> Orders { get; set; }//После добавления новой сущьности следует выполнить
                                                //dotnet ef migrations add Orders
    }
}

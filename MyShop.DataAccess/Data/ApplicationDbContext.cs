using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyShop.Entities;
using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Data
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option)
            : base(option)
        {
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<ShopingCart>()
        //        .HasKey(c => c.Id);

        //    modelBuilder.Entity<ShopingCart>()
        //        .Property(c => c.Id)
        //        .ValueGeneratedOnAdd();
        //}
        public DbSet<Product> product { get; set; }
        public DbSet<Catigory> catigory { get; set; }
        public DbSet<ApplicationUser> applicationUser { get; set; }
        public DbSet<ShopingCart> shopingCarts { get; set; }
        public DbSet<OrderHeader> orderHeader { get; set; }
        public DbSet<OrderDetail> orderDetail { get; set; }


    }
}

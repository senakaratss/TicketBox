using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Domain.Entities;
using TicketBox.Persistence.Identity;

namespace TicketBox.Persistence.Context
{
    public class TicketContext : IdentityDbContext<AppUser>
    {
        public TicketContext(DbContextOptions<TicketContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                 .Property(x => x.Price)
                 .HasPrecision(18, 2);

            modelBuilder.Entity<Booking>()
                .Property(x => x.TotalPrice)
                .HasPrecision(18, 2);
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}

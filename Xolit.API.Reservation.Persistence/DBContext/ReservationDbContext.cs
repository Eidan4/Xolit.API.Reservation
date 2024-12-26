using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xolit.API.Reservation.Domain.Reservation;
using Xolit.API.Reservation.Domain.Space;
using Xolit.API.Reservation.Domain.User;

namespace Xolit.API.Reservation.Persistence.DBContext
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options) { }

        public DbSet<ReservationEntity> Reservations { get; set; }
        public DbSet<UserEntity> User { get; set; }
        public DbSet<SpaceEntity> Space { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de mapeos usando Fluent API
            modelBuilder.Entity<ReservationEntity>(entity =>
            {
                entity.ToTable("Reservations");
                entity.HasKey(r => r.Id);
            });

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(u => u.Id);
            });

            modelBuilder.Entity<SpaceEntity>(entity =>
            {
                entity.ToTable("Space");
                entity.HasKey(s => s.Id);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

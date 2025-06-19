using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Toran.Models
{
    public partial class BoiappContext : DbContext
    {
        public BoiappContext()
        {
        }

        public BoiappContext(DbContextOptions<BoiappContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ToranModel> Torans { get; set; }
        public virtual DbSet<ToranStatus> ToranStatuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server=DESKTOP-UT7PC9L;Database=BOIAPP;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToranModel>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__TORAN__3214EC072B60CC0A");

                entity.ToTable("TORAN");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            modelBuilder.Entity<ToranStatus>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ToranSta__3214EC075EECCE62");
                entity.Property(e => e.EmployeeName).HasMaxLength(100);
                entity.Property(e => e.LastDutyDate).HasColumnType("datetime");
            });

        }

    }
}

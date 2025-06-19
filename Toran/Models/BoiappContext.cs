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
        => optionsBuilder.UseMySql("server=127.0.0.1;port=3306;database=boiapp;user=root;password=1234", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));

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

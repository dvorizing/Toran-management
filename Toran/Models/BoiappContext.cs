using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Toran.Models
{
    public partial class BoiappContext : DbContext
    {
        private readonly string _connectionString;

        public BoiappContext(DbContextOptions<BoiappContext> options, IConfiguration config)
      : base(options)
        {
            _connectionString = config["BoiappConnection"];
        }


        public virtual DbSet<ToranModel> Torans { get; set; }
        public virtual DbSet<ToranStatus> ToranStatuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql(_connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));

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

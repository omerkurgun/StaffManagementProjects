using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DLL
{
    public partial class StaffDBModel : DbContext
    {
        public StaffDBModel(string connStr)
            : base(connStr)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<city> city { get; set; }
        public virtual DbSet<country> country { get; set; }
        public virtual DbSet<department> department { get; set; }
        public virtual DbSet<staff> staff { get; set; }
        public virtual DbSet<users> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<city>()
                .HasMany(e => e.staff)
                .WithOptional(e => e.city)
                .HasForeignKey(e => e.city_id);

            modelBuilder.Entity<country>()
                .HasMany(e => e.city)
                .WithRequired(e => e.country)
                .HasForeignKey(e => e.country_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<country>()
                .HasMany(e => e.staff)
                .WithOptional(e => e.country)
                .HasForeignKey(e => e.country_id);

            modelBuilder.Entity<department>()
                .HasMany(e => e.staff)
                .WithOptional(e => e.department)
                .HasForeignKey(e => e.department_id);
        }
    }
}

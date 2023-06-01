using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OkVip.ManagementDataMarketing.Models.DbModels;
using Taipei101.SslDomainVpsManager.Data;

namespace OkVip.ManagementDataMarketing.Data
{
    public class ApplicationDbContext : IdentityDbContext<TaipeiUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DisableCascadeDelete(modelBuilder);
            base.OnModelCreating(modelBuilder);
            new DbInitializer(modelBuilder).Seed();
        }

        public DbSet<TaipeiUser> User { get; set; }
        public DbSet<DataMarketing> DataMarketing { get; set; }


        #region helpers
        private void DisableCascadeDelete(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
        #endregion

    }
}

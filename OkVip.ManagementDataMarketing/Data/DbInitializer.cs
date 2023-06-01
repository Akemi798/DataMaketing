using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OkVip.ManagementDataMarketing.Commons;
using OkVip.ManagementDataMarketing.Commons.Constants;
using OkVip.ManagementDataMarketing.Models.DbModels;

namespace Taipei101.SslDomainVpsManager.Data
{
    public class DbInitializer
    {
        private readonly ModelBuilder modelBuilder;

        public DbInitializer(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            modelBuilder.Entity<IdentityRole>().HasData(SeedAdminRole());
            modelBuilder.Entity<TaipeiUser>().HasData(SeedAdminUser());
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(SeedIdentityUserRole());
        }

        private TaipeiUser SeedAdminUser()
        {
            TaipeiUser userAdmin = new TaipeiUser
            {
                Id = UserConstants.USER_ADMIN_ID,
                UserName = "admin789bet",
                NormalizedUserName = "admin789bet",
                Email = "admin789bet",
                NormalizedEmail = "admin789bet",
                PhoneNumber = "",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = new Guid().ToString("D"),
                GoogleAuthenticatorSecretCode = "C67F5D0E23F7",
                IsActivated = true,
                CreateDate = DateTime.Now,
                UpdateDate = null
            };
            userAdmin.PasswordHash = Functions.PassGenerate(userAdmin, "P@ssw0rd@789bet");
            return userAdmin;
        }

        private IdentityRole SeedAdminRole()
        {
            IdentityRole adminRole = new IdentityRole()
            {
                Id = RoleConstants.ADMIN_ROLE_ID,
                Name = RoleConstants.ADMIN_ROLE_NAME,
                NormalizedName = RoleConstants.ADMIN_ROLE_NAME
            };
            return adminRole;
        }

        private IdentityUserRole<string> SeedIdentityUserRole()
        {
            IdentityUserRole<string> result = new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
            {
                RoleId = RoleConstants.ADMIN_ROLE_ID,
                UserId = UserConstants.USER_ADMIN_ID
            };
            return result;
        }

        

    }
}

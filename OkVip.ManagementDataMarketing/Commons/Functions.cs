using Microsoft.AspNetCore.Identity;
using OkVip.ManagementDataMarketing.Models.DbModels;

namespace OkVip.ManagementDataMarketing.Commons
{
    public static class Functions
    {
        public static string PassGenerate(TaipeiUser user, string password)
        {
            var passHash = new PasswordHasher<TaipeiUser>();
            return passHash.HashPassword(user, password);
        }
    }
}

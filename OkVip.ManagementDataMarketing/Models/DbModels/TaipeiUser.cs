using Microsoft.AspNetCore.Identity;
using System;

namespace OkVip.ManagementDataMarketing.Models.DbModels
{
    public class TaipeiUser : IdentityUser
    {
        public string GoogleAuthenticatorSecretCode { get; set; }
        public bool IsActivated { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}

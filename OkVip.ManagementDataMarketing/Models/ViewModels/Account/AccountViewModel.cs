using System;

namespace OkVip.ManagementDataMarketing.Models.ViewModels.Account
{
    public class AccountViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool IsActivated { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string QrCode { get; set; }
        public string ManualEntryKey { get; set; }

    }
}

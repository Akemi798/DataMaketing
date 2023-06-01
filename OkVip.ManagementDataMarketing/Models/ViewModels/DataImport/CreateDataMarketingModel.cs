using System;

namespace OkVip.ManagementDataMarketing.Models.ViewModels.DataImport
{
    public class CreateDataMarketingModel
    {
        public string PhoneNumber { get; set; }
        public string EmployeeName { get; set; }
        public string BuyDate { get; set; }
        public string DataBuyOfWeb { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDuplicate { get; set; }
        public bool IsDeleted { get; set; }
    }
}

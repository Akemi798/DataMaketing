using System;

namespace OkVip.ManagementDataMarketing.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class ModelStateErrorViewModel
    {
        public string PropertyName { get; set; }
        public string Message { get; set; }
    }
}

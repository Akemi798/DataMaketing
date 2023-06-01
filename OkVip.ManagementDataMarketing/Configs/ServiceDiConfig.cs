using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OkVip.ManagementDataMarketing.Services;

namespace OkVip.ManagementDataMarketing.Configs
{
    public static class ServiceDiConfig
    {
        public static void Config(IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IDataMarketingServices, DataMarketingServices>();
        }
    }
}

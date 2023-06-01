using AutoMapper;
using System;
using OkVip.ManagementDataMarketing.Models.DbModels;
using OkVip.ManagementDataMarketing.Models.ViewModels.DataImport;

namespace OkVip.ManagementDataMarketing.Models.Mappers
{
    public class DataMarketingProfile : Profile
    {
        public DataMarketingProfile()
        {
            CreateMap<DataMarketing, DataMarketingViewModel>();
        }
    }
}

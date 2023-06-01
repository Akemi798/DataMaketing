using AutoMapper;
using System;
using OkVip.ManagementDataMarketing.Models.DbModels;
using OkVip.ManagementDataMarketing.Models.ViewModels.Account;

namespace OkVip.ManagementDataMarketingModels.Mappers
{
    public class TaipeiUserProfile : Profile
    {
        public TaipeiUserProfile()
        {
            CreateMap<TaipeiUser, AccountViewModel>();
            CreateMap<CreateAccountViewModel, TaipeiUser>()
                .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.UserName))
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName))
                .ForMember(dest => dest.IsActivated, opts => opts.MapFrom(src => true))
                .ForMember(dest => dest.LockoutEnabled, opts => opts.MapFrom(src => false))
                .ForMember(dest => dest.PhoneNumberConfirmed, opts => opts.MapFrom(src => true))
                .ForMember(dest => dest.EmailConfirmed, opts => opts.MapFrom(src => true))
                .ForMember(dest => dest.CreateDate, opts => opts.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.PhoneNumber, opts => opts.MapFrom(src => string.Empty))
            ;

            CreateMap<AccountViewModel, UpdateAccountViewModel>();
            CreateMap<UpdateAccountViewModel, TaipeiUser>();
            

        }
    }

}

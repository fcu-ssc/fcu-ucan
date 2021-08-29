using AutoMapper;
using fcu_ucan.Entities;
using fcu_ucan.Models.User;

namespace fcu_ucan.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            #region ApplicationUser 轉換成 UserViewModel

            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.EmailConfirmed,
                    opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PhoneNumberConfirmed,
                    opt => opt.MapFrom(src => src.PhoneNumberConfirmed))
                .ForMember(dest => dest.TwoFactorEnabled,
                    opt => opt.MapFrom(src => src.TwoFactorEnabled))
                .ForMember(dest => dest.LockoutEnd,
                    opt => opt.MapFrom(src => src.LockoutEnd))
                .ForMember(dest => dest.LockoutEnabled,
                    opt => opt.MapFrom(src => src.LockoutEnabled))
                .ForMember(dest => dest.AccessFailedCount,
                    opt => opt.MapFrom(src => src.AccessFailedCount))
                .ForMember(dest => dest.IsEnable,
                    opt => opt.MapFrom(src => src.IsEnable));

            #endregion
            
            #region ApplicationUser 轉換成 UserEditViewModel

            CreateMap<ApplicationUser, UserEditViewModel>()
                .ForMember(dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PhoneNumberConfirmed,
                    opt => opt.MapFrom(src => src.PhoneNumberConfirmed))
                .ForMember(dest => dest.TwoFactorEnabled,
                    opt => opt.MapFrom(src => src.TwoFactorEnabled))
                .ForMember(dest => dest.LockoutEnd,
                    opt => opt.MapFrom(src => src.LockoutEnd))
                .ForMember(dest => dest.LockoutEnabled,
                    opt => opt.MapFrom(src => src.LockoutEnabled))
                .ForMember(dest => dest.AccessFailedCount,
                    opt => opt.MapFrom(src => src.AccessFailedCount))
                .ForMember(dest => dest.IsEnable,
                    opt => opt.MapFrom(src => src.IsEnable));

            #endregion
            
            #region UserEditViewModel 轉換成 ApplicationUser

            CreateMap<UserEditViewModel, ApplicationUser>()
                .ForMember(dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PhoneNumberConfirmed,
                    opt => opt.MapFrom(src => src.PhoneNumberConfirmed))
                .ForMember(dest => dest.TwoFactorEnabled,
                    opt => opt.MapFrom(src => src.TwoFactorEnabled))
                .ForMember(dest => dest.LockoutEnd,
                    opt => opt.MapFrom(src => src.LockoutEnd))
                .ForMember(dest => dest.LockoutEnabled,
                    opt => opt.MapFrom(src => src.LockoutEnabled))
                .ForMember(dest => dest.AccessFailedCount,
                    opt => opt.MapFrom(src => src.AccessFailedCount))
                .ForMember(dest => dest.IsEnable,
                    opt => opt.MapFrom(src => src.IsEnable));

            #endregion
        }
    }
}
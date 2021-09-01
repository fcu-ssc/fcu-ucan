using System.Linq;
using AutoMapper;
using fcu_ucan.Entities;
using fcu_ucan.Models.Account;
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
                .ForMember(dest => dest.IsEnable,
                    opt => opt.MapFrom(src => src.IsEnable))
                .ForMember(dest => dest.IsRecorder,
                    opt => opt.MapFrom(src => src.UserRoles.Any(x => x.Role.NormalizedName == "Recorder".ToUpperInvariant())))
                .ForMember(dest => dest.IsMember,
                    opt => opt.MapFrom(src => src.UserRoles.Any(x => x.Role.NormalizedName == "Member".ToUpperInvariant())))
                .ForMember(dest => dest.IsUser,
                    opt => opt.MapFrom(src => src.UserRoles.Any(x => x.Role.NormalizedName == "User".ToUpperInvariant())))
                .ForMember(dest => dest.IsUCAN,
                    opt => opt.MapFrom(src => src.UserRoles.Any(x => x.Role.NormalizedName == "UCAN".ToUpperInvariant())));

            #endregion
            
            #region ApplicationUser 轉換成 UserEditViewModel

            CreateMap<ApplicationUser, UserEditViewModel>()
                .ForMember(dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PhoneNumberConfirmed,
                    opt => opt.MapFrom(src => src.PhoneNumberConfirmed))
                .ForMember(dest => dest.IsEnable,
                    opt => opt.MapFrom(src => src.IsEnable))
                .ForMember(dest => dest.IsRecorder,
                    opt => opt.MapFrom(src => src.UserRoles.Any(x => x.Role.NormalizedName == "Recorder".ToUpperInvariant())))
                .ForMember(dest => dest.IsMember,
                    opt => opt.MapFrom(src => src.UserRoles.Any(x => x.Role.NormalizedName == "Member".ToUpperInvariant())))
                .ForMember(dest => dest.IsUser,
                    opt => opt.MapFrom(src => src.UserRoles.Any(x => x.Role.NormalizedName == "User".ToUpperInvariant())))
                .ForMember(dest => dest.IsUCAN,
                    opt => opt.MapFrom(src => src.UserRoles.Any(x => x.Role.NormalizedName == "UCAN".ToUpperInvariant())));

            #endregion
            
            #region UserEditViewModel 轉換成 ApplicationUser

            CreateMap<UserEditViewModel, ApplicationUser>()
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedEmail,
                    opt => opt.MapFrom(src => src.Email.ToUpperInvariant()))
                .ForMember(dest => dest.EmailConfirmed,
                    opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PhoneNumberConfirmed,
                    opt => opt.MapFrom(src => src.PhoneNumberConfirmed))
                .ForMember(dest => dest.IsEnable,
                    opt => opt.MapFrom(src => src.IsEnable));

            #endregion

            #region UserInviteViewModel 轉換成 ApplicationUser

            CreateMap<UserInviteViewModel, ApplicationUser>()
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedEmail,
                    opt => opt.MapFrom(src => src.Email.ToUpperInvariant()));

            #endregion

            #region RegisterViewModel 轉換成 ApplicationUser

            CreateMap<RegisterViewModel, ApplicationUser>()
                .ForMember(dest => dest.EmailConfirmed,
                    opt => opt.MapFrom(o => true))
                .ForMember(dest => dest.IsEnable,
                    opt => opt.MapFrom(o => true));

            #endregion
        }
    }
}
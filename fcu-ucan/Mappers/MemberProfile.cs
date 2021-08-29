using AutoMapper;
using fcu_ucan.Entities;
using fcu_ucan.Models.Member;

namespace fcu_ucan.Mappers
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            #region Member 轉換成 MemberViewModel

            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NetworkId,
                    opt => opt.MapFrom(src => src.NetworkId))
                .ForMember(dest => dest.StudentId,
                    opt => opt.MapFrom(src => src.StudentId));

            #endregion
            
            #region Member 轉換成 MemberEditViewModel

            CreateMap<Member, MemberEditViewModel>()
                .ForMember(dest => dest.NetworkId,
                    opt => opt.MapFrom(src => src.NetworkId))
                .ForMember(dest => dest.StudentId,
                    opt => opt.MapFrom(src => src.StudentId));

            #endregion
            
            #region MemberEditViewModel 轉換成 Member

            CreateMap<MemberEditViewModel, Member>()
                .ForMember(dest => dest.NetworkId,
                    opt => opt.MapFrom(src => src.NetworkId))
                .ForMember(dest => dest.StudentId,
                    opt => opt.MapFrom(src => src.StudentId));

            #endregion
            
            #region MemberAddViewModel 轉換成 Member

            CreateMap<MemberAddViewModel, Member>()
                .ForMember(dest => dest.NetworkId,
                    opt => opt.MapFrom(src => src.NetworkId))
                .ForMember(dest => dest.StudentId,
                    opt => opt.MapFrom(src => src.StudentId));

            #endregion
        }
    }
}
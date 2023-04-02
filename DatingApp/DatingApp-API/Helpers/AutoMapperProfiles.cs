using AutoMapper;
using DatingApp_API.Entities;
using DatingApp_API.DTOs;
using System.Linq;
using DatingApp_API.Extensions;
using API.DTOs;

namespace DatingApp_API.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(x=>x.PhotoUrl,o=>o.MapFrom(s=>s.Photos.FirstOrDefault(s=>s.IsMain).Url))
                .ForMember(x=>x.Age,o=>o.MapFrom(s=>s.DateOfBirth.CalculatedAge()));
            CreateMap<Photo, PhotoDto>();

            CreateMap<MemberUpdateDto, AppUser>();

        }
    }
}

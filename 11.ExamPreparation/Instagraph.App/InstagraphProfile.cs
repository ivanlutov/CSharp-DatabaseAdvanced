using AutoMapper;
using Instagraph.DataProcessor.Dtos;
using Instagraph.Models;

namespace Instagraph.App
{
    public class InstagraphProfile : Profile
    {
        public InstagraphProfile()
        {
            CreateMap<UserDto, User>()
                .ForMember(u => u.ProfilePicture, pp => pp.UseValue<Picture>(null));

            CreateMap<Post, UncommentedPostDto>()
                .ForMember(dto => dto.User,
                    opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dto => dto.Picture,
                    opt => opt.MapFrom(src => src.Picture.Path));

            CreateMap<User, UserFollowerCountDto>()
                .ForMember(dto => dto.Followers,
                    opt => opt.MapFrom(src => src.Followers.Count));
        }
    }
}

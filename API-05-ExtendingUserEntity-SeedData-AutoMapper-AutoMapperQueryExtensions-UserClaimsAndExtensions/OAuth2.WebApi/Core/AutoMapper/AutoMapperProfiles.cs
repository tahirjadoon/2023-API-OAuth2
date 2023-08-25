using AutoMapper;
using OAuth2.WebApi.Core.Dto;
using OAuth2.WebApi.Core.Entities;
using OAuth2.WebApi.Core.Extensions;

namespace OAuth2.WebApi.Core.AutoMapper;

//derive from a class provided by AutoMapper called Profile
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        Map_AppUser_To_UserDto();
        Map_Photo_To_PhotoDto();
    }

    private void Map_AppUser_To_UserDto()
    {
        //same name propertirs will be automatically mapped
        //Age will also get automatically mapped since source has GetAge, the keywor Age are the same
        //PhotoUrl we'll need to map manually. will pick the url where isMain is true. Do check for null. 
        //  ***Hint: An expression tree lambda may not contain a null propagating operator.
        //  so use a function intead
        CreateMap<AppUser, UserDto>()
        //.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName.ToTitleCase()))
        .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => PickMainUrl_AppUser_To_UserDto(src.Photos)))
        .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()))
        ;
    }

    //made the method static after change to AutoMapper Queryable Extensions
    private static string PickMainUrl_AppUser_To_UserDto(ICollection<Photo> photos)
    {
        if (photos == null || !photos.Any()) return string.Empty;
        var url = photos.FirstOrDefault(x => x.IsMain)?.URL ?? string.Empty;
        return url;
    }

    private void Map_Photo_To_PhotoDto()
    {
        CreateMap<Photo, PhotoDto>();
    }
}

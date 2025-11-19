using AutoMapper;
using BacklogApp.DataAccess.Layer.Models;
using BacklogGames.Bussinnes.Layer.DTOs.Game;
using BacklogGames.Bussinnes.Layer.DTOs.Igdb;
using BacklogGames.Bussinnes.Layer.DTOs.UserList;
using BacklogGames.DataAccess.Layer.Models;

namespace BacklogGames.Bussinnes.Layer
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CreateGameDto,Game>().ReverseMap();
            CreateMap<GameDto,Game>().ReverseMap();
            CreateMap<UserListDTo,UserList>().ReverseMap();

            // IGDB mappings
            CreateMap<IgdbGameResponseDto, GameInfoDto>()
                .ForMember(dest => dest.IgdbId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CoverUrl, opt => opt.MapFrom(src =>
                    src.Cover != null
                        ? $"https://images.igdb.com/igdb/image/upload/t_cover_big/{src.Cover.ImageId}.jpg"
                        : null));
        }
    }
}

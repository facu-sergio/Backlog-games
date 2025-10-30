using AutoMapper;
using BacklogApp.DataAccess.Layer.Models;
using BacklogGames.Bussinnes.Layer.DTOs.Game;
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
        }
    }
}

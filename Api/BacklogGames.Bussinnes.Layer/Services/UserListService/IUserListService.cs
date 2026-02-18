using BacklogGames.Bussinnes.Layer.DTOs.Game;
using BacklogGames.Bussinnes.Layer.DTOs.UserList;

namespace BacklogGames.Bussinnes.Layer.Services.UserListService
{
    public interface IUserListService
    {
        Task<UserListDTo> AddUserList(string name);
        Task<UserListGameResponseDto> AddGameToListAsync(AddGameToListDto addGameToListDto);
        Task<ICollection<UserListDTo>> GetAllList();
        Task<ICollection<GameDto>> GetGamesByListIdAsync(int listId);
        Task<UserListGameResponseDto> MarkGameAsCompletedAsync(int listId, int gameId, DateTime? completedAt);
        Task<ICollection<CompletedGameDto>> GetCompletedGamesByYearAsync(int year);
    }
}

using AutoMapper;
using BacklogApp.DataAccess.Layer.Models;
using BacklogGames.Bussinnes.Layer.DTOs.Game;
using BacklogGames.Bussinnes.Layer.DTOs.UserList;
using BacklogGames.DataAccess.Layer.Enums;
using BacklogGames.DataAccess.Layer.Models;
using BacklogGames.DataAccess.Layer.UnitOfWork;

namespace BacklogGames.Bussinnes.Layer.Services.UserListService
{
    public class UserListService : IUserListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserListService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserListDTo> AddUserList(string name)
        {
            var UserList = new UserList { Name = name};
            await _unitOfWork.UserListRepository.AddAsync(UserList);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UserListDTo>(UserList);
        }

        public async Task<UserListGameResponseDto> AddGameToListAsync(AddGameToListDto dto)
        {
            var userList = await _unitOfWork.UserListRepository.GetAsync(dto.UserListId);

            var existingGames = await _unitOfWork.GameRepository.GetAsync(dto.GameInfo.IgdbId);

            var game = new Game();
            if (existingGames == null)
            {
                game = new Game
                {   
                    IgdbId = dto.GameInfo.IgdbId,
                    Name = dto.GameInfo.Name,
                    CoverUrl = dto.GameInfo.CoverUrl,
                    FirstReleaseDate = dto.GameInfo.FirstReleaseDate,
                    Summary = dto.GameInfo.Summary,
                    Rating = dto.GameInfo.Rating,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.GameRepository.AddAsync(game);
                await _unitOfWork.SaveChangesAsync();
            }

            var existingEntry = await _unitOfWork.UserListGameRepository.GetFirstOrDefaultAsync(ulg => ulg.GameId == dto.GameInfo.IgdbId && ulg.UserListId == dto.UserListId);

            if(existingEntry != null)
            {
                throw new Exception();
            }

            var userListGame = new UserListGame
            {
                GameId = game.Id,
                UserListId = dto.UserListId,
                GameStatusId = dto.StatusId ?? (int)GameProgressStatus.Pendiente,
                AddedAt = DateTime.UtcNow
            };

            await _unitOfWork.UserListGameRepository.AddAsync(userListGame);
            await _unitOfWork.SaveChangesAsync();

            return new UserListGameResponseDto
            {
                GameId = game.Id,
                GameName = game.Name,
                UserListId = userList.Id,
                UserListName = userList.Name,
                StatusId = userListGame.GameStatusId,
                StatusName = ((GameProgressStatus)userListGame.GameStatusId).ToString(),
                AddedAt = userListGame.AddedAt
            };
        }

        public async Task<ICollection<UserListDTo>> GetAllList()
        {
            var list = await _unitOfWork.UserListRepository.GetAllAsync();
            return list.Select(item => _mapper.Map<UserListDTo>(item)).ToList();
        }

        public async Task<ICollection<GameDto>> GetGamesByListIdAsync(int listId)
        {
            var games = await _unitOfWork.UserListRepository.GetGamesByListIdAsync(listId);
            return games.Select(game => _mapper.Map<GameDto>(game)).ToList();
        }

        public async Task<UserListGameResponseDto> MarkGameAsCompletedAsync(int listId, int gameId, DateTime? completedAt)
        {
            var date = completedAt?.ToUniversalTime() ?? DateTime.UtcNow;
            await _unitOfWork.UserListGameRepository.MarkAsCompletedAsync(gameId, listId, date);

            var entry = await _unitOfWork.UserListGameRepository.GetFirstOrDefaultAsync(
                ulg => ulg.GameId == gameId && ulg.UserListId == listId,
                includeProperties: "Game,UserList");

            return new UserListGameResponseDto
            {
                GameId = entry!.GameId,
                GameName = entry.Game.Name,
                UserListId = entry.UserListId,
                UserListName = entry.UserList.Name,
                StatusId = entry.GameStatusId,
                StatusName = ((GameProgressStatus)entry.GameStatusId).ToString(),
                AddedAt = entry.AddedAt,
                CompletedAt = entry.CompletedAt
            };
        }

        public async Task<ICollection<CompletedGameDto>> GetCompletedGamesByYearAsync(int year)
        {
            var entries = await _unitOfWork.UserListGameRepository.GetCompletedByYearAsync(year);
            return entries.Select(ulg => new CompletedGameDto
            {
                GameId = ulg.GameId,
                GameName = ulg.Game.Name,
                CoverUrl = ulg.Game.CoverUrl,
                Rating = ulg.Game.Rating,
                UserListId = ulg.UserListId,
                UserListName = ulg.UserList.Name,
                CompletedAt = ulg.CompletedAt!.Value
            }).ToList();
        }
    }
}

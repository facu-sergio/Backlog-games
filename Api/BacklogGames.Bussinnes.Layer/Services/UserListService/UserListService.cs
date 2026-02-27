using AutoMapper;
using BacklogApp.DataAccess.Layer.Models;
using BacklogGames.Bussinnes.Layer.DTOs.Game;
using BacklogGames.Bussinnes.Layer.DTOs.UserList;
using BacklogGames.Bussinnes.Layer.Exceptions;
using BacklogGames.Bussinnes.Layer.Services.IgdbService;
using BacklogGames.DataAccess.Layer.Enums;
using BacklogGames.DataAccess.Layer.Models;
using BacklogGames.DataAccess.Layer.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace BacklogGames.Bussinnes.Layer.Services.UserListService
{
    public class UserListService : IUserListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IIgdbService _igdbService;
        private readonly ILogger<UserListService> _logger;

        public UserListService(IUnitOfWork unitOfWork, IMapper mapper, IIgdbService igdbService, ILogger<UserListService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _igdbService = igdbService;
            _logger = logger;
        }

        /// <summary>
        /// Crea una nueva lista de usuario con el nombre dado y la persiste en la base de datos.
        /// </summary>
        public async Task<UserListDTo> AddUserList(string name)
        {
            var UserList = new UserList { Name = name};
            await _unitOfWork.UserListRepository.AddAsync(UserList);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UserListDTo>(UserList);
        }

        /// <summary>
        /// Agrega un juego a una lista de usuario. Si el juego no existe localmente lo crea,
        /// intenta enriquecer sus datos con información de tiempo de juego desde IGDB,
        /// y lo vincula a la lista con el estado indicado.
        /// </summary>
        public async Task<UserListGameResponseDto> AddGameToListAsync(AddGameToListDto dto)
        {
            var userList = await _unitOfWork.UserListRepository.GetAsync(dto.UserListId)
                ?? throw new CustomException(404, $"UserList con id {dto.UserListId} no encontrada.");

            var game = await GetOrCreateGameAsync(dto.GameInfo);

            var existingEntry = await _unitOfWork.UserListGameRepository
                .GetFirstOrDefaultAsync(ulg => ulg.GameId == game.Id && ulg.UserListId == dto.UserListId);

            if (existingEntry != null)
                throw new CustomException(409, $"El juego '{game.Name}' ya está en la lista '{userList.Name}'.");

            var userListGame = new UserListGame
            {
                GameId = game.Id,
                UserListId = dto.UserListId,
                GameStatusId = dto.StatusId ?? (int)GameStatusEnum.Pendiente,
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
                StatusName = ((GameStatusEnum)userListGame.GameStatusId).ToString(),
                AddedAt = userListGame.AddedAt
            };
        }

        /// <summary>
        /// Devuelve todas las listas de usuario existentes.
        /// </summary>
        public async Task<ICollection<UserListDTo>> GetAllList()
        {
            var list = await _unitOfWork.UserListRepository.GetAllAsync();
            return [.. list.Select(_mapper.Map<UserListDTo>)];
        }

        /// <summary>
        /// Devuelve todos los juegos asociados a una lista, incluyendo su estado y datos de tiempo de juego.
        /// </summary>
        public async Task<ICollection<GameDto>> GetGamesByListIdAsync(int listId)
        {
            var entries = await _unitOfWork.UserListRepository.GetGamesByListIdAsync(listId);
            return [.. entries.Select(ulg => new GameDto
            {
                Id = ulg.Game.Id,
                Name = ulg.Game.Name,
                CoverUrl = ulg.Game.CoverUrl,
                FirstReleaseDate = ulg.Game.FirstReleaseDate,
                Summary = ulg.Game.Summary,
                Rating = ulg.Game.Rating,
                CreatedAt = ulg.Game.CreatedAt,
                UpdatedAt = ulg.Game.UpdatedAt,
                GameStatusId = ulg.GameStatusId,
                GameStatusName = ulg.GameStatus.Name,
                HastilySeconds = ulg.Game.HastilySeconds,
                NormallySeconds = ulg.Game.NormallySeconds,
                CompletelySeconds = ulg.Game.CompletelySeconds,
                TimeToBeatCount = ulg.Game.TimeToBeatCount
            })];
        }

        /// <summary>
        /// Actualiza el estado de un juego dentro de una lista y devuelve la entrada actualizada.
        /// </summary>
        public async Task<UserListGameResponseDto> UpdateGameStatusAsync(int listId, int gameId, UpdateGameStatusDto dto)
        {
            await _unitOfWork.UserListGameRepository.UpdateGameStatusAsync(gameId, listId, dto.StatusId, dto.CompletedAt);

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
                StatusName = ((GameStatusEnum)entry.GameStatusId).ToString(),
                AddedAt = entry.AddedAt,
                CompletedAt = entry.CompletedAt
            };
        }

        /// <summary>
        /// Devuelve todos los juegos completados en un año específico, con su información de estado y lista.
        /// </summary>
        public async Task<ICollection<CompletedGameDto>> GetCompletedGamesByYearAsync(int year)
        {
            var entries = await _unitOfWork.UserListGameRepository.GetCompletedByYearAsync(year);
            return [.. entries.Select(ulg => new CompletedGameDto
            {
                Id = ulg.GameId,
                GameName = ulg.Game.Name,
                CoverUrl = ulg.Game.CoverUrl,
                Rating = ulg.Game.Rating,
                UserListId = ulg.UserListId,
                UserListName = ulg.UserList.Name,
                StatusId = ulg.GameStatusId,
                StatusName = ((GameStatusEnum)ulg.GameStatusId).ToString(),
                CompletedAt = ulg.CompletedAt!.Value
            })];
        }

        /// <summary>
        /// Busca un juego por su IgdbId en la base de datos local. Si no existe, lo crea usando
        /// la información del DTO e intenta obtener sus datos de tiempo de juego desde IGDB.
        /// Si la llamada a IGDB falla, el juego se guarda igualmente sin esa información.
        /// </summary>
        private async Task<Game> GetOrCreateGameAsync(GameInfoDto info)
        {
            var existing = await _unitOfWork.GameRepository.GetFirstOrDefaultAsync(g => g.IgdbId == info.IgdbId);
            if (existing != null)
                return existing;

            var game = _mapper.Map<Game>(info);

            try
            {
                var timeToBeat = await _igdbService.GetTimeToBeatAsync(info.IgdbId);
                if (timeToBeat != null)
                {
                    game.HastilySeconds = timeToBeat.Hastily;
                    game.NormallySeconds = timeToBeat.Normally;
                    game.CompletelySeconds = timeToBeat.Completely;
                    game.TimeToBeatCount = timeToBeat.Count;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch time to beat from IGDB for game {IgdbId}. Game will be saved without this data.", info.IgdbId);
            }

            await _unitOfWork.GameRepository.AddAsync(game);
            await _unitOfWork.SaveChangesAsync();

            return game;
        }
    }
}

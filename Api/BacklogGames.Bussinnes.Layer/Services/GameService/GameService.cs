using AutoMapper;
using BacklogApp.DataAccess.Layer.Models;
using BacklogGames.Bussinnes.Layer.DTOs.Game;
using BacklogGames.DataAccess.Layer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogGames.Bussinnes.Layer.Services.GameService
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GameService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GameDto> AddGame(CreateGameDto gameDto)
        {
          var game = _mapper.Map<Game>(gameDto);
          await _unitOfWork.GameRepository.AddAsync(game);
          await _unitOfWork.SaveChangesAsync();
         return _mapper.Map<GameDto>(game); 
        }
    }
}

using Popcore.Domain.Models;
using Popcore.Domain.Repositories;
using Popcore.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Popcore.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<Game> DeleteAsync(int id)
        {
            var existingGame = await _gameRepository.FindByIdAsync(id);

            if (existingGame == null)
                return new Game();

            try
            {
                _gameRepository.Remove(existingGame);
            }
            catch (Exception ex)
            {
                // do some logging.

            }
            return existingGame;
        }

        public async Task<IEnumerable<Game>> ListAsync(string query)
        {
            return await _gameRepository.ListAsync("sushil");
        }

        public Task<Game> SaveAsync(Game game)
        {
            throw new NotImplementedException();
        }

        public Task<Game> UpdateAsync(int id, Game product)
        {
            throw new NotImplementedException();
        }

        Task<Game> IGameService.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Game>> IGameService.ListAsync(string query)
        {
            throw new NotImplementedException();
        }
    }
}

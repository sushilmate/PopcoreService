using Popcore.Domain.Models;
using Popcore.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Popcore.Persistence.Repositories
{
    public class GameRepository : IGameRepository
    {
        public Task AddAsync(Game game)
        {
            throw new NotImplementedException();
        }

        public Task<Game> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Game>> ListAsync(string query)
        {
            throw new NotImplementedException();
        }

        public void Remove(Game game)
        {
            throw new NotImplementedException();
        }

        public void Update(Game game)
        {
            throw new NotImplementedException();
        }
    }
}

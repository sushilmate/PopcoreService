using Popcore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Popcore.Domain.Repositories
{
    public interface IGameRepository
    {
        public Task<IEnumerable<Game>> ListAsync(string query);
        public Task AddAsync(Game game);
        public Task<Game> FindByIdAsync(int id);
        public void Update(Game game);
        public void Remove(Game game);
    }
}

using Popcore.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Popcore.Domain.Services
{
    public interface IGameService
    {
        public Task<IEnumerable<Game>> ListAsync(string query);
        public Task<Game> SaveAsync(Game game);
        public Task<Game> UpdateAsync(int id, Game product);
        public Task<Game> DeleteAsync(int id);
    }
}
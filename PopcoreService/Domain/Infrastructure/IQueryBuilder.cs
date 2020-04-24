using Popcore.API.Domain.Models.Search.External;

namespace Popcore.API.Domain.Infrastructure
{
    public interface IQueryBuilder
    {
        string Build(ProductSearchInput input);
    }
}

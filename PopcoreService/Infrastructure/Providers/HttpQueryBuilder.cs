using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Popcore.API.Domain.Infrastructure;
using Popcore.API.Domain.Models.Search.External;
using Popcore.API.Infrastructure.Configuratiions;

namespace Popcore.API.Infrastructure.Providers
{
    public class HttpQueryBuilder : IQueryBuilder
    {
        private readonly BaseUrlOptions _options;

        public HttpQueryBuilder(IOptions<BaseUrlOptions> options)
        {
            _options = options.Value;
        }

        public string Build(ProductSearchInput input)
        {
            string baseUrl = _options.UsOpenFoodFacts;

            QueryBuilder builder = new QueryBuilder
            {
                { "action", "process" },
                { "tagtype_0", input.CriteriaType },
                { "tag_contains_0", input.CriteriaClause.ToString().ToLower() },
                { "tag_0", input.CriteriaValue },
                { "json", "true" }
            };

            return $"{baseUrl}{builder}";
        }
    }
}

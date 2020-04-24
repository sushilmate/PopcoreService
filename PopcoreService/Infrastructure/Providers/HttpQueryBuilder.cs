using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Popcore.API.Domain.Infrastructure;
using Popcore.API.Domain.Models.Search.External;

namespace Popcore.API.Infrastructure.Providers
{
    public class HttpQueryBuilder : IQueryBuilder
    {
        private readonly IConfiguration _config;

        public HttpQueryBuilder(IConfiguration config)
        {
            _config = config;
        }

        public string Build(ProductSearchInput input)
        {
            // ToDo we can convert this one into singlton so we dont have to read everytime
            string baseUrl = _config.GetSection("BaseUrls").GetSection("UsOpenFoodFacts").Value;

            QueryBuilder builder = new QueryBuilder
            {
                { "action", "process" },
                { "tagtype_0", input.CriteriaType },
                { "tag_contains_0", input.CriteriaClause.ToString().ToLower() },
                { "tag_0", input.CriteriaValue },
                { "json", "true" }
            };

            return string.Format("{0}{1}", baseUrl, builder.ToQueryString());
        }
    }
}

using Microsoft.Extensions.Configuration;
using Popcore.API.Domain.Infrastructure;
using Popcore.API.Domain.Models.Search.External;

namespace Popcore.API.Infrastructure.Providers
{
    public class HttpQueryBuilder: IQueryBuilder
    {
        private readonly IConfiguration _config;

        public HttpQueryBuilder(IConfiguration config)
        {
            _config = config;
        }

        public string Build(ProductSearchInput input)
        {
            // not sure with web api url so used US food facts and search api.
            string baseUrl = _config.GetSection("BaseUrls").GetSection("OpenFoodFacts").Value;

            string url = string.Format("{0}action=process&tagtype_0={1}&tag_contains_0=contains&tag_0={2}&json=true",
                baseUrl, input.CriteriaType, input.CriteriaValue);

            return url;
        }
    }
}

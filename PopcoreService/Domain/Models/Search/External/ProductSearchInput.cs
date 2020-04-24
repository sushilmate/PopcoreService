using Popcore.API.Domain.Models.Type;

namespace Popcore.API.Domain.Models.Search.External
{
    public class ProductSearchInput
    {
        public ProductSearchInput()
        {
        }

        public ProductSearchInput(string criteriaType, SearchClause searchClause, string criteriaValue)
        {
            CriteriaType = criteriaType;
            CriteriaClause = searchClause;
            CriteriaValue = criteriaValue;
        }

        public string CriteriaType { get; set; }

        public SearchClause CriteriaClause { get; set; }

        public string CriteriaValue { get; set; }
    }
}

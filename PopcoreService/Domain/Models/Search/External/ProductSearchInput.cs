using System.ComponentModel;

namespace Popcore.API.Domain.Models.Search.External
{
    public class ProductSearchInput
    {
        public string CriteriaType { get; set; }

        public SearchClause CriteriaClause { get; set; }

        public string CriteriaValue { get; set; }
    }

    public enum SearchClause
    {
        [Description("contains")]
        Contains = 0,
        [Description("dostnotcontain")]
        DoesNotContain = 1
    }
}

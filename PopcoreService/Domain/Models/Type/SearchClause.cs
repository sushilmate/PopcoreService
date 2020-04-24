using System.ComponentModel;

namespace Popcore.API.Domain.Models.Type
{
    public enum SearchClause
    {
        [Description("contains")]
        Contains = 0,
        [Description("dostnotcontain")]
        DoesNotContain = 1
    }
}

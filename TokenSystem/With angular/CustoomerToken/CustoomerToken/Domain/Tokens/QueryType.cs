using System.ComponentModel.DataAnnotations;

namespace CustoomerToken.Domain.Tokens
{
    public enum QueryType
    {
        General = 1,
        [Display(Name = @"New Product")]
        NewProduct,
        Others
    }

    public enum QueryStatus
    {
        Pending = 1,
        Processing,
        Resolved
    }
}

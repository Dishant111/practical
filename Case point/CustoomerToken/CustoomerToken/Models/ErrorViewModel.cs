using CustoomerToken.Domain.Tokens;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace CustoomerToken.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }


    public class TokenListModel
    {
        public int Id { get; set; }

        [DisplayName("Query type")]
        public QueryType Query { get; set; }
        [DisplayName("Query Status")]
        public QueryStatus Status { get; set; }
    }

    public class PaginationResult<T>
    {
        protected PaginationResult()
        { }
        public PaginationResult(int totalCount, int pagesize, int pageNumber, List<T> data)
        {
            TotalCount = totalCount;
            Pagesize = pagesize;
            PageNumber = pageNumber;
            Data = data;
        }

        public int TotalCount { get; }
        public int Pagesize { get; }
        public int PageNumber { get; }
        public List<T> Data { get; }
    }

    public class TokenCreateModel
    {
        [DisplayName("Query type")]
        public QueryType Query { get; set; }
    }


    public class EmployeeLoginModel
    {
        [Required]
        [Length(minimumLength: 2, maximumLength: 20)]
        public string UserName { get; set; }
        [Required]
        [Length(minimumLength: 2, maximumLength: 20)]
        public string Password { get; set; }
    }


    public class RegistrationModel
    {
        [DisplayName("Query type")]
        public QueryType Query { get; set; }
        [Required]
        [Length(minimumLength: 2, maximumLength: 20)]
        public string UserName { get; set; }
        [Required]
        [Length(minimumLength: 2, maximumLength: 20)]
        public string Password { get; set; }
    }
    public class ResolveToken
    {
        public int Id { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [DisplayName("Query type")]
        public QueryType Query { get; set; }

    }
}

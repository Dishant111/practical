using CustoomerToken.Domain.Tokens;

public class Token
{
    public int Id { get; set; }
    public QueryType QueryId { get; set; }
    public QueryStatus StatusId { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    public bool IsDeleted { get; set; }
}
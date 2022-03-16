namespace Accounting.Models;

public class Block
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int Amount { get; set; }
    public DateTime CreateDatetime { get; set; }
}
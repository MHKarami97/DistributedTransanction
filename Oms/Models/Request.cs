namespace Oms.Models;

public class Request
{
    public Request()
    {
        Errors = new List<RequestError>();
    }

    public int Id { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
    public RequestType RequestType { get; set; }
    public RequestState RequestState { get; set; }
    public ICollection<RequestError> Errors { get; set; }
}
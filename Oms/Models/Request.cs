using Oms.Models.Enums;

namespace Oms.Models;

public class Request
{
    public Request()
    {
        Errors = new List<RequestError>();
    }

    public int Id { get; set; }
    public string? InstrumentName { get; set; }
    public int? OriginCode { get; set; }
    public int Price { get; set; }
    public int CustomerId { get; set; }
    public int BlockCode { get; set; }
    public int Quantity { get; set; }
    public RequestType RequestType { get; set; }
    public RequestState RequestState { get; set; }
    public ICollection<RequestError> Errors { get; set; }
}

using WebApplication1.Core.BaseModels;

namespace WebApplication1.Core.ResponseModels;

public class WordTransactionResponse{
    public bool IsSuccess { get; set; }
    public  string? Message { get; set; }
    public  string? Transaction { get; set; }
    public List<Words>? Words { get; set; }
    public int Word_Count { get; set; }
}
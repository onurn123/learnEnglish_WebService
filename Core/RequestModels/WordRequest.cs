using System;
using WebApplication1.Core.BaseModels;

namespace WebApplication1.Core.RequestModels;

public class WordRequest
{
    public required string Token { get; set; }
    public int? id { get; set; }
    public  Words? Word { get; set; }
}

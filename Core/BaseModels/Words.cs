
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Core.BaseModels;

public class Words{

    [Key]
    public int id { get; set; }
    public required string word_english { get; set; }
    public required string word_turkish { get; set; }
}
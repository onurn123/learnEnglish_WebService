
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Core.BaseModels;

public class UserModel{

    [Key]
    public int id { get; set; }
    public required string user_mail { get; set; }
    public required string user_password { get; set; }
    public required string user_registration_date { get; set; }

}
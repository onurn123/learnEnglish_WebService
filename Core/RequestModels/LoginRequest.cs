
namespace WebApplication1.Core.RequestModels;

public class LoginRequest{

    public required string user_mail { get; set; }
    public required string user_password { get; set; }
}
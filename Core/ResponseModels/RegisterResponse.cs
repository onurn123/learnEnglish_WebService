
using WebApplication1.Core.BaseModels;

namespace WebApplication1.Core.ResponseModels;

public class RegisterResponse
{
    public bool IsSuccess { get; set; }

    public required string Message { get; set; }

    public UserModel? User { get; set; }
}
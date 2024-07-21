using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using WebApplication1.Core.ApplicationDatabaseContext;
using WebApplication1.Core.BaseModels;
using WebApplication1.Core.RequestModels;
using WebApplication1.Core.ResponseModels;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class userController : ControllerBase{

    public readonly dbContext _context;
    public userController(dbContext db) { 
        // Constructor
        _context = db;
    }
    [HttpPost]
    [Route("login")]
    public IActionResult Login([FromBody] LoginRequest login) {
        
        //Login if matches with Bcrypt.Net Verify Password
        var user = _context.users.FirstOrDefault(x => x.user_mail == login.user_mail);
        if(user == null || BCrypt.Net.BCrypt.Verify(login.user_password, user.user_password)==false)
            return Unauthorized(new LoginResponse{
                IsSuccess = false,
                Message = "Invalid email or password",
            });

        return Ok(new LoginResponse{
            IsSuccess = true,
            Message = "Login Successful",
            User = user
        });
    }
    [HttpPost]
    [Route("register")]
    public IActionResult Register([FromBody] RegisterRequest register) {
        
        //Is Registered 
        if(_context.users.Any(x => x.user_mail == register.user_mail))
            return BadRequest(new RegisterResponse{
                IsSuccess = false,
                Message = "User already registered with this email",
            });

        var newUser = new UserModel{
            user_mail = register.user_mail,
            user_password = BCrypt.Net.BCrypt.HashPassword(register.user_password),
            user_registration_date = DateTime.Now.ToShortDateString()+" "+DateTime.Now.ToString("HH:mm:ss")
        };

        _context.users.Add(newUser);
        _context.SaveChanges();

        return Ok(new RegisterResponse{
            IsSuccess = true,
            Message = "User registered successfully",
            User = _context.users.FirstOrDefault(x=>x.user_mail==newUser.user_mail)
        });
    
    }
}
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API;

public class AccountController :BaseApiController
{
    private readonly DataContext _context;

    private readonly ItokenService _TokenService ;

    public AccountController(DataContext context ,ItokenService tokenService)
    {
        _context = context;
        _TokenService = tokenService;
    }
    [HttpPost("register")] // Post : // api/account/register

    public async Task<ActionResult<UserDtos>>Register(RegisterDto registerDto)
    {
        if (await UserExist(registerDto.UserName)) return BadRequest("user is Taken");
          using  var hmac = new HMACSHA512();
           var user = new AppUser
           {
            UserName =registerDto.UserName.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
           };
           _context.Add(user);
           await _context.SaveChangesAsync();
           return new UserDtos
           {
            Username =user.UserName,
            Token = _TokenService.CreateToken(user)
           };
    }
     [HttpPost("Login")]

     public async Task<ActionResult<UserDtos>> Login(LoginDto loginDto)
     {
          var user = await _context.Users.SingleOrDefaultAsync(x =>x.UserName ==loginDto.Username);

          if(user == null) return Unauthorized("User Invalid");

           var hmac = new HMACSHA512(user.PasswordSalt);

           var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
           for (int i = 0; i < computedHash.Length; i++)
           {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Password isnot Correct");
           }
             
             return new UserDtos
           {
            Username =user.UserName,
            Token = _TokenService.CreateToken(user)
           };
     }

    private async Task<bool> UserExist(string username)
    {
        return await _context.Users.AnyAsync( x =>x.UserName ==username.ToLower());
    }
}

using BlazorShoppingApp.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Server.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;
        private readonly IConfiguration configuration; //dzieki temu mozna czytac rzeczy z appsettings.json

        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();

            if (await UserExists(user.Email))
            {
                response.Success = false;
                response.Message = "Email already exist!";
                return response;
            }

            using (var hmac = new HMACSHA512())
            {
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            response.Data = user.Id;
            response.Success = true;
            response.Message = "Account created!";
            return response;
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            User user = await context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
            if (user == null)
            {
                return new ServiceResponse<string>()
                {
                    Success = false,
                    Message = "User not exists!"
                };
            }
            else if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                return new ServiceResponse<string>()
                {
                    Success = false,
                    Message = "Invalid password!"
                };
            }

            return new ServiceResponse<string>
            {
                Success = true,
                Data = CreateToken(user)
            };
    }
    

    public async Task<bool> UserExists(string email)
    {
        return await context.Users.AnyAsync(user => user.Email.ToLower().Equals(email.ToLower()));
    }

    private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }

        private string CreateToken(User user)
    {
        List<Claim> claims = new() //te rzeczy beda dopisane do paylodu tokena
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

            var keyStr = configuration.GetSection("AppSettings:Token").Value;
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(keyStr));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(claims: claims, signingCredentials: creds, expires: DateTime.Now.AddDays(1));
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
    }
}
}

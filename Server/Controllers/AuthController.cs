using BlazorShoppingApp.Server.Data;
using BlazorShoppingApp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorShoppingApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        [HttpPost("register")] //   localhost:port/auth/register
        public async Task<IActionResult> Register(UserRegistration userRegistration)
        {
            User user = new User
            {
                Email = userRegistration.Email,
                Confirmation = userRegistration.Confirmation,
                DateOfBirth = userRegistration.DateOfBirth,
                Username = userRegistration.Username,
                Gender = userRegistration.Gender
                //TODO: reszte dopisac
            };

            var response = await authRepository.Register(user, userRegistration.Password);
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("login")] //   localhost:port/auth/register
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            var response = await authRepository.Login(userLogin.Email, userLogin.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}

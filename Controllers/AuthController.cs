using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace DatingApp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisDto user){
            user.Username = user.Username.ToLower();
            if(await _repo.UserExists(user.Username))
            return BadRequest("Username Already exists");

            var userToCreate = new User{
                Username = user.Username
            };
            var createdUser = await _repo.Register(userToCreate,user.Password);
            return StatusCode(201);
        }
    }
}
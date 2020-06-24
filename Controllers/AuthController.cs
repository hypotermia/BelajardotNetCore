using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;



namespace DatingApp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
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
        // before this u must install a nuget 
        //dotnet add package Microsoft.IdentityModel.Tokens --version 6.5.0
        //dotnet add package System.IdentityModel.Tokens.Jwt --version 6.5.0
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto userLogin){
            var userFromRepo = await _repo.Login(userLogin.Username.ToLower(),userLogin.Password);
            if(userFromRepo == null)
            return Unauthorized();
            //token
            var claims = new []{
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromRepo.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                // expired 24hours(1day)
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });

        }

    }
}
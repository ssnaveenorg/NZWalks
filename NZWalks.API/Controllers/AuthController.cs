using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenHandler _tokenHandler;

        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
        {
            this._userRepository = userRepository;
            this._tokenHandler = tokenHandler;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {
            //Validate incoming request
            //Check if user is authenticated
            //check username and password
            var user = await _userRepository.Authenticate(loginRequest.Username, loginRequest.Password);
            if (user != null)
            {
                //Generate JWT Token
                var token = await _tokenHandler.CreateTokerAsync(user);
                return Ok(token);
            }
            return BadRequest("Wrong Credentials");
        }
    }
}

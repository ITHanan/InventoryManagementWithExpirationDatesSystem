using InventoryManagementWithExpirationDatesSystem.JWTHelper;
using InventoryManagementWithExpirationDatesSystem.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;


namespace InventoryManagementWithExpirationDatesSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly JWTGenerater _jwtGenerater;

        public UserController(JWTGenerater jwtGenerater)
        {
            _jwtGenerater = jwtGenerater;
        }


        [HttpPost("logIn")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            
            if (login.Username == "admin" && login.Password == "password")
            {
                var token = _jwtGenerater.JWTTokenGenerator(login.Username);


                return Ok(new { token });
            }
           return Unauthorized();
        }
    }
}

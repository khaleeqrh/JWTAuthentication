using System;
using System.Threading.Tasks;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
    public class UserController : Controller
    {
        private UserService userService;
        public UserController()
        {
            userService = new UserService();
        }
         [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] UserDTO request)
        {
            var response = await userService.loginAsync();
            if(response.Value is not null)
            {
                return Ok(response);
            }
            return BadRequest();
        }
        
    }
}
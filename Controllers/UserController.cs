using System;
using System.Threading.Tasks;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Model;
using Services;

namespace Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] UserDTO request)
        {
            var response = await userService.loginAsync(request);
            if (response.Value.AccessToken is not null)
            {
                return Ok(response);
            }
            return BadRequest();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterPostDTO postedData)
        {
            var response = new TokenResponseDTO();

            //automapper
            User postedUser = new User();
            postedUser.UserName = postedData.Email;
            postedUser.Email = postedData.Email;
            postedUser.FirstName = postedData.FirstName;
            postedUser.LastName = postedData.LastName;
            postedUser.PhoneNumber = postedData.PhoneNumber;

            //pass these to UserService
            try
            {
                var jwt = await userService.Register(postedUser, postedData.Password);
                response.AccessToken = jwt.Value.ToString();
            }
            catch (Exception ex)
            {
                response.AccessToken = ex.Message;
                return BadRequest(new {ex.Message});
            }
            return Ok(response);
        }

    }
}
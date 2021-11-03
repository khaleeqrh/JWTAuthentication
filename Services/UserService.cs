using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly dbContext db;
        private readonly SignInManager<User> signInManager;
        private readonly IOptions<AppSettings> appSettings;
        private readonly UserManager<User> userManager;

        private IConfiguration config;
        public UserService(dbContext db, SignInManager<User> signInManager, IOptions<AppSettings> appSettings,UserManager<User> userManager,IConfiguration config)
        {
            this.db = db;
            this.signInManager = signInManager;
            this.appSettings = appSettings;
            this.userManager = userManager;
            this.config = config;
        }
        public async Task<ActionResult<TokenResponseDTO>> loginAsync(UserDTO userDetails)
        {
            var response = new TokenResponseDTO();
            var userFromDB = db.Users.Where(a => a.Email == userDetails.Users).FirstOrDefault();
            if (userFromDB == null)
            {
                throw new Exception("Provided Email does not exist.");
            }
            var signInResult = await signInManager.CheckPasswordSignInAsync(userFromDB, userDetails.Password, false);
            if (signInResult.Succeeded)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = config["Jwt:Key"];
                var key = Encoding.UTF8.GetBytes(secretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                };
                tokenDescriptor.Expires = DateTime.Now.AddDays(60);
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var accessToken = tokenHandler.WriteToken(token);
                response.Expiry = tokenDescriptor.Expires;
                response.AccessToken = accessToken;
                
            }

            return response;
        }
         public async Task<ActionResult<TokenResponseDTO>> Register(User submittedUser, string submittedPassword)
        {
            if (String.IsNullOrWhiteSpace(submittedPassword) == true && submittedUser is null)
            {
                throw new Exception("Submitted information is invalid");
            }

            var userExist = await userManager.FindByEmailAsync(submittedUser.Email);
            if (userExist != null)
            {
                throw new Exception($"User with email “{userExist.Email}” already exist.");
            }

            StringBuilder stringBuilder = new StringBuilder("");

            var signupResult = await userManager.CreateAsync(submittedUser, submittedPassword);

            if (signupResult.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(submittedUser.Email);
                if (user.Email is not null)
                {
                    var LoginUser = new UserDTO();
                    LoginUser.Users = submittedUser.Email;
                    LoginUser.Password = submittedPassword;
                    var authenticateResponseDTO = await loginAsync(LoginUser);
                    return authenticateResponseDTO;
                }
                else
                {
                    throw new Exception(stringBuilder.ToString());
                }
            }
            else
            {
                foreach (var item in signupResult.Errors)
                {
                    stringBuilder.Append(item.Description + "\n");
                }
                throw new Exception(stringBuilder.ToString());
            }
        }
    }
}
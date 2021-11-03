using System;
using System.Threading.Tasks;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace Services
{
    public interface IUserService
    {
         Task<ActionResult<TokenResponseDTO>> loginAsync(UserDTO userDetails);
        Task<ActionResult<TokenResponseDTO>> Register (User submittedUser, string submittedPassword);   
    }
}
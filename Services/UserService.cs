using System;
using System.Threading.Tasks;
using DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Services
{
    public class UserService
    {
        public async Task<ActionResult<TokenResponseDTO>> loginAsync()
        {
            var response = new TokenResponseDTO();
            return response;
        }
    }
}
using System;

namespace DTOs
{
    public class TokenResponseDTO
    {
        public string AccessToken { get; set; }
        public DateTimeOffset? Expiry { get; set; }
    }
}
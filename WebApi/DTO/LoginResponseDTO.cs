namespace WebApi.DTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }



        /*
         The response will look like:
     { "token": "eyJhbGciOiJIUzI1NiIs...",
       "expiration": "2026-10-12T15:30:00Z" 
        
      }*/

    }

}


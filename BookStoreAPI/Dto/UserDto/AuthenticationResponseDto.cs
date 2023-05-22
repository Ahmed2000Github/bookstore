namespace BookStoreAPI.Dto.UserDto
{
    public class AuthenticationResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpireIn { get; set; }
        public DateTime RefreshTokenExpireIn { get; set; }
    }
}

namespace BookStoreApp.ViewModels.Admin
{
    public class AdminAuthorViewModel
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
    public class AdminTokenData
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpireIn { get; set; }
        public DateTime RefreshTokenExpireIn { get; set; }
    }
}

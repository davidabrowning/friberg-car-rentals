namespace FribergCarRentals.WebApi.Dtos
{
    public class UserDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public List<string> AuthRoles { get; set; } = new();
        public AdminDto? AdminDto { get; set; }
        public CustomerDto? CustomerDto { get; set; }
    }
}

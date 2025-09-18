namespace GestaoConcessionariasWebApp.Models.Users.Login
{
    public sealed class RegisterDto
    {
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public AccessLevel AccessLevel { get; set; }
    }
}

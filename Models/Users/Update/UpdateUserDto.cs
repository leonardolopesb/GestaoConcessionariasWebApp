namespace GestaoConcessionariasWebApp.Models.Users.Update
{
    public sealed class UpdateUserDto
    {
        public string UserName { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public AccessLevel AccessLevel { get; set; }
    }
}

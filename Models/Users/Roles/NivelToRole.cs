namespace GestaoConcessionariasWebApp.Models.Users.Roles
{
    public static class NivelToRole
    {
        public static string Map(string nivel)
        {
            return nivel?.Trim().ToLower() switch
            {
                "administrador" => Roles.Admin,
                "vendedor" => Roles.Vendedor,
                "gerente" => Roles.Gerente,
                _ => Roles.Vendedor
            };
        }
    }
}

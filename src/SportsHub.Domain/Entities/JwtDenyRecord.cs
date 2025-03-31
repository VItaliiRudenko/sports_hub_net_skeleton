namespace SportsHub.Domain.Entities;

public class JwtDenyRecord
{
    public string Jti { get; set; }
    public DateTime Iat { get; set; }
    public DateTime Exp { get; set; }
}
using SportsHub.Domain.Entities;

namespace SportsHub.Domain.Repositories;

public interface IJwtDenyListRepository
{
    public void Create(JwtDenyRecord jwtDenyRecord);
    public Task<JwtDenyRecord> GetById(string jti);
    public Task DeleteExpired(DateTime maxExp);
}
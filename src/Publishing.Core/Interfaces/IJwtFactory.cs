using Publishing.Core.DTOs;

namespace Publishing.Core.Interfaces
{
    public interface IJwtFactory
    {
        string GenerateToken(UserDto user);
    }
}

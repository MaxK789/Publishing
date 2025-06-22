namespace Publishing.Services;

using Publishing.Core.DTOs;

public interface IJwtFactory
{
    string GenerateToken(UserDto user);
}

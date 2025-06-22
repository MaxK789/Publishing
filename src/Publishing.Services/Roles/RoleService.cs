using System;

namespace Publishing.Services
{
    public class RoleService : IRoleService
    {
        public bool IsAdmin(string userType) => string.Equals(userType, "admin", StringComparison.OrdinalIgnoreCase);

        public bool IsContactPerson(string userType) => string.Equals(userType, "контактна особа", StringComparison.OrdinalIgnoreCase);

        public bool CanViewStatistics(string userType) => IsAdmin(userType);
    }
}

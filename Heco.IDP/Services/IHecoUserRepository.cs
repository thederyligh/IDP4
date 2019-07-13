using Heco.IDP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heco.IDP.Services
{
    public interface IHecoUserRepository
    {
        ApplicationUser GetUserByUsername(string username);
        ApplicationUser GetUserBySubjectId(string subjectId);
        ApplicationUser GetUserByEmail(string email);
        ApplicationUser GetUserByProvider(string loginProvider, string providerKey);
        IEnumerable<ApplicationUserLogin> GetUserLoginsBySubjectId(string subjectId);
        IEnumerable<ApplicationClaim> GetUserClaimsBySubjectId(string subjectId);
        bool AreUserCredentialsValid(string username, string password);
        bool IsUserActive(string subjectId);
        void AddUser(ApplicationUser user);
        void AddUserLogin(string subjectId, string loginProvider, string providerKey);
        void AddUserClaim(string subjectId, string claimType, string claimValue);
        bool Save();
    }
}

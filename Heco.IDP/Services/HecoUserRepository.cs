using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heco.IDP.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heco.IDP.Services
{
    public class HecoUserRepository : IHecoUserRepository
    {
        HecoUserContext _context;

        public HecoUserRepository(HecoUserContext context)
        {
            _context = context;
        }

        public bool AreUserCredentialsValid(string username, string password)
        {
            // get the user
            var user = GetUserByUsername(username);
            if (user == null)
            {
                return false;
            }

            return (user.PasswordHash == password && !string.IsNullOrWhiteSpace(password));
        }

        public ApplicationUser GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Claims.Any(c => c.ClaimType == "email" && c.ClaimValue == email));
        }

        public ApplicationUser GetUserByProvider(string loginProvider, string providerKey)
        {
            return _context.Users
                .FirstOrDefault(u => 
                    u.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey));
        }

        public ApplicationUser GetUserBySubjectId(string subjectId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == subjectId);
        }

        public ApplicationUser GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == username);
        }

        public IEnumerable<ApplicationClaim> GetUserClaimsBySubjectId(string subjectId)
        {
            // get user with claims
            var user = _context.Users.Include("Claims").FirstOrDefault(u => u.Id == subjectId);
            if (user == null)
            {
                return new List<ApplicationClaim>();
            }
            return user.Claims.ToList();
        }

        public IEnumerable<ApplicationUserLogin> GetUserLoginsBySubjectId(string subjectId)
        {
            var user = _context.Users.Include("Logins").FirstOrDefault(u => u.Id == subjectId);
            if (user == null)
            {
                return new List<ApplicationUserLogin>();
            }
            return user.Logins.ToList();
        }

        public bool IsUserActive(string subjectId)
        {
            var user = GetUserBySubjectId(subjectId);
            return user.IsActive;
         }

        public void AddUser(ApplicationUser user)
        {
            _context.Users.Add(user);
        }

        public void AddUserLogin(string subjectId, string loginProvider, string providerKey)
        {
            var user = GetUserBySubjectId(subjectId);
            if (user == null)
            {
                throw new ArgumentException("User with given subjectId not found.", subjectId);
            }

            user.Logins.Add(new ApplicationUserLogin()
            {
                ApplicationUserId = subjectId,
                LoginProvider = loginProvider,
                ProviderKey = providerKey
            });
        }

        public void AddUserClaim(string subjectId, string claimType, string claimValue)
        {          
            var user = GetUserBySubjectId(subjectId);
            if (user == null)
            {
                throw new ArgumentException("User with given subjectId not found.", subjectId);
            }

            user.Claims.Add(new ApplicationClaim(claimType, claimValue));         
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }

       
    }
}

using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Heco.IDP.Services
{
    public class HecoUserProfileService : IProfileService
    {
        public IHecoUserRepository _hecoUserRepository { get; set; }

        public HecoUserProfileService(IHecoUserRepository hecoUserRepository)
        {
            _hecoUserRepository = hecoUserRepository;
        }
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var claimsForUser = _hecoUserRepository.GetUserClaimsBySubjectId(subjectId);

            context.IssuedClaims = claimsForUser.Select(select => new Claim(select.ClaimType, select.ClaimValue)).ToList();

            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            context.IsActive = _hecoUserRepository.IsUserActive(subjectId);

            return Task.FromResult(0);
        }
    }
}

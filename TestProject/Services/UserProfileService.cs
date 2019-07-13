using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TestProject.Services
{
    public class TestProjectUserProfileService : IProfileService
    {
        public ITestProjectUserRepository _TestProjectUserRepository { get; set; }

        public TestProjectUserProfileService(ITestProjectUserRepository TestProjectUserRepository)
        {
            _TestProjectUserRepository = TestProjectUserRepository;
        }
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var claimsForUser = _TestProjectUserRepository.GetUserClaimsBySubjectId(subjectId);

            context.IssuedClaims = claimsForUser.Select(select => new Claim(select.ClaimType, select.ClaimValue)).ToList();

            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            context.IsActive = _TestProjectUserRepository.IsUserActive(subjectId);

            return Task.FromResult(0);
        }
    }
}

using TestProject.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddTestProjectUserStore(this IIdentityServerBuilder builder)
        {
            builder.Services.AddScoped<ITestProjectUserRepository, TestProjectUserRepository>();
            builder.AddProfileService<TestProjectUserProfileService>();
            return builder;
        }
    }
}

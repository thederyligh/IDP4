using Heco.IDP.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heco.IDP
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddHecoUserStore(this IIdentityServerBuilder builder)
        {
            builder.Services.AddScoped<IHecoUserRepository, HecoUserRepository>();
            builder.AddProfileService<HecoUserProfileService>();
            return builder;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject
{
    public static class TestProjectUserContextExtensions
    {
        public static void EnsureSeedDataForContext(this TestProjectUserContext context)
        {
            //// Add 2 demo users if there aren't any users yet
            //if (context.Users.Any())
            //{
            //    return;
            //}

            //// init users
            //var users = new List<User>()
            //{
            //    new ApplicationUser()
            //    {
            //        SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
            //        Username = "Frank",
            //        Password = "password",
            //        IsActive = true,
            //        Claims = {
            //             new ApplicationClaim("role", "FreeUser"),
            //             new ApplicationClaim("given_name", "Frank"),
            //             new ApplicationClaim("family_name", "Underwood"),
            //             new ApplicationClaim("address", "Main Road 1"),
            //             new ApplicationClaim("subscriptionlevel", "FreeUser"),
            //             new ApplicationClaim("country", "nl")
            //        }
            //    },
            //    new ApplicationUser()
            //    {
            //        SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
            //        Username = "Claire",
            //        Password = "password",
            //        IsActive = true,
            //        Claims = {
            //             new ApplicationClaim("role", "PayingUser"),
            //             new ApplicationClaim("given_name", "Claire"),
            //             new ApplicationClaim("family_name", "Underwood"),
            //             new ApplicationClaim("address", "Big Street 2"),
            //             new ApplicationClaim("subscriptionlevel", "PayingUser"),
            //             new ApplicationClaim("country", "be")
            //    }
            //    }
            //};

            //context.Users.AddRange(users);
            //context.SaveChanges();
        }
    }
}

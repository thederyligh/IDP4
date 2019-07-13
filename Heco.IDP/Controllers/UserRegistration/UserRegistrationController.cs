using IdentityServer4.Services;
using Heco.IDP.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Heco.IDP.Controllers.UserRegistration
{
    public class UserRegistrationController : Controller
    {
        private readonly IHecoUserRepository _hecoUserRepository;
        private readonly IIdentityServerInteractionService _interaction;

        public UserRegistrationController(IHecoUserRepository hecoUserRepository,
             IIdentityServerInteractionService interaction)
        {
            _hecoUserRepository = hecoUserRepository;
            _interaction = interaction;
        }

        [HttpGet]
        public IActionResult RegisterUser(RegistrationInputModel registrationInputModel)
        {
            var vm = new RegisterUserViewModel()
            {
                ReturnUrl = registrationInputModel.ReturnUrl,
                Provider = registrationInputModel.Provider,
                ProviderUserId = registrationInputModel.ProviderUserId
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var obj = Guid.NewGuid();
                var subjectId = obj.ToString();
                // create user + claims
                var userToCreate = new Entities.ApplicationUser();
                userToCreate.Id = subjectId;
                userToCreate.PasswordHash = model.Password;
                userToCreate.UserName = model.Username;
                userToCreate.IsActive = true;
                userToCreate.Claims.Add(new Entities.ApplicationClaim("country", model.Country));
                userToCreate.Claims.Add(new Entities.ApplicationClaim("address", model.Address));
                userToCreate.Claims.Add(new Entities.ApplicationClaim("given_name", model.Firstname));
                userToCreate.Claims.Add(new Entities.ApplicationClaim("family_name", model.Lastname));
                userToCreate.Claims.Add(new Entities.ApplicationClaim("email", model.Email));
                userToCreate.Claims.Add(new Entities.ApplicationClaim("subscriptionlevel", "FreeUser"));

                // if we're provisioning a user via external login, we must add the provider &
                // user id at the provider to this user's logins
                if (model.IsProvisioningFromExternal)
                {
                    userToCreate.Logins.Add(new Entities.ApplicationUserLogin()
                    {
                        LoginProvider = model.Provider,
                        ProviderKey = model.ProviderUserId
                    });
                }

                // add it through the repository
                _hecoUserRepository.AddUser(userToCreate);

                if (!_hecoUserRepository.Save())
                {
                    throw new Exception($"Creating a user failed.");
                }

                if (!model.IsProvisioningFromExternal)
                {
                    // log the user in
                    await HttpContext.SignInAsync(userToCreate.Id, userToCreate.UserName);
                }

                // continue with the flow     
                if (_interaction.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return Redirect("~/");
            }

            // ModelState invalid, return the view with the passed-in model
            // so changes can be made
            return View(model);
        }

    }
}

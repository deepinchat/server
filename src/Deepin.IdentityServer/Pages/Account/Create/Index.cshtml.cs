using Deepin.Application.IntegrationEvents;
using Deepin.Application.Interfaces;
using Deepin.Domain.Identity;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Deepin.IdentityServer.Pages.Create;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly UserManager<User> _userManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventBus _eventBus;

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public Index(
        IIdentityServerInteractionService interaction,
        UserManager<User> userManager,
        IEventBus eventBus)
    {
        _userManager = userManager;
        _eventBus = eventBus;
        _interaction = interaction;
    }

    public IActionResult OnGet(string? returnUrl)
    {
        Input = new InputModel { ReturnUrl = returnUrl };
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        // check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

        // the user clicked the "cancel" button
        if (Input.Button != "create")
        {
            if (context != null)
            {
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage(Input.ReturnUrl);
                }

                return Redirect(Input.ReturnUrl ?? "~/");
            }
            else
            {
                // since we don't have a valid context, then we just go back to the home page
                return Redirect("~/");
            }
        }

        if (await _userManager.FindByNameAsync(Input.Username) != null)
        {
            ModelState.AddModelError("Input.Username", "Username already exists");
        }

        if (ModelState.IsValid)
        {
            var user = new User(Input.Username, Input.Email!);
            var result = await _userManager.CreateAsync(user, Input.Password!);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _eventBus.PublishAsync(new SendEmailIntegrationEvent(
                    To: [user.Email!],
                    Subject: "Confirm your registration",
                    Body: $@"
                    <p>Hi {user.UserName},</p>
                    <p>Your email confirmation code is: <h3>{code}</h3></p>
                    <p>To confirm your registration, please click the link below:</p>
                    <p><a href='{Url.Page("/Account/Confirmation", null, new { email = user.Email }, Request.Scheme)}'>Confirm your email</a></p>
                    <p>If you did not register, please ignore this email.</p>
                    <p>Best regards,</p>
                    <p>DEEPIN</p>
                    <p>Note: This is an automated message, please do not reply.</p>
                    <p>Thank you for your understanding.</p>",
                    IsBodyHtml: true,
                    CC: null));
                return RedirectToPage("/Account/Confirmation/Index", new { email = user.Email });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            if (context != null)
            {
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage(Input.ReturnUrl);
                }

                // we can trust Input.ReturnUrl since GetAuthorizationContextAsync returned non-null
                return Redirect(Input.ReturnUrl ?? "~/");
            }

            // request for a local page
            if (Url.IsLocalUrl(Input.ReturnUrl))
            {
                return Redirect(Input.ReturnUrl);
            }
            else if (string.IsNullOrEmpty(Input.ReturnUrl))
            {
                return Redirect("~/");
            }
            else
            {
                // user might have clicked on a malicious link - should be logged
                throw new ArgumentException("invalid return URL");
            }
        }

        return Page();
    }
}

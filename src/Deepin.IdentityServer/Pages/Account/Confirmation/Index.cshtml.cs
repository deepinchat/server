using Deepin.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Deepin.IdentityServer.Pages.Account.Confirmation
{

    [SecurityHeaders]
    [AllowAnonymous]
    public class Index(UserManager<User> userManager) : PageModel
    {
        private readonly UserManager<User> _userManager = userManager;
        [BindProperty]
        public InputModel Input { get; set; } = default!;
        public void OnGet(string email, string? returnUrl)
        {
            Input = new InputModel
            {
                Email = email,
                ReturnUrl = returnUrl
            };
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                return RedirectToPage("/Account/Login/Index", new { returnUrl = Input.ReturnUrl });
            }
            var result = await _userManager.ConfirmEmailAsync(user, Input.Code);
            if (result.Succeeded)
            {
                return RedirectToPage("/Account/Login/Index", new { returnUrl = Input.ReturnUrl });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}

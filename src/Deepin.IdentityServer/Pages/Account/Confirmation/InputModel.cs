using System.ComponentModel.DataAnnotations;

namespace Deepin.IdentityServer.Pages.Account.Confirmation;

public class InputModel
{
    [Required]
    
    public string? Email { get; set; }

    [Required]
    public string? Code { get; set; }

    public string? ReturnUrl { get; set; }
}

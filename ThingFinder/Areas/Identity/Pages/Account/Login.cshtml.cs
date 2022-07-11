#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ThingFinder.Models.Identity;

namespace ThingFinder.Areas.Identity.Pages.Account
{
	public class LoginModel : PageModel
	{
		private readonly SignInManager<User> signInManager;
		private readonly ILogger<LoginModel> logger;

		public LoginModel(SignInManager<User> signInManager, ILogger<LoginModel> logger)
		{
			this.signInManager = signInManager;
			this.logger = logger;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public string ReturnUrl { get; set; }

		[TempData]
		public string ErrorMessage { get; set; }

		public class InputModel
		{
			[Required]
			[EmailAddress]
			public string Email { get; set; }

			[Required]
			[DataType(DataType.Password)]
			public string Password { get; set; }

			[Display(Name = "Remember me?")]
			public bool RememberMe { get; set; }
		}

		public async Task OnGetAsync(string returnUrl = null)
		{
			if (!string.IsNullOrEmpty(ErrorMessage))
				ModelState.AddModelError(string.Empty, ErrorMessage);

			returnUrl ??= Url.Content("~/");

			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			ReturnUrl = returnUrl;
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");

			if (ModelState.IsValid)
			{
				var result = await this.signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

				if (result.Succeeded)
				{
					this.logger.LogInformation("User logged in.");
					return LocalRedirect(returnUrl);
				}

				if (result.IsLockedOut)
				{
					this.logger.LogWarning("User account locked out.");
					return RedirectToPage("./Lockout");
				}

				ModelState.AddModelError(string.Empty, "Invalid login attempt.");
				return Page();
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}
	}
}
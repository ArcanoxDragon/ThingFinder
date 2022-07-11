// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using ThingFinder.Models.Identity;

namespace ThingFinder.Areas.Identity.Pages.Account
{
	public class RegisterModel : PageModel
	{
		private readonly SignInManager<User>    signInManager;
		private readonly UserManager<User>      userManager;
		private readonly IUserStore<User>       userStore;
		private readonly IUserEmailStore<User>  emailStore;
		private readonly ILogger<RegisterModel> logger;
		private readonly IEmailSender           emailSender;

		public RegisterModel(
			UserManager<User> userManager,
			IUserStore<User> userStore,
			SignInManager<User> signInManager,
			ILogger<RegisterModel> logger,
			IEmailSender emailSender)
		{
			this.userManager = userManager;
			this.userStore = userStore;
			this.emailStore = GetEmailStore();
			this.signInManager = signInManager;
			this.logger = logger;
			this.emailSender = emailSender;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public string ReturnUrl { get; set; }

		public class InputModel
		{
			[Required]
			[EmailAddress]
			[Display(Name = "Email")]
			public string Email { get; set; }

			[Required]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }

			[DataType(DataType.Password)]
			[Display(Name = "Confirm password")]
			[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; }
		}

		public void OnGet(string returnUrl = null)
		{
			ReturnUrl = returnUrl;
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");

			if (ModelState.IsValid)
			{
				var user = CreateUser();

				await this.userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
				await this.emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
				var result = await this.userManager.CreateAsync(user, Input.Password);

				if (result.Succeeded)
				{
					this.logger.LogInformation("User created a new account with password.");

					var userId = await this.userManager.GetUserIdAsync(user);
					var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

					code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

					var callbackUrl = Url.Page(
						"/Account/ConfirmEmail",
						pageHandler: null,
						values: new { area = "Identity", userId, code, returnUrl },
						protocol: Request.Scheme);

					await this.emailSender.SendEmailAsync(
						Input.Email,
						"Confirm your email",
						$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

					if (this.userManager.Options.SignIn.RequireConfirmedAccount)
						return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });

					await this.signInManager.SignInAsync(user, isPersistent: false);

					return LocalRedirect(returnUrl);
				}

				foreach (var error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}

		private User CreateUser()
		{
			try
			{
				return Activator.CreateInstance<User>();
			}
			catch
			{
				throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
													$"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
													$"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
			}
		}

		private IUserEmailStore<User> GetEmailStore()
		{
			if (!this.userManager.SupportsUserEmail)
			{
				throw new NotSupportedException("The default UI requires a user store with email support.");
			}

			return (IUserEmailStore<User>) this.userStore;
		}
	}
}
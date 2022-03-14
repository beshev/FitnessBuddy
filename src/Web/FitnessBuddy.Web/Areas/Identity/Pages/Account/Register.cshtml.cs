namespace FitnessBuddy.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using FitnessBuddy.Common;
    using FitnessBuddy.Data.Models;
    using FitnessBuddy.Data.Models.Enums;
    using FitnessBuddy.Services.Data.Users;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IUsersService usersService;
        private readonly IEmailSender emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<RegisterModel> logger,
            IUsersService usersService,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.logger = logger;
            this.usersService = usersService;
            this.emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(DataConstants.UserUsernameMaxLength, MinimumLength = DataConstants.UserUsernameMinLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
            public string Username { get; set; }

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

            public GenderType Gender { get; set; }

            [Range(DataConstants.UserWeightMinValue, DataConstants.UserWeightMaxValue)]
            [Display(Name = "Current weights (in kg)")]
            public double CurrentWeight { get; set; }

            [Range(DataConstants.UserWeightMinValue, DataConstants.UserWeightMaxValue)]
            [Display(Name = "Goal weights (in kg)")]
            public double GoalWeight { get; set; }

            [Range(DataConstants.UserHeightMinValue, DataConstants.UserHeightMaxValue)]
            [Display(Name = "Height (in cm)")]
            public double Height { get; set; }

            [Range(DataConstants.UserDailyNutritionsMinValue, DataConstants.UserDailyNutritionsMaxValue)]
            [Display(Name = "Daily Protein Goal")]
            public double DailyProteinGoal { get; set; }

            [Range(DataConstants.UserDailyNutritionsMinValue, DataConstants.UserDailyNutritionsMaxValue)]
            [Display(Name = "Daily Carbohydrates Goal")]
            public double DailyCarbsGoal { get; set; }

            [Range(DataConstants.UserDailyNutritionsMinValue, DataConstants.UserDailyNutritionsMaxValue)]
            [Display(Name = "Daily Fat Goal")]
            public double DailyFatGoal { get; set; }

            [Display(Name = "About me")]
            public string AboutMe { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= this.Url.Content("~/");
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (this.ModelState.IsValid)
            {
                if (this.usersService.IsUsernameExist(this.Input.Username))
                {
                    this.ModelState.AddModelError(nameof(this.Input.Username), "There is already user with that username.");
                    return this.Page();
                }

                var user = new ApplicationUser
                {
                    UserName = this.Input.Username,
                    Email = this.Input.Email,
                    WeightInKg = this.Input.CurrentWeight,
                    GoalWeightInKg = this.Input.GoalWeight,
                    HeightInCm = this.Input.Height,
                    DailyProteinGoal = this.Input.DailyProteinGoal,
                    DailyCarbohydratesGoal = this.Input.DailyCarbsGoal,
                    DailyFatGoal = this.Input.DailyFatGoal,
                    Gender = this.Input.Gender,
                    AboutMe = this.Input.AboutMe,
                };

                var result = await this.userManager.CreateAsync(user, this.Input.Password);
                if (result.Succeeded)
                {
                    this.logger.LogInformation("User created a new account with password.");

                    await this.userManager.AddToRoleAsync(user, GlobalConstants.UserRoleName);

                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = this.Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: this.Request.Scheme);

                    await this.emailSender.SendEmailAsync(
                        this.Input.Email,
                        "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await this.signInManager.SignInAsync(user, isPersistent: false);
                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}

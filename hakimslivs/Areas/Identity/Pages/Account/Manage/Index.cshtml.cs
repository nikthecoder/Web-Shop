using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using hakimslivs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace hakimslivs.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [TempData]
        public string UserNameChangeLimitMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(50, MinimumLength = 2)]
            [Display(Name = "Alias (Username)")]
            public string Username { get; set; }

            [Required]
            [StringLength(50, MinimumLength = 2)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            [StringLength(50, MinimumLength = 2)]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Street")]
            [StringLength(50, MinimumLength = 2)]
            public string Street { get; set; }

            [Required]
            [Display(Name = "Street number")]
            [RegularExpression("([0-9]+)")]
            public int StreetNumber { get; set; }

            [Required]
            [Display(Name = "Postal Code")]
            [RegularExpression("(^[0-9]{5})")]
            public int PostalCode { get; set; }

            [Required]
            [Display(Name = "City")]
            [StringLength(50, MinimumLength = 1)]
            public string City { get; set; }

            [Required]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            var street = user.Street;
            var streetNumber = user.StreetNumber;
            var postalCode = user.PostalCode;
            var city = user.City;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Username = userName,
                FirstName = firstName,
                LastName = lastName,
                Street = street,
                StreetNumber = streetNumber,
                PostalCode = postalCode,
                City = city,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            UserNameChangeLimitMessage = $"You can change your username {user.UsernameChangeLimit} more time(s).";

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            var firstName = user.FirstName;
            var lastName = user.LastName;
            var street = user.Street;
            var streetNumber = user.StreetNumber;
            var postalCode = user.PostalCode;
            var city = user.City;


            if (Input.FirstName != firstName)
            {
                user.FirstName = Input.FirstName;
                await _userManager.UpdateAsync(user);
            }
            if (Input.LastName != lastName)
            {
                user.LastName = Input.LastName;
                await _userManager.UpdateAsync(user);
            }
            if (Input.Street != street)
            {
                user.Street = Input.Street;
                await _userManager.UpdateAsync(user);
            }
            if (Input.StreetNumber != streetNumber)
            {
                user.StreetNumber = Input.StreetNumber;
                await _userManager.UpdateAsync(user);
            }
            if (Input.PostalCode != postalCode)
            {
                user.PostalCode = Input.PostalCode;
                await _userManager.UpdateAsync(user);
            }
            if (Input.City != city)
            {
                user.City = Input.City;
                await _userManager.UpdateAsync(user);
            }

            if (user.UsernameChangeLimit > 0)
            {
                if (Input.Username != user.UserName)
                {
                    var userNameExists = await _userManager.FindByNameAsync(Input.Username);
                    if (userNameExists != null)
                    {
                        StatusMessage = "User name already taken. Select a different username.";
                        return RedirectToPage();
                    }
                    var setUserName = await _userManager.SetUserNameAsync(user, Input.Username);
                    if (!setUserName.Succeeded)
                    {
                        StatusMessage = "Unexpected error when trying to set user name.";
                        return RedirectToPage();
                    }
                    else
                    {
                        user.UsernameChangeLimit -= 1;
                        await _userManager.UpdateAsync(user);

                        UserNameChangeLimitMessage = $"You can change your username {user.UsernameChangeLimit} more time(s).";
                    }
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}

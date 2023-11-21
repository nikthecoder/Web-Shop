using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hakimslivs.Models
{
    public enum Roles
    {
        SuperAdmin,
        Admin,
        Moderator,
        Basic
    }
    public class ApplicationUser : IdentityUser
    {
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

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        public int UsernameChangeLimit { get; set; } = 10;
    }
}

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ECommerce.Application.Core.Interfaces;
using ECommerceServer.Application.Core.DTOs.Enumerations;

namespace ECommerce.Application.Core.Entities
{
    public class User: BaseEntity
    {
        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^\w]).{8,25}",
            ErrorMessage = "Password must contain both upper and lower case with atleast " +
            "one digit and special character, length in between 8-25")]
        public string Password { get; set; }

        [Required]
        [MaxLength(30)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(320)] // Found max length of email as 320 in google
        [DisplayName("E-mail")]
        public string Email { get; set; } // Required unique

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Date of Birth")]
        public DateTime DOB { get; set; }

        [Required]
        [DisplayName("Role")]
        public Role Role { get; set; }

        [DataType(DataType.Currency)]
        [DisplayName("Wallet")]
        public double Wallet { get; set; }
    }
}

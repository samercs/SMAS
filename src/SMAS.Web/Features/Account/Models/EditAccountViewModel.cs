using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OrangeJetpack.Regionalization.Models;
using SMAS.Web.Features.Shared;

namespace SMAS.Web.Features.Account.Models
{
    public class EditAccountViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        public PhoneNumberViewModel PhoneNumberViewModel { get; set; }
    }
}

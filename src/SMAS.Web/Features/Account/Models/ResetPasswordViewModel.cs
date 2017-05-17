using OrangeJetpack.Core.Security;
using System.ComponentModel.DataAnnotations;

namespace SMAS.Web.Features.Account.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public UrlTokenParameters TokenParameters { get; set; }
    }
}

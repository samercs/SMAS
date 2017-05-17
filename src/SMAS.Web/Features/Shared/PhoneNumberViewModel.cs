using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrangeJetpack.Regionalization.Models;

namespace SMAS.Web.Features.Shared
{
    public class PhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Country/Region")]
        public string PhoneCountryCode { get; set; }
        [StringLength(15, MinimumLength = 7)]
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneLocalNumber { get; set; }

        public IList<Country> Countries { get; set; }

        public IList<SelectListItem> CountriesOptions
        {
            get
            {
                return Countries?.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.PhoneCountryCode,
                    Selected = i.PhoneCountryCode == PhoneCountryCode
                }).ToList();
            }
        }

        public static PhoneNumberViewModel Create(IList<Country> countries)
        {
            return new PhoneNumberViewModel
            {
                Countries = countries
            };
        }
    }
}

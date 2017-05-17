using System.ComponentModel.DataAnnotations;

namespace SMAS.Entities
{
    public class Address : EntityBase
    {
        public int AddressId { get; set; }

        [Required]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }

        [Required]
        public string CityArea { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }

        [Required]
        public string CountryCode { get; set; }
    }
}

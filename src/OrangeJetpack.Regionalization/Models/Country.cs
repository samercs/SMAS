namespace OrangeJetpack.Regionalization.Models
{
    public class Country
    {
        public string IsoCode { get; set; }
        public string Name { get; set; }
        public string PhoneCountryCode { get; set; }

        public Country(string isoCode, string name, string phoneCountryCode)
        {
            IsoCode = isoCode;
            Name = name;
            PhoneCountryCode = phoneCountryCode;
        }
    }
}

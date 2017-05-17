using System.Collections.Generic;
using OrangeJetpack.Localization;

namespace OrangeJetpack.Regionalization.Models
{
    public class Governorate: ILocalizable
    {
        public int GovernorateId { get; set; }

        [Localized]
        public string Name { get; set; }

        public IReadOnlyCollection<Area> Areas { get; set; }

        public Governorate()
        {

        }

        internal Governorate(int governorateId, string englishName, string arabicName, params Area[] areas)
        {
            GovernorateId = governorateId;
            Areas = areas;
            this.Set(i => i.Name, new[]
            {
                new LocalizedContent("en", englishName),
                new LocalizedContent("ar", arabicName)
            });
        }
    }
}

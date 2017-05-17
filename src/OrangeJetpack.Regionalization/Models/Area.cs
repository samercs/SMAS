using OrangeJetpack.Localization;

namespace OrangeJetpack.Regionalization.Models
{
    public class Area: ILocalizable
    {
        public int AreaId { get; set; }

        [Localized]
        public string Name { get; set; }

        public Area()
        {

        }

        internal Area(int areaId, string englishName, string arabicName)
        {
            AreaId = areaId;

            this.Set(i => i.Name, new[]
            {
                new LocalizedContent("en", englishName),
                new LocalizedContent("ar", arabicName)
            });
        }
    }
}

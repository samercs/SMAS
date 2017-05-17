using System.Collections.Generic;
using OrangeJetpack.Localization;
using OrangeJetpack.Regionalization.Models;
using System.Linq;

namespace OrangeJetpack.Regionalization
{
    public class Kuwait
    {
        /// <summary>
        /// Gets a list of all governorates and areas in Kuwait
        /// </summary>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public static IReadOnlyCollection<Governorate> GetGovernates(string languageCode)
        {
            var governorates = Governorates.Localize<Governorate>(languageCode, LocalizationDepth.OneLevel)
                .OrderBy(i => i.Name)
                .ToList();

            foreach (var governorate in Governorates)
            {
                governorate.Areas = governorate.Areas.OrderBy(i => i.Name).ToList();
            }

            return governorates;
        }

        /// <summary>
        /// Gets a list of all areas in Kuwait
        /// </summary>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public static IReadOnlyCollection<Area> GetAreas(string languageCode)
        {
            var areas = Governorates.SelectMany(i => i.Areas);
            return areas.Localize(languageCode)
                .OrderBy(i => i.Name)
                .ToList();
        }

        /// <summary>
        /// Gets an alphabetized list of all areas in Kuwait.
        /// </summary>
        public static IReadOnlyCollection<string> GetAreaList()
        {
            var en = Governorates.SelectMany(i => i.Areas).Localize("en");
            var ar = Governorates.SelectMany(i => i.Areas).Localize("ar");

            return en.Concat(ar).OrderBy(i => i.Name)
                .Select(i => i.Name)
                .OrderBy(i => i)
                .ToList();
        }

        private static readonly Governorate[] Governorates =
        {
            // ==========================  Kuwait ==========================================
            new Governorate(1, "Al Asimah", "«·ﬁ”Ì„…",
                new Area(1, "Kuwait City", "„œÌ‰… «·ﬂÊÌ "),
                new Area(2, "Dasman", "œ”„«‰"),
                new Area(3, "Sharq", "‘—ﬁ"),
                new Area(4, "Mirgab", "«·„—ﬁ«»"),
                new Area(5, "Jibla", "Ã»·…"),
                new Area(6, "Dasma", "«·œ”„…"),
                new Area(7, "Daiya", "«·œ⁄Ì…"),
                new Area(8, "Sawabir", "«·’Ê«»—"),
                new Area(9, "Salhiya", "«·’«·ÕÌ…"),
                new Area(10, "Bneid il-Gar", "»‰Ìœ «·ﬁ«—"),
                new Area(11, "Kaifan", "ﬂÌ›«‰"),
                new Area(12, "Mansuriya", "«·„‰’Ê—Ì…"),
                new Area(13, "Abdullah as-Salim suburb", "÷«ÕÌ… ⁄»œ «··Â «·”«·„"),
                new Area(14, "Nuzha", "«·‰“Â…"),
                new Area(15, "Faiha", "«·›ÌÕ«¡"),
                new Area(16, "Shamiya", "«·‘«„Ì…"),
                new Area(17, "Rawda", "«·—Ê÷…"),
                new Area(18, "Adiliya", "«·⁄œÌ·Ì…"),
                new Area(19, "Khaldiya", "«·Œ«·œÌ…"),
                new Area(20, "Qadsiya", "«·ﬁ«œ”Ì…"),
                new Area(21, "Qurtuba", "ﬁ—ÿ»…"),
                new Area(22, "Surra", "«·”—…"),
                new Area(23, "Yarmuk", "«·Ì—„Êﬂ"),
                new Area(24, "Shuwaikh", "«·‘ÊÌŒ"),
                new Area(25, "Rai", "«·—Ì"),
                new Area(26, "Ghirnata", "€—‰«ÿ…"),
                new Area(27, "Sulaibikhat", "«·’·Ì»Œ« "),
                new Area(28, "Doha", "«·œÊÕ…"),
                new Area(29, "Nahdha", "«·‰Â÷…"),
                new Area(30, "Jabir al-Ahmad City", "„œÌ‰… Õ«»— «·√Õ„œ"),
                new Area(31, "Qairawn", "«·ﬁÌ—Ê«‰")
                ),
            // ==========================  Hawalli ==========================================
            new Governorate(2, "Hawalli", "ÕÊ·Ì",
                new Area(32, "Hawally", "ÕÊ·Ì"),
                new Area(33, "Rumaithiya", "«·—„ÌÀÌ…"),
                new Area(34, "Jabriya", "«·Ã«»—Ì…"),
                new Area(35, "Salmiya", "«·”«·„Ì…"),
                new Area(36, "Mishrif", "„‘—›"),
                new Area(37, "Shaab", "«·‘⁄»"),
                new Area(38, "Bayan", "»Ì«‰"),
                new Area(39, "Bidi", "«·»œ⁄"),
                new Area(40, "Nigra", "«·‰ﬁ—…"),
                new Area(41, "Salwa", "”·ÊÏ"),
                new Area(42, "Maidan Hawalli", "„Ìœ«‰ ÕÊ·Ì"),
                new Area(43, "Mubarak aj-Jabir suburb", "÷«ÕÌ… „»«—ﬂ «·Ã«»—"),
                new Area(44, "South Surra", "Ã‰Ê» «·”—…"),
                new Area(45, "Hittin", "ÕÿÌ‰")
                ),
            // ==========================   Farwaniya ==========================================
            new Governorate(3, "Farwaniya", "«·›—Ê«‰Ì…",
                new Area(46, "Abraq Khaitan", "√»—ﬁ ŒÌÿ«‰"),
                new Area(47, "Al Andalus", "«·√‰œ·”"),
                new Area(48, "Ishbilia", "≈‘»Ì·Ì…"),
                new Area(49, "Jleeb Al Shouyouk", "Ã·Ì» «·‘ÌÊŒ"),
                new Area(50, "Omariya", "«·⁄„—Ì…"),
                new Area(51, "Ardiya", "«·⁄«—÷Ì…"),
                new Area(52, "ndustrial Ardiya", "«·⁄«—÷Ì… «·’‰«⁄Ì…"),
                new Area(53, "Fordous", "«·›—œÊ”"),
                new Area(54, "Farwaniya", "«·›—Ê«‰Ì…"),
                new Area(55, "Shadadiya", "«·‘œ«œÌ…"),
                new Area(56, "Rihab", "«·—Õ«»"),
                new Area(57, "Rabiya", "«·—«»Ì…"),
                new Area(58, "Industrial Rai", "«·—Ì «·’‰«⁄Ì…"),
                new Area(59, "Abdullah Al Mubarak", "⁄»œ «··Â «·„»«—ﬂ"),
                new Area(60, "Dajeej", "«·÷ÃÌÃ"),
                new Area(61, "South Khaitan", "Ã‰Ê» ŒÌÿ«‰")
                ),
            // ==========================  Mubarak al-Kabeer ==========================================
            new Governorate(4,"Mubarak al-Kabeer","„»«—ﬂ «·ﬂ»Ì—",
                new Area(62, "Mubarak al-Kabeer","„»«—ﬂ «·ﬂ»Ì—"),
                new Area(63, "Adan","«·⁄œ«‰"),
                new Area(64, "Qurain","«·ﬁ—Ì‰"),
                new Area(65, "Qusur","«·ﬁ’Ê—"),
                new Area(66, "Sabah as-Salim suburb","÷«ÕÌ… ’»«Õ «·”«·„"),
                new Area(67, "Misila","«·„”Ì·…"),
                new Area(68, "Abu Fteira","√»Ê ›ÿÌ—…"),
                new Area(69, "Abu Al Hasaniya","√»Ê «·Õ’«‰Ì…"),
                new Area(70, "Sabhan","’»Õ«‰"),
                new Area(71, "Fintas","«·›‰ÿ«”"),
                new Area(72, "Funaitis","«·›‰ÌÿÌ”")
                ),
            // ==========================  Ahmadi ==========================================
            new Governorate(5,"Ahmadi","«·√Õ„œÌ",
                new Area(73, "Ahmadi","«·√Õ„œÌ"),
                new Area(74, "Aqila","«·⁄ﬁÌ·…"),
                new Area(75, "Zuhar","«·ŸÂ—"),
                new Area(76, "Miqwa","«·„ﬁÊ⁄"),
                new Area(77, "Mahbula","«·„Â»Ê·…"),
                new Area(78, "Rigga","«·—ﬁ…"),
                new Area(97, "Hadiya","ÂœÌ…"),
                new Area(80, "Abu Hulaifa","√»Ê Õ·Ì›…"),
                new Area(81, "Sabahiya","«·’»«ÕÌ…"),
                new Area(82, "Mangaf","«·„‰ﬁ›"),
                new Area(83, "Fahaheel","«·›ÕÌÕÌ·"),
                new Area(84, "Wafra","«·Ê›—…"),
                new Area(85, "Zoor","«·“Ê—"),
                new Area(86, "Khairan","«·ŒÌ—«‰"),
                new Area(87, "Abdullah Port","„Ì‰«¡ ⁄»œ «··Â"),
                new Area(88, "Agricultural Wafra","«·Ê›—… «·“—«⁄Ì…"),
                new Area(89, "Jileia","«·Ã·Ì⁄…"),
                new Area(90, "Jabir al-Ali Suburb","÷«ÕÌ… Ã«»— «·⁄·Ì"),
                new Area(91, "Fahd al-Ahmad Suburb","÷«ÕÌ… ›Âœ «·√Õ„œ"),
                new Area(92, "Shuaiba","«·‘⁄Ì»…"),
                new Area(93, "Sabah al-Ahmad City","„œÌ‰… ’»«Õ «·√Õ„œ"),
                new Area(94, "Nuwaiseeb","«·‰ÊÌ’Ì»"),
                new Area(95, "Khairan City","„œÌ‰… «·ŒÌ—«‰	"),
                new Area(96, "Ali as-Salim suburb","÷«ÕÌ… ⁄·Ì ’»«Õ «·”«·„"),
                new Area(97, "Sabah al-Ahmad Nautical City","„œÌ‰… ’»«Õ «·√Õ„œ «·»Õ—Ì…"),
                new Area(98, "Funaitis","«·›‰ÌÿÌ”")
                ), 
            // ==========================  Jahra  ==========================================
            new Governorate(5,"Jahra","«·ÃÂ—«¡",
                new Area(99, "Al Salibia","«·’·Ì»Ì…"),
                new Area(100, "Amghra","√„€—…"),
                new Area(101, "Al Naaem","«·‰⁄Ì„"),
                new Area(102, "Al Qasr","«·ﬁ’—"),
                new Area(103, "Al Waha","«·Ê«Õ…"),
                new Area(104, "Taimaa"," Ì„«¡"),
                new Area(105, "Al Nasem","«·‰”Ì„"),
                new Area(106, "Al Euon","«·⁄ÌÊ‰"),
                new Area(107, "Bobyan Island","Ã“Ì—… »Ê»Ì«‰"),
                new Area(108, "Warba Island","Ã“Ì—… Ê—»…"),
                new Area(109, "Al Qaysriya","«·ﬁÌ’—Ì…"),
                new Area(110, "Al Abdly","«·⁄»œ·Ì"),
                new Area(111, "Old Jahra","«·ÃÂ—«¡ «·ﬁœÌ„…"),
                new Area(112, "New Jahra","«·ÃÂ—«¡ «·ÃœÌœ…"),
                new Area(113, "Kazma","ﬂ«Ÿ„…"),
                new Area(114, "Saad Alabdallah City","„œÌ‰… ”⁄œ «·⁄»œ «··Â"),
                new Area(115, "Al Salmy","«·”«·„Ì"),
                new Area(116, "Al Mtlaa","«·„ÿ·«⁄"),
                new Area(117, "Al Hariri City","„œÌ‰… «·Õ—Ì—"),
                new Area(118, "Kabd","ﬂ»œ"),
                new Area(119, "Al Rawdten","«·—Ê÷ Ì‰"),
                new Area(120, "Al Sabiya","«·’»Ì…")
                )
        };
    }
}

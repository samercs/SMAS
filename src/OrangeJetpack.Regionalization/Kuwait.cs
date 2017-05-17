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
            new Governorate(1, "Al Asimah", "�������",
                new Area(1, "Kuwait City", "����� ������"),
                new Area(2, "Dasman", "�����"),
                new Area(3, "Sharq", "���"),
                new Area(4, "Mirgab", "�������"),
                new Area(5, "Jibla", "����"),
                new Area(6, "Dasma", "������"),
                new Area(7, "Daiya", "������"),
                new Area(8, "Sawabir", "�������"),
                new Area(9, "Salhiya", "��������"),
                new Area(10, "Bneid il-Gar", "���� �����"),
                new Area(11, "Kaifan", "�����"),
                new Area(12, "Mansuriya", "���������"),
                new Area(13, "Abdullah as-Salim suburb", "����� ��� ���� ������"),
                new Area(14, "Nuzha", "������"),
                new Area(15, "Faiha", "�������"),
                new Area(16, "Shamiya", "�������"),
                new Area(17, "Rawda", "������"),
                new Area(18, "Adiliya", "��������"),
                new Area(19, "Khaldiya", "��������"),
                new Area(20, "Qadsiya", "��������"),
                new Area(21, "Qurtuba", "�����"),
                new Area(22, "Surra", "�����"),
                new Area(23, "Yarmuk", "�������"),
                new Area(24, "Shuwaikh", "������"),
                new Area(25, "Rai", "����"),
                new Area(26, "Ghirnata", "������"),
                new Area(27, "Sulaibikhat", "���������"),
                new Area(28, "Doha", "������"),
                new Area(29, "Nahdha", "������"),
                new Area(30, "Jabir al-Ahmad City", "����� ���� ������"),
                new Area(31, "Qairawn", "��������")
                ),
            // ==========================  Hawalli ==========================================
            new Governorate(2, "Hawalli", "����",
                new Area(32, "Hawally", "����"),
                new Area(33, "Rumaithiya", "��������"),
                new Area(34, "Jabriya", "��������"),
                new Area(35, "Salmiya", "��������"),
                new Area(36, "Mishrif", "����"),
                new Area(37, "Shaab", "�����"),
                new Area(38, "Bayan", "����"),
                new Area(39, "Bidi", "�����"),
                new Area(40, "Nigra", "������"),
                new Area(41, "Salwa", "����"),
                new Area(42, "Maidan Hawalli", "����� ����"),
                new Area(43, "Mubarak aj-Jabir suburb", "����� ����� ������"),
                new Area(44, "South Surra", "���� �����"),
                new Area(45, "Hittin", "����")
                ),
            // ==========================   Farwaniya ==========================================
            new Governorate(3, "Farwaniya", "���������",
                new Area(46, "Abraq Khaitan", "���� �����"),
                new Area(47, "Al Andalus", "�������"),
                new Area(48, "Ishbilia", "�������"),
                new Area(49, "Jleeb Al Shouyouk", "���� ������"),
                new Area(50, "Omariya", "�������"),
                new Area(51, "Ardiya", "��������"),
                new Area(52, "ndustrial Ardiya", "�������� ��������"),
                new Area(53, "Fordous", "�������"),
                new Area(54, "Farwaniya", "���������"),
                new Area(55, "Shadadiya", "��������"),
                new Area(56, "Rihab", "������"),
                new Area(57, "Rabiya", "�������"),
                new Area(58, "Industrial Rai", "���� ��������"),
                new Area(59, "Abdullah Al Mubarak", "��� ���� �������"),
                new Area(60, "Dajeej", "������"),
                new Area(61, "South Khaitan", "���� �����")
                ),
            // ==========================  Mubarak al-Kabeer ==========================================
            new Governorate(4,"Mubarak al-Kabeer","����� ������",
                new Area(62, "Mubarak al-Kabeer","����� ������"),
                new Area(63, "Adan","������"),
                new Area(64, "Qurain","������"),
                new Area(65, "Qusur","������"),
                new Area(66, "Sabah as-Salim suburb","����� ���� ������"),
                new Area(67, "Misila","�������"),
                new Area(68, "Abu Fteira","��� �����"),
                new Area(69, "Abu Al Hasaniya","��� ��������"),
                new Area(70, "Sabhan","�����"),
                new Area(71, "Fintas","�������"),
                new Area(72, "Funaitis","��������")
                ),
            // ==========================  Ahmadi ==========================================
            new Governorate(5,"Ahmadi","�������",
                new Area(73, "Ahmadi","�������"),
                new Area(74, "Aqila","�������"),
                new Area(75, "Zuhar","�����"),
                new Area(76, "Miqwa","������"),
                new Area(77, "Mahbula","��������"),
                new Area(78, "Rigga","�����"),
                new Area(97, "Hadiya","����"),
                new Area(80, "Abu Hulaifa","��� �����"),
                new Area(81, "Sabahiya","��������"),
                new Area(82, "Mangaf","������"),
                new Area(83, "Fahaheel","��������"),
                new Area(84, "Wafra","������"),
                new Area(85, "Zoor","�����"),
                new Area(86, "Khairan","�������"),
                new Area(87, "Abdullah Port","����� ��� ����"),
                new Area(88, "Agricultural Wafra","������ ��������"),
                new Area(89, "Jileia","�������"),
                new Area(90, "Jabir al-Ali Suburb","����� ���� �����"),
                new Area(91, "Fahd al-Ahmad Suburb","����� ��� ������"),
                new Area(92, "Shuaiba","�������"),
                new Area(93, "Sabah al-Ahmad City","����� ���� ������"),
                new Area(94, "Nuwaiseeb","��������"),
                new Area(95, "Khairan City","����� �������	"),
                new Area(96, "Ali as-Salim suburb","����� ��� ���� ������"),
                new Area(97, "Sabah al-Ahmad Nautical City","����� ���� ������ �������"),
                new Area(98, "Funaitis","��������")
                ), 
            // ==========================  Jahra  ==========================================
            new Governorate(5,"Jahra","�������",
                new Area(99, "Al Salibia","��������"),
                new Area(100, "Amghra","�����"),
                new Area(101, "Al Naaem","������"),
                new Area(102, "Al Qasr","�����"),
                new Area(103, "Al Waha","������"),
                new Area(104, "Taimaa","�����"),
                new Area(105, "Al Nasem","������"),
                new Area(106, "Al Euon","������"),
                new Area(107, "Bobyan Island","����� ������"),
                new Area(108, "Warba Island","����� ����"),
                new Area(109, "Al Qaysriya","��������"),
                new Area(110, "Al Abdly","�������"),
                new Area(111, "Old Jahra","������� �������"),
                new Area(112, "New Jahra","������� �������"),
                new Area(113, "Kazma","�����"),
                new Area(114, "Saad Alabdallah City","����� ��� ����� ����"),
                new Area(115, "Al Salmy","�������"),
                new Area(116, "Al Mtlaa","�������"),
                new Area(117, "Al Hariri City","����� ������"),
                new Area(118, "Kabd","���"),
                new Area(119, "Al Rawdten","��������"),
                new Area(120, "Al Sabiya","������")
                )
        };
    }
}

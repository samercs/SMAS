using System.Text.RegularExpressions;

namespace OrangeJetpack.Base.Core.Formatting
{
    public class StringFormatter
    {
        /// <summary>
        /// Compiled regular expression for performance.
        /// </summary>
        static readonly Regex NotDigitsRegex = new Regex(@"[^(0-9|/\u0660-\u0669/)]", RegexOptions.Compiled);

        /// <summary>
        /// Gets a string with all non-numeric digits removed.
        /// </summary>
        public static string StripNonDigits(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return NotDigitsRegex.Replace(input, "").Trim();
        }
    }
}

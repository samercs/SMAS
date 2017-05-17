using System.Collections.Generic;
using System.Linq;

namespace SMAS.Notifications
{
    public static class StringExtensions
    {
        public static string ReplaceParameters(this string value, Dictionary<string, string> parameters)
        {
            return parameters.Aggregate(value, (current, parameter) => current.Replace(parameter.Key, parameter.Value));
        }
    }
}

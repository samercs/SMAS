using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SMAS.Web.Core.TagHelpers
{
    [HtmlTargetElement("gravatar")]
    public class GravatarTagHelper : TagHelper
    {
        private const string EmailAttributeName = "email";
        private const string SizeAttributeName = "size";
        private const string AltTextAttributeName = "alt";

        private const string GravatarBaseUrl = "https://www.gravatar.com/avatar";

        [HtmlAttributeName(EmailAttributeName)]
        public string Email { get; set; }

        [HtmlAttributeName(AltTextAttributeName)]
        public string AltText { set; get; }

        [HtmlAttributeName(SizeAttributeName)]
        public int? Size { get; set; }

        private static string ToGravatarHash(string email)
        {
            var encoder = new UTF8Encoding();
            var md5 = MD5.Create();
            var hashedBytes = md5.ComputeHash(encoder.GetBytes(email.ToLower()));
            var sb = new StringBuilder(hashedBytes.Length * 2);

            foreach (var b in hashedBytes)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString().ToLower();
        }

        private string ToGravatarUrl(string email, int? size)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(email.Trim()))
            {
                throw new ArgumentException("The email is empty.", nameof(email));
            }

            var hash = ToGravatarHash(email);

            var imageUrl = $"{GravatarBaseUrl}/{hash}?d=mm";

            if (size.HasValue)
            {
                imageUrl += "&s=" + size.Value;
            }

            return imageUrl;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var str = new StringBuilder();
            var url = ToGravatarUrl(Email, Size);
            str.AppendFormat("<img class='gravatar' src='{0}' alt='{1}' />", url, AltText);
            output.Content.AppendHtml(str.ToString());
        }
    }
}

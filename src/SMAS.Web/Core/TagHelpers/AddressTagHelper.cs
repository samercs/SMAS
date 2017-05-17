using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SMAS.Entities;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SMAS.Web.Core.TagHelpers
{
    [HtmlTargetElement("address-template", Attributes = "address")]
    public class AddressTagHelper : TagHelper
    {
        private readonly HtmlHelper _htmlHelper;
        private readonly HtmlEncoder _htmlEncoder;

        public AddressTagHelper(IHtmlHelper htmlHelper, HtmlEncoder htmlEncoder)
        {
            _htmlHelper = htmlHelper as HtmlHelper;
            _htmlEncoder = htmlEncoder;
        }

        [ViewContext]
        public ViewContext ViewContext
        {
            set
            {
                _htmlHelper.Contextualize(value);
            }
        }

        public Address Address { get; set; }


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            var partial = await _htmlHelper.PartialAsync("AddressTemplate", Address);
            var writer = new StringWriter();
            partial.WriteTo(writer, _htmlEncoder);
            output.Content.SetHtmlContent(writer.ToString());
        }
    }
}

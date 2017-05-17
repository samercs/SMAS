using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SMAS.Web.Core.TagHelpers
{
    [HtmlTargetElement("form-group")]
    public class FormGroup : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "form-group");
        }
    }
}

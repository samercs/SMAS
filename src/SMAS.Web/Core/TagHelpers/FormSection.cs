using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SMAS.Web.Core.TagHelpers
{
    [HtmlTargetElement("form-section")]
    public class FormSection : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "mdl-card mdl-card--expand mdl-shadow--6dp");
            output.PreContent.SetHtmlContent("<div class='mdl-card__supporting-text'>");
            output.PostContent.SetHtmlContent("</div>");
        }
    }
}

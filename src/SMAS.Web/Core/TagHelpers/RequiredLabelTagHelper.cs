using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SMAS.Web.Core.TagHelpers
{
    [HtmlTargetElement("label")]
    public class RequiredLabelTagHelper : TagHelper
    {
        [HtmlAttributeName("required")]
        public bool Required { get; set; }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "label";

            if (For != null)
            {
                output.Attributes.Add("for", For.Name);
            }

            if (Required)
            {
                output.PostContent.SetHtmlContent("<span class='required-label'><i class='fa fa-pad-right fa-asterisk'></i></span>");
            }


        }
    }
}

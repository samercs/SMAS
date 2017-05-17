using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SMAS.Web.Core.TagHelpers
{
    [HtmlTargetElement("form-wrapper")]
    public class FormWrapper : TagHelper
    {
        [HtmlAttributeName("columns")]
        public int Columns { get; set; } = 6;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string colClass;

            switch (Columns)
            {
                case 6:
                    colClass = "col-md-6 col-md-offset-3";
                    break;
                case 8:
                    colClass = "col-md-8 col-md-offset-2";
                    break;
                case 10:
                    colClass = "col-md-10 col-md-offset-1";
                    break;
                default:
                    colClass = "col-md-6 col-md-offset-3";
                    break;
            }

            output.TagName = "div";
            output.Attributes.Add("class", "row");
            output.PreContent.SetHtmlContent($"<div class='{colClass}'>");
            output.PostContent.SetHtmlContent("</div>");
        }
    }
}

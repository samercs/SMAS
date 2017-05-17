using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace SMAS.Web.Core.TagHelpers
{
    [HtmlTargetElement("date-time", Attributes = "date")]
    public class DateTimeTagHelper : TagHelper
    {
        private readonly HtmlHelper _htmlHelper;

        public DateTimeTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper as HtmlHelper;
        }

        [ViewContext]
        public ViewContext ViewContext
        {
            set
            {
                _htmlHelper.Contextualize(value);
            }
        }

        public DateTime? Date { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "input";
            output.Attributes.Add("class", "form-control");
            output.Attributes.Add("data-type", "date");
            output.Attributes.Add("id", Id);
            output.Attributes.Add("name", Name);
            output.Attributes.Add("type", "datetime");
            output.Attributes.Add("value", Date.HasValue ? Date.Value.ToString("yyyy-MM-dd") : "");
        }
    }
}

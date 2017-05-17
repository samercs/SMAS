using System;
using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using OrangeJetpack.Core.Web.UI;

namespace OrangeJetpack.Core.Web.TagHelpers
{
    [HtmlTargetElement("status-message")]
    public class StatusMessageTagHelper : TagHelper
    {
        public string Message { get; set; }
        public string Type { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrWhiteSpace(Message))
            {
                output.SuppressOutput();
                return;
            }

            var type = ToEnum(Type, StatusMessageType.Success);

            output.Attributes.SetAttribute("class", GetStatusMessageClass(type));
            output.Content.AppendHtml(GetStatusMessageIcon(type));
            output.Content.AppendHtml("<span>" + Message + "</span>");
            output.Content.AppendHtml("<div class='clearfix'></div>");
        }

        private static StatusMessageType ToEnum(string value, StatusMessageType defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            StatusMessageType result;
            return Enum.TryParse(value, true, out result) ? result : defaultValue;
        }

        private static string GetStatusMessageClass(StatusMessageType statusMessageType)
        {
            switch (statusMessageType)
            {
                case StatusMessageType.Information:
                    return "alert alert-info";
                case StatusMessageType.Success:
                    return "alert alert-success";
                case StatusMessageType.Warning:
                    return "alert alert-warning";
                case StatusMessageType.Error:
                    return "alert alert-danger";
                default:
                    throw new NotImplementedException("Invalid status message type.");
            }
        }

        private static string GetStatusMessageIcon(StatusMessageType statusMessageType)
        {
            using (var writer = new StringWriter())
            {
                var icon = new TagBuilder("i");
                icon.AddCssClass("fa fa-3x pull-left");
                switch (statusMessageType)
                {
                    case StatusMessageType.Success:
                        icon.AddCssClass("fa-check-square-o");
                        break;
                    case StatusMessageType.Information:
                        icon.AddCssClass("fa-question-circle");
                        break;
                    case StatusMessageType.Warning:
                        icon.AddCssClass("fa-warning");
                        break;
                    case StatusMessageType.Error:
                        icon.AddCssClass("fa-exclamation-triangle");
                        break;
                }

                icon.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}

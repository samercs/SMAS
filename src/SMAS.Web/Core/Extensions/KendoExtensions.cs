using System;
using Kendo.Mvc.UI.Fluent;

namespace SMAS.Web.Core.Extensions
{
    public static class KendoExtensions
    {
        public static GridBuilder<T> Init<T>(this GridBuilder<T> gridBuilder,
            Action<CrudOperationBuilder> ajaxDataSource) where T : class
        {
            const int pageSize = 50;
            var gridName = $"Grid_{Guid.NewGuid()}";

            return gridBuilder
                .Name(gridName)
                .Pageable()
                .Sortable()
                .Filterable()
                .DataSource(dataSource => dataSource
                    .Ajax()
                    .Read(ajaxDataSource)
                    .PageSize(pageSize)
                    .ServerOperation(false))
               .HtmlAttributes(new { @class = "table table-responsive" });
        }

        public static GridTemplateColumnBuilder<T> LinkColumn<T>(this GridColumnFactory<T> column, string text, string href) where T : class
        {
            return column
                .Template($"<a href=\"{href}\">{text}</a>")
                .HtmlAttributes(new { @class = "link-cell " });
        }

        public static GridTemplateColumnBuilder<T> LinkColumn<T>(this GridColumnFactory<T> column, string text, string href, object parameter) where T : class
        {
            href = href.Replace("-99", parameter.ToString());
            return column
                .Template($"<a href=\"{href}\">{text}</a>")
                .HtmlAttributes(new { @class = "link-cell " });
        }

        public static GridBoundColumnBuilder<T> FitCell<T>(this GridBoundColumnBuilder<T> column) where T : class
        {
            return column.HtmlAttributes(new { @class = "fit-cell" });
        }

        public static GridBoundColumnBuilder<T> ImageCell<T>(this GridBoundColumnBuilder<T> column) where T : class
        {
            return column.HtmlAttributes(new { @class = "image-cell" });
        }

        public static GridBoundColumnBuilder<T> FormatDate<T>(this GridBoundColumnBuilder<T> builder) where T : class
        {
            return builder.Format("{0:yyyy-MM-dd}");
        }

        public static GridBoundColumnBuilder<T> FormatDateTime<T>(this GridBoundColumnBuilder<T> builder) where T : class
        {
            return builder.Format("{0:yyyy-MM-dd hh:mm tt}");
        }
    }
}

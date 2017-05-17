using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SMAS.Web.Core.Configuration
{
    public class PageSettings
    {
        private readonly ViewDataDictionary _viewData;

        public PageSettings(ViewDataDictionary viewData)
        {
            _viewData = viewData;
        }

        public string Title
        {
            get { return _viewData["PageSettings.Title"].ToString(); }
            set { _viewData["PageSettings.Title"] = value; }
        }
    }

    public abstract class AppBasePage<TModel> : RazorPage<TModel>
    {
        private PageSettings _pageSettings;
        public PageSettings PageSettings => _pageSettings ?? (_pageSettings = new PageSettings(ViewData));
    }
}

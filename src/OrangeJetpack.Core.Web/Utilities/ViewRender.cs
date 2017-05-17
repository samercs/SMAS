using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace OrangeJetpack.Core.Web.Utilities
{
    public class ViewRender
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public ActionContext ActionContext { get; set; }

        public ViewRender(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            
        }

        public string GetPartialViewAsString<TModel>(string name, TModel model)
        {
            var viewEngineResult = _viewEngine.FindView(ActionContext, name, false);

            if (!viewEngineResult.Success)
            {
                var searchLocation = string.Join(",", viewEngineResult.SearchedLocations);
                throw new InvalidOperationException($"Couldn't find view {name} search locations : {searchLocation}");
            }

            var view = viewEngineResult.View;

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    ActionContext,
                    view,
                    new ViewDataDictionary<TModel>(
                        new EmptyModelMetadataProvider(),
                        new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(
                        ActionContext.HttpContext,
                        _tempDataProvider),
                    output,
                    new HtmlHelperOptions());

                view.RenderAsync(viewContext).GetAwaiter().GetResult();

                return output.ToString();
            }
        }

    }
}

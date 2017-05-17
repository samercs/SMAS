using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SMAS.Web.Features.Shared;

namespace SMAS.Web.Core.Configuration
{
    public class FromBodyConvention: IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            
            var type = action.Controller.ControllerType;
            if (typeof(ApiBaseController).IsAssignableFrom(type.BaseType))
            {
                foreach (var parameter in action.Parameters)
                {
                    if (parameter.BindingInfo == null)
                    {
                        parameter.BindingInfo = parameter.BindingInfo ?? new BindingInfo();
                        parameter.BindingInfo.BindingSource = BindingSource.Body;
                    }
                }
            }

            
        }
    }
}

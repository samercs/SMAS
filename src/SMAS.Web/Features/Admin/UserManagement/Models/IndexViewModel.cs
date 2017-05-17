using System;
using Humanizer;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Identity = SMAS.Core.Identity;

namespace SMAS.Web.Features.Admin.UserManagement.Models
{
    public class IndexViewModel
    {
        public string Search { get; set; }
        public string Role { get; set; }
        public DateTime DateTime { get; set; }

        public IEnumerable<SelectListItem> RoleList
        {
            get
            {
                var roles = new[]
                {
                    Identity.Role.Administrator
                };

                return roles.Select(i => new SelectListItem
                {
                    Value = i,
                    Text = i.Humanize(),
                    Selected = i.Equals(Role)
                });
            }
        }
    }
}

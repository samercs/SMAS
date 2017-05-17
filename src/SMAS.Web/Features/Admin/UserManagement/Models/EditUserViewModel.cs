using Microsoft.AspNetCore.Mvc.Rendering;
using SMAS.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMAS.Services.Identity;

namespace SMAS.Web.Features.Admin.Users.Models
{
    public class EditUserViewModel
    {
        public Entities.User User { get; set; }      
        public IEnumerable<string> UserRoles { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }

        public EditUserViewModel()
        {
            
        }

        public EditUserViewModel(Entities.User user)
        {
            User = user;
        }

        public async Task<EditUserViewModel> Hydrate(UserService userService)
        {
            var roles = await userService.GetRoles();

            UserRoles = await userService.GetRolesForUser(User);

            RoleList = roles
                .OrderBy(i => i.Name)
                .Select(i => new SelectListItem
                {
                    Value = i.Name,
                    Text = i.Name,
                    Selected = UserRoles.Contains(i.Name)
                });

            return this;
        }
    }
}

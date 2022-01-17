using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Infraestructure.UsersContext.Data.Entities
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User
        {
            get; set;
        }
        public virtual ApplicationRole Role
        {
            get; set;
        }
    }

    public class ApplicationRole : IdentityRole<string>
    {
        public virtual ICollection<ApplicationUserRole> UserRoles
        {
            get; set;
        }
    }
}

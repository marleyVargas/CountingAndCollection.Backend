using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Infraestructure.UsersContext.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [JsonIgnore]
        public virtual ICollection<ApplicationUserRole> UserRoles
        {
            get; set;
        }
    }
}

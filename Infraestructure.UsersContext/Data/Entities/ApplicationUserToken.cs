using Microsoft.AspNetCore.Identity;
using System;

namespace Infraestructure.UsersContext.Data.Entities
{
    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public DateTime ExpirationDate
        {
            get; set;
        }
        public bool IsDisabled
        {
            get; set;
        }
    }
}

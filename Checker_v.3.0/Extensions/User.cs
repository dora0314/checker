using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.Models
{
    public partial class User
    {
        public static User Get(DataContext dataContext, HttpContext httpContext)
        {
            var email = httpContext.User.Identity.Name;
            var user = dataContext.Users.FirstOrDefault(x => x.Email == email);

            return user;
        }
    }
}

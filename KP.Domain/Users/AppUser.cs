using System;
using KP.Domain.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KP.Domain.Users
{
    public class AppUser : IdentityUser {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public DateTime LastLogin { get; set; }

        public int FailedPasswordAttemptCount { get; set; }
    }
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace REST2.Entities
{
    public class ESPLUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
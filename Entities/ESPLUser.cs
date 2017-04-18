using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ESPL.KP.Entities
{
    public class ESPLUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
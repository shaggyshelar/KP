using System.ComponentModel.DataAnnotations;

namespace ESPL.KP.Models.Auth
{
    public class CredentialModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
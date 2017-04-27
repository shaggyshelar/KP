using System.ComponentModel.DataAnnotations;

namespace ESPL.KP.Models.Core
{
    public class AppModuleForCreationDto
    {
        [Required]
        [MaxLength(20), MinLength(4)]
        public string MenuText { get; set; }

        [Required]
        [MaxLength(50), MinLength(4)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2), MinLength(2)]
        public string ShortName { get; set; }
    }
}
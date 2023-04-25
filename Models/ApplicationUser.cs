using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace Human_Resource_Generator.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string MSNV { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
    }
}

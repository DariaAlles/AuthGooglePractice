using Microsoft.AspNetCore.Identity;

namespace AuthGoogle.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            
        }
        public ApplicationUser(string userName) : base (userName)
        {
            
        }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}

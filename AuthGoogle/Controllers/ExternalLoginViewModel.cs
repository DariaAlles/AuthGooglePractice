using System.ComponentModel.DataAnnotations;

namespace AuthGoogle.Controllers
{
    public partial class AdminController
    {
        public class ExternalLoginViewModel
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            public string ReturnUrl { get; set; }

        }
    }

    
}

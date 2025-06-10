using Microsoft.Extensions.Options;

namespace Identity.Configuration
{
    public class AuthorizationOptions
    {
        public string ConnectionString { get; set; }
        public string AdministratorEmail { get; set; }
    }
}

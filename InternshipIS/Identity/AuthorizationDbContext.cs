using Identity.Database.Entities;
using Identity.Database.EntityConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity
{
    /// <summary>
    /// The db context for authentications / authorizations using asp net core identity
    /// </summary>
    public class AuthorizationDbContext : IdentityDbContext<User, Role, Guid>
    {
        private readonly IRoleEntityConfiguration _roleConfiguration;
        private readonly IUserEntityConfiguration _userConfiguration;

        /// <summary>
        /// default .ctor for AuthorizationDbContext
        /// </summary>
        public AuthorizationDbContext()
        {

        }

        /// <summary>
        /// .ctor for AuthorizationDbContext
        /// </summary>
        /// <param name="roleConfiguration"></param>
        /// <param name="userEntityConfiguration"></param>
        /// <param name="options"></param>
        public AuthorizationDbContext(
            IRoleEntityConfiguration roleConfiguration,
            IUserEntityConfiguration userEntityConfiguration,
            DbContextOptions options) : base(options)
        {
            _roleConfiguration = roleConfiguration;
            _userConfiguration = userEntityConfiguration;
        }

        ///<inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _roleConfiguration.Configure(modelBuilder.Entity<Role>());
            _userConfiguration.Configure(modelBuilder.Entity<User>());

            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("auth");
        }
    }
}
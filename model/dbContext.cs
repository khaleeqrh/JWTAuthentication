using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Model
{
    public class dbContext : IdentityDbContext<User>
    {
        public dbContext(DbContextOptions<dbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        public override DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void HandleDependent(EntityEntry entry)
        {
            entry.State = EntityState.Modified;
            entry.CurrentValues["IsDeleted"] = true;
        }
    }
}
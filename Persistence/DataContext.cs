using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    /// <summary>
    /// Represents the database context for the application, inheriting from IdentityDbContext to include identity-related tables.
    /// </summary>
    public class DataContext : IdentityDbContext<AppUser>
    {
        /// <summary>
        /// Initializes a new instance of the DataContext class with the specified options.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the Activities table in the database.
        /// </summary>
        public DbSet<Activity> Activities { get; set; }

        /// <summary>
        /// Gets or sets the ActivityAttendees table in the database.
        /// </summary>
        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }

        /// <summary>
        /// Gets or sets the Photos table in the database.
        /// </summary>
        public DbSet<Photo> Photos { get; set; }

        /// <summary>
        /// Gets or sets the Comments table in the database.
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// Gets or sets the UserFollowings table in the database.
        /// </summary>
        public DbSet<UserFollowing> UserFollowings { get; set; }
        // public IEnumerable<object> ActivityAttendees { get; set; }


        /// <summary>
        /// Configures the entity relationships and constraints for the database.
        /// </summary>
        /// <param name="builder">The model builder used to configure the database schema.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure composite key for ActivityAttendee
            builder.Entity<ActivityAttendee>(x => x.HasKey(aa => new { aa.AppUserId, aa.ActivityId }));

            // Configure relationships for ActivityAttendee
            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.AppUser)
                .WithMany(a => a.Activities)
                .HasForeignKey(aa => aa.AppUserId);

            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.Activity)
                .WithMany(a => a.Attendees)
                .HasForeignKey(aa => aa.ActivityId);

            // Configure relationship for Comment
            builder.Entity<Comment>()
                .HasOne(a => a.Activity)
                .WithMany(c => c.Comments)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure composite key and relationships for UserFollowing
            builder.Entity<UserFollowing>(b =>
            {
                b.HasKey(k => new { k.ObserverId, k.TargetId });

                b.HasOne(o => o.Observer)
                    .WithMany(f => f.Followings)
                    .HasForeignKey(o => o.ObserverId)
                    .OnDelete(DeleteBehavior.NoAction);

                b.HasOne(o => o.Target)
                    .WithMany(f => f.Followers)
                    .HasForeignKey(o => o.TargetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }

}

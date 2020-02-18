using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodApp.Data.Models;
using GoodApp.Data.Views;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GoodApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {

        }

        public virtual DbSet<Application> Applications { get; set; }

        public virtual DbSet<Challenge> Challenges { get; set; }

        public virtual DbSet<Deed> Deeds { get; set; }

        public virtual DbSet<Group> Groups { get; set; }

        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }

        public virtual DbSet<ErrorMessage> ErrorMessages { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().HasMany(e => e.CreatedChallenges)
                .WithRequired(e => e.Creator)
                .HasForeignKey(e => e.CreatorId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<ApplicationUser>().HasMany(e => e.CreatedGroups)
                .WithRequired(e => e.Creator)
                .HasForeignKey(e => e.CreatorId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<ApplicationUser>().HasMany(e => e.MyDeeds)
                .WithRequired(e => e.Creator)
                .HasForeignKey(e => e.CreatorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Challenge>().HasMany(e => e.Groups)
                  .WithMany(e => e.Challenges)
                  .Map(m =>
                  {
                      m.MapLeftKey("ChallengeId");
                      m.MapRightKey("GroupId");
                      m.ToTable("ChallengeGroups");
                  });

            modelBuilder.Entity<Deed>()
                .HasRequired<Challenge>(p => p.Challenge)
                .WithMany(p => p.Deeds)
                .HasForeignKey(p => p.ChallengeId);

            modelBuilder.Entity<Group>().HasMany(e => e.Members)
                .WithMany(e => e.JoinedGroups)
                .Map(m =>
                {
                    m.MapLeftKey("GroupId");
                    m.MapRightKey("UserId");
                    m.ToTable("GroupMembers");
                });

            modelBuilder.Entity<Deed>().HasMany(e => e.TaggedUsers)
                .WithMany(e => e.TagDeeds)
                .Map(m =>
                {
                    m.MapLeftKey("DeedId");
                    m.MapRightKey("UserId");
                    m.ToTable("TagUsers");
                });

            modelBuilder.Entity<Comment>().HasRequired(e => e.User).WithMany(p => p.Comments).HasForeignKey(p => p.UserId);

        }
    }
}

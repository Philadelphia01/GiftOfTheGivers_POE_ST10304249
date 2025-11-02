using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<DisasterReport> DisasterReports => Set<DisasterReport>();
        public DbSet<Donation> Donations => Set<Donation>();
        public DbSet<Volunteer> Volunteers => Set<Volunteer>();
        public DbSet<VolunteerTask> VolunteerTasks => Set<VolunteerTask>();
        public DbSet<VolunteerCommunication> VolunteerCommunications => Set<VolunteerCommunication>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Donation entity
            builder.Entity<Donation>(entity =>
            {
                entity.HasOne(d => d.DonorUser)
                      .WithMany()
                      .HasForeignKey(d => d.DonorUserId)
                      .OnDelete(DeleteBehavior.NoAction)
                      .IsRequired(false);

                entity.HasOne(d => d.DistributedByUser)
                      .WithMany()
                      .HasForeignKey(d => d.DistributedByUserId)
                      .OnDelete(DeleteBehavior.NoAction)
                      .IsRequired(false);
            });

            // Configure VolunteerTask entity
            builder.Entity<VolunteerTask>(entity =>
            {
                entity.HasOne(vt => vt.AssignedVolunteer)
                      .WithMany()
                      .HasForeignKey(vt => vt.AssignedVolunteerId)
                      .OnDelete(DeleteBehavior.NoAction)
                      .IsRequired(false);

                entity.HasOne(vt => vt.CreatedByUser)
                      .WithMany()
                      .HasForeignKey(vt => vt.CreatedByUserId)
                      .OnDelete(DeleteBehavior.NoAction)
                      .IsRequired(false);
            });

            // Configure VolunteerCommunication entity
            builder.Entity<VolunteerCommunication>(entity =>
            {
                entity.HasOne(vc => vc.SenderUser)
                      .WithMany()
                      .HasForeignKey(vc => vc.SenderUserId)
                      .OnDelete(DeleteBehavior.NoAction)
                      .IsRequired(true);

                entity.HasOne(vc => vc.RecipientUser)
                      .WithMany()
                      .HasForeignKey(vc => vc.RecipientUserId)
                      .OnDelete(DeleteBehavior.NoAction)
                      .IsRequired(false);

                entity.HasOne(vc => vc.RelatedTask)
                      .WithMany()
                      .HasForeignKey(vc => vc.RelatedTaskId)
                      .OnDelete(DeleteBehavior.NoAction)
                      .IsRequired(false);
            });

            // Configure Volunteer entity
            builder.Entity<Volunteer>(entity =>
            {
                entity.HasOne(v => v.VolunteerUser)
                      .WithMany()
                      .HasForeignKey(v => v.VolunteerUserId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .IsRequired(false);
            });

            // Configure VolunteerTask entity
            builder.Entity<VolunteerTask>(entity =>
            {
                entity.HasOne<ApplicationUser>()
                      .WithMany()
                      .HasForeignKey(vt => vt.AssignedVolunteerId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .IsRequired(false);
            });
        }
    }
}

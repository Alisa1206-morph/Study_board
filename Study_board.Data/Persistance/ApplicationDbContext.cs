using Microsoft.EntityFrameworkCore;
using Study_board.Models.Domain.Entities;


namespace Study_board.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }


        public DbSet<Checklist> Checklists { get; set; }
        public DbSet<ChecklistImage> ChecklistImages { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        // Checklist -> Image relationship
        modelBuilder.Entity<Checklist>()
            .HasOne(c => c.Image)
            .WithOne(i => i.Checklist)
            .HasForeignKey<ChecklistImage>(i => i.ChecklistId)
            .OnDelete(DeleteBehavior.Cascade);

        // Project configuration
        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(p => p.Description)
                .HasMaxLength(500);

            entity.Property(p => p.Type)
                .IsRequired();
        });
    }
}
}
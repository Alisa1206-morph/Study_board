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

        // Checklist -> Image
        modelBuilder.Entity<Checklist>()
            .HasOne(r => r.Images)
            .WithOne(c => c.Checklist)
            .HasForeignKey(i => i.ChecklistId)
            .OnDelete(DeleteBehavior.Cascade);

        // Checklist -> Projects
        modelBuilder.Entity<Checklist>()
            .HasMany(r => r.Projects)
            .WithOne(c => c.Checklist)
            .HasForeignKey(m => m.ChecklistId)
            .OnDelete(DeleteBehavior.Cascade);

        // Project configuration
        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(p => p.Description)
                .HasMaxLength(500);

            entity.Property(p => p.DueDate);

            entity.Property(p => p.IsCompleted);

            entity.Property(p => p.Type)
                .IsRequired();

            entity.HasIndex(p => new { p.ChecklistId, p.Title });
            entity.Property(p => p.ActiveFrom)
                .IsRequired();

            // ActiveTo is optional by design
        });
    }
}
}
using System.Linq.Expressions;
using BloggingPLatformApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BloggingPLatformApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Post> Posts { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Tag> Tags { get; set; } = default!;
    public DbSet<PostTag> PostTags { get; set; } = default!;
    public DbSet<Comment> Comments { get; set; } = default!;
    public DbSet<Like> Likes { get; set; } = default!;
    public DbSet<MediaAttachment> MediaAttachments { get; set; } = default!;
    public DbSet<Notification> Notifications { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            
            entity.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);
            entity.HasIndex(u => u.Username)
                .IsUnique();

            entity.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.HasIndex(c => c.Name)
                .IsUnique();
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(p => p.Content)
                .IsRequired()
                .HasColumnType("TEXT");

            entity.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(p => p.CreatedAt)
                .HasDefaultValueSql("NOW()")
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            entity.Property(p => p.PublishedAt)
                .IsRequired(false);

            entity.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            entity.HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.HasIndex(t => t.Name)
                .IsUnique();
        });

        modelBuilder.Entity<PostTag>(entity =>
        {
            entity.HasKey(pt => new { pt.PostId, pt.TagId });

            entity.HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);

            entity.HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Content)
                .IsRequired()
                .HasColumnType("TEXT");

            entity.Property(c => c.CreatedAt)
                .HasDefaultValueSql("NOW()");

            entity.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            entity.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(l => l.Id);

            entity.HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);
        });

        modelBuilder.Entity<MediaAttachment>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.FileUrl)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(m => m.FileType)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(m => m.Post)
                .WithMany(p => p.MediaAttachments)
                .HasForeignKey(m => m.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(n => n.Id);

            entity.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(n => n.IsRead)
                .HasDefaultValue(false);

            entity.Property(n => n.CreatedAt)
                .HasDefaultValueSql("NOW()");

            entity.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}
using BloggingPLatformApi.Entities;
using BloggingPLatformApi.Services;
using Microsoft.EntityFrameworkCore;

namespace BloggingPLatformApi.Data;

public class SeedService(AppDbContext dbContext, IConfiguration configuration)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        // Seeding yoqilganligini konfiguratsiyadan tekshiramiz
        bool isSeedingEnabled = configuration.GetValue<bool>("Seeding:IsEnabled");
        if (!isSeedingEnabled)
            return;

        // 1. Users: Agar Users jadvali bo'sh bo'lsa, seeding qilamiz
        if (!dbContext.Users.Any())
        {
            // Admin foydalanuvchi
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                PasswordHash = "admin".HashPassword(), // Extension metod orqali hashing qilinadi
                Role = "Admin",
            };

            // Moderator foydalanuvchi
            var moderatorUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "moderator",
                PasswordHash = "moderator".HashPassword(),
                Role = "Moderator",
            };

            // 10 ta Author foydalanuvchilar
            var authors = new List<User>();
            for (int i = 1; i <= 10; i++)
            {
                authors.Add(new User
                {
                    Id = Guid.NewGuid(),
                    Username = $"author{i}",
                    PasswordHash = $"author{i}".HashPassword(),
                    Role = "Author",
                });
            }

            dbContext.Users.Add(adminUser);
            dbContext.Users.Add(moderatorUser);
            dbContext.Users.AddRange(authors);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        // 2. Categories: Agar Categories jadvali bo'sh bo'lsa
        if (!dbContext.Categories.Any())
        {
            var categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Texnologiya" },
                new Category { Id = Guid.NewGuid(), Name = "Madaniyat" },
                new Category { Id = Guid.NewGuid(), Name = "Sport" },
                new Category { Id = Guid.NewGuid(), Name = "Sog'liq" }
            };
            dbContext.Categories.AddRange(categories);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        // 3. Posts: Agar Posts jadvali bo'sh bo'lsa
        if (!dbContext.Posts.Any())
        {
            // Foydalanuvchilar va birinchi kategoriyani olish
            var userIds = dbContext.Users.Select(u => u.Id).ToList();
            var firstCategory = await dbContext.Categories.FirstOrDefaultAsync(cancellationToken: cancellationToken);
            
            var posts = new List<Post>();
            var random = new Random();

            // 15 ta post yarataylik
            for (int i = 1; i <= 15; i++)
            {
                posts.Add(new Post
                {
                    Id = Guid.NewGuid(),
                    Title = $"Post {i}: O'zbekcha Sarlavha",
                    Content = $"Bu {i}-chi postning mazmuni. Mazkur post O'zbek tilida yozilgan va foydalanuvchi uchun qiziqarli ma'lumotlarni o'z ichiga oladi.",
                    Status = i % 2 == 0 ? "Published" : "Draft",
                    CreatedAt = DateTime.UtcNow,
                    UserId = userIds[random.Next(userIds.Count)],
                    CategoryId = firstCategory?.Id ?? Guid.NewGuid()
                });
            }
            dbContext.Posts.AddRange(posts);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        if (!dbContext.Likes.Any())
        {
            var postIds = dbContext.Posts.Select(p => p.Id).ToList();
            var userIds = dbContext.Users.Select(u => u.Id).ToList();
            var likes = new List<Like>();
            var random = new Random();
            int numberOfLikes = 15;
            for (int i = 0; i < numberOfLikes; i++)
            {
                likes.Add(new Like
                {
                    Id = Guid.NewGuid(),
                    PostId = postIds[random.Next(postIds.Count)],
                    UserId = userIds[random.Next(userIds.Count)]
                });
            }
            dbContext.Likes.AddRange(likes);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        // 5. MediaAttachments: Agar MediaAttachments jadvali bo'sh bo'lsa
        if (!dbContext.MediaAttachments.Any())
        {
            var posts = await dbContext.Posts.ToListAsync(cancellationToken);
            var mediaAttachments = new List<MediaAttachment>();
            int counter = 1;
            foreach (var post in posts)
            {
                // Har bir post uchun bitta media attachment qo'shamiz
                mediaAttachments.Add(new MediaAttachment
                {
                    Id = Guid.NewGuid(),
                    FileUrl = $"/uploads/sample{counter}.jpg",
                    FileType = "image",
                    PostId = post.Id
                });
                counter++;
            }
            dbContext.MediaAttachments.AddRange(mediaAttachments);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        // 6. Notifications: Agar Notifications jadvali bo'sh bo'lsa, har bir foydalanuvchi uchun 2 ta bildirishnoma yarataylik
        if (!dbContext.Notifications.Any())
        {
            var userIds = dbContext.Users.Select(u => u.Id).ToList();
            var notifications = new List<Notification>();
            foreach (var userId in userIds)
            {
                notifications.Add(new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Message = "Yangi bildirishnoma: Profilingiz yangilandi.",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                });
                notifications.Add(new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Message = "Yangi bildirishnoma: Yangi post yaratilgan.",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                });
            }
            dbContext.Notifications.AddRange(notifications);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

    }
}
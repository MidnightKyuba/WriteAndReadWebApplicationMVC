using Microsoft.EntityFrameworkCore;
using WriteAndReadWebApplicationMVC.Models;

namespace WriteAndReadWebApplicationMVC
{
    public class DbWriteAndReadContext : DbContext
    {
        public DbWriteAndReadContext(DbContextOptions<DbWriteAndReadContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Readed> Readeds { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Block> Blocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(b => b.mybooks).WithOne(u => u.author).HasForeignKey(f => f.authorId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany(c => c.comments).WithOne(u => u.user).HasForeignKey(f => f.userId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany(b => b.blocks).WithOne(u => u.user).HasForeignKey(f => f.userId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany(r => r.readed).WithOne(u => u.user).HasForeignKey(f => f.userId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().Property(u => u.id).UseIdentityColumn();
            modelBuilder.Entity<Book>().HasMany(ch => ch.chapters).WithOne(b => b.book).HasForeignKey(f => f.bookId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Chapter>().HasMany(c => c.comments).WithOne(ch => ch.chapter).HasForeignKey(f => f.chapterId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Chapter>().HasMany(r => r.readers).WithOne(ch => ch.chapter).HasForeignKey(f => f.chapterId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Comment>();
            modelBuilder.Entity<Readed>();
            modelBuilder.Entity<Block>();
            base.OnModelCreating(modelBuilder);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using MinAPIMusicProject.Models;

namespace MinAPIMusicProject.Data;

public class MusicContext : DbContext
{
    public MusicContext(DbContextOptions<MusicContext> options) : base(options)
    {
    }

    public DbSet<Track> Tracks { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*modelBuilder.Entity<Track>()
            .Property(x => x.Title)
            .IsRequired(false);*/

        modelBuilder.Entity<User>()
            .HasMany(u => u.LikedTracks)
            .WithMany(t => t.LikedByUsers)
            .UsingEntity(j => j.ToTable("UserTrackLikes"));
    }
}
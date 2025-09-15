using Microsoft.EntityFrameworkCore;
using MyUrlShortner;

namespace MyUrlShortner
{
public class UrlShortenerDbContext : DbContext
{
    public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : base(options) { }

    public DbSet<UrlShort> UrlShort { get; set; }
}
}
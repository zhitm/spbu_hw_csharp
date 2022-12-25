using Microsoft.EntityFrameworkCore;

namespace Journal;

public class DbClassJournalContext : DbContext
{
    public DbClassJournalContext(
        DbContextOptions<DbClassJournalContext> options)
        : base(options)
    {
    }
    public DbSet<Data> Participants => Set<Data>();
}
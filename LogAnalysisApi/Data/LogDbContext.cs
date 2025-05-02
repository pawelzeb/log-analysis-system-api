using Microsoft.EntityFrameworkCore;
using LogAnalysisApi.Models;

namespace LogAnalysisApi.Data;

public class LogDbContext : DbContext
{
    public LogDbContext(DbContextOptions<LogDbContext> options) : base(options) {}

    public DbSet<LogEntry> Logs => Set<LogEntry>();
}

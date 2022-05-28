using Microsoft.EntityFrameworkCore;
using Models;

namespace webapi.Data;

public class FileContext : DbContext
{
    public FileContext(DbContextOptions<FileContext> options)
        : base(options)
    { }

    public DbSet<CustomFile> CustomFiles { get; set; }
}
using Microsoft.EntityFrameworkCore;
using DataCalculatorWebAppServer.Models;

namespace DataCalculatorWebAppServer.Data
{
    public class DataCalculatorDbContext : DbContext
    {
        public DataCalculatorDbContext(DbContextOptions<DataCalculatorDbContext> options)
            : base(options)
        {

        }

        public DbSet<FileSendData> FileSendData { get; set; }
        public DbSet<FileStorageData> FileStorageData { get; set; }
    }
}

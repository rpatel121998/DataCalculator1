using Microsoft.EntityFrameworkCore;
using DataCalculatorWebAppServer.Models;

// The local database is not necessary for now because we could now upload to AWS
namespace DataCalculatorWebAppServer.Data 
{
    public class DataCalculatorDbContext : DbContext
    {
        public DataCalculatorDbContext(DbContextOptions<DataCalculatorDbContext> options)
            : base(options)
        {

        }

        public DbSet<FileSendData> FileSendData { get; set; } 
    }
}

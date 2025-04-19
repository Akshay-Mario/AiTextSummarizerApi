
using AiTextSummarizeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AiTextSummarizeApi.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {

        }

        public DbSet<User> Users { get; set; }
    }
}
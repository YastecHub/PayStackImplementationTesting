using Microsoft.EntityFrameworkCore;
using PayStackImplementationTesting.Models.Entities;

namespace PayStackImplementationTesting.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<PaymentRequest> PaymentRequests { get; set; }
    }
}

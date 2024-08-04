using Microsoft.EntityFrameworkCore;
using KalahGameApi.Models;

namespace KalahGameApi.Data
{
    public class KalahGameContext : DbContext
    {
        public KalahGameContext(DbContextOptions<KalahGameContext> options) : base(options) { }

   
    }
}

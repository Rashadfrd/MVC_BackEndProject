using Microsoft.EntityFrameworkCore;

namespace Riode.DAL
{
    public class RiodeContext:DbContext
    {
        public RiodeContext(DbContextOptions<RiodeContext>options):base(options)
        {
        }

    }
}

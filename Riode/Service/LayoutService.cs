using Riode.DAL;

namespace Riode.Service
{
    public class LayoutService
    {
        RiodeContext _context { get; }
        public LayoutService(RiodeContext context)
        {
            _context = context;
        }


        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.ToDictionary(p => p.Key, p => p.Value);
        }

    }
}

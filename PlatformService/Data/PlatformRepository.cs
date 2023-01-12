using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly ApplicationDbContext _context;

        public PlatformRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreatePlatform(Platform request)
        {
            if (request == null)
            {
                throw new ArgumentNullException();
            }

            _context.Platforms.Add(request);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
            return _context.Platforms.FirstOrDefault(x => x.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}

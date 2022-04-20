using Oms.Context;
using Oms.Models;

namespace Oms.Repository
{
    public class RequestRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Request? GetLastRequestById(int requestId) {
            return _context.Set<Request>()
                .Where(r => r.Id == requestId || r.OriginCode == requestId)
                .OrderByDescending(r => r.Id)
                .FirstOrDefault();
        }
    }
}

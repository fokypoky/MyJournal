using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class AuditoriesRepository : EntityRepository<Auditory>
{
    public AuditoriesRepository(DbContext context) : base(context)
    {
    }

    public bool IsAuditoryNumberExists(string auditoryNumber)
    {
        return _context.Set<Auditory>()
	        .FirstOrDefault(a => a.AuditoryNumber == auditoryNumber) is not null;
    }

    public ICollection<Auditory> GetAll()
    {
        return _context.Set<Auditory>()
	        .Include(a => a.Classes)
	        .ToList();
    }

}
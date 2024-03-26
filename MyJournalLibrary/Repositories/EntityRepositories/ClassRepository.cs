using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class ClassRepository : EntityRepository<Class>
{
    public ClassRepository(DbContext context) : base(context)
    {
    }

    public ICollection<Class> GetRangeByAuditory(Auditory auditory)
    {
        return _context.Set<Class>()
	        .Where(c => c.AuditoryId == auditory.Id)
	        .ToList();
    }

    public void UpdateRange(List<Class> classes)
    {
        _context.Set<Class>().UpdateRange(classes);
        _context.SaveChanges();
    }

    public ICollection<Class> GetAll()
    {
        return _context.Set<Class>().ToList();
    }

    public Class? GetByAuditory(Auditory auditory)
    {
	    return _context.Set<Class>()
		    .FirstOrDefault(c => c.AuditoryId == auditory.Id);
    }

    public ICollection<Class> GetClassesWithoutLeader()
    {
        return _context.Set<Class>()
	        .Where(c => c.LeaderId == null)
	        .ToList();
    }

    public ICollection<Class> GetByEmployee(Employee employee)
    {
        return _context.Set<Class>()
            .Where(c => c.Leader.Id == employee.Id)
            .ToList();
    }

    public ICollection<Class> GetByEmployeeId(int employeeId)
    {
        return _context.Set<Class>()
            .Where(c => c.LeaderId == employeeId)
            .ToList();
    }
}
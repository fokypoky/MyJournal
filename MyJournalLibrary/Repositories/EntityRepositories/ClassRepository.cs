using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class ClassRepository : EntityRepository<Class>
{
    public ClassRepository(DbContext context) : base(context)
    {
    }

    public void UpdateNoTracked(Class existingClass, Class updatedClass)
    {
	    _context.Set<Class>().Entry(existingClass).CurrentValues.SetValues(updatedClass);
	    _context.SaveChanges();
	}

    public bool IsClassNumberExists(string classNumber)
    {
	    return _context.Set<Class>()
		    .FirstOrDefault(c => c.ClassNumber == classNumber) is not null;
    }

    public ICollection<Class> GetAllWithLeaderAndAuditory()
    {
        return _context.Set<Class>()
	        .Include(c => c.Leader)
				.ThenInclude(l => l.Contacts)
	        .Include(c => c.Auditory)
	        .ToList();
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
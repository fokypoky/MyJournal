using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class SubjectsRepository : EntityRepository<Subject>
{
    public SubjectsRepository(DbContext context) : base(context)
    {
    }

    public ICollection<Subject> GetWithClassesByEmployee(Employee employee)
    {
        return _context
            .Set<Subject>()
            .Include(s => s.Classes)
            .Where(s => s.Employees.Contains(employee)).ToList();
    }

    public ICollection<Subject> GetByClass(Class @class)
    {
        return _context.Set<Subject>()
            .Where(s => s.ClassSubjects.Any(cs => cs.ClassId == @class.Id))
            .ToList();
    }

    public ICollection<Subject> GetByStudent(Student student)
    {
        return _context.Set<Subject>()
	        .Where(s => s.ClassSubjects.Any(cs => cs.ClassId == student.ClassId))
	        .ToList();
    }
}
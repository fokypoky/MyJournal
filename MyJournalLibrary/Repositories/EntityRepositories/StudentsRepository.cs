using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class StudentsRepository : EntityRepository<Student>
{
    public StudentsRepository(DbContext context) : base(context)
    {
    }
    public ICollection<Student> GetByClass(Class @class)
    {
        return _context.Set<Student>()
            .Include(s => s.Contacts)
            .Include(s => s.Marks)
                .ThenInclude(m => m.Task)
            .Where(s => s.ClassId == @class.Id)
            .ToList();
    }

    public Student GetByIdWithContactsClassAndSubjects(int studentId)
    {
        return _context.Set<Student>()
            .Where(s => s.Id == studentId)
            .Include(s => s.Contacts)
            .Include(s => s.Class)
            .ThenInclude(m => m.Subjects)
            .FirstOrDefault();
    }

    public ICollection<Student>? GetWithContactsByParent(Parent parent)
    {
	    return _context.Set<Student>()
		    .Where(s => s.ParentStudents.Any(ps => ps.ParentId == parent.Id))?
		    .Include(s => s.Contacts)
		    .ToList();
    }
}
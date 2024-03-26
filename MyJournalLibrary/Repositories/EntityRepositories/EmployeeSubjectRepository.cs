using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories
{
	public class EmployeeSubjectRepository : EntityRepository<EmployeeSubject>
	{
		public EmployeeSubjectRepository(DbContext context) : base(context)
		{
		}

		public ICollection<EmployeeSubject> GetEmployeesWithContactsBySubject(Subject subject)
		{
			return _context.Set<EmployeeSubject>()
				.Where(es => es.SubjectId == subject.Id)
				.Include(es => es.Employee)
				.ThenInclude(e => e.Contacts)
				.ToList();
		}
	}
}
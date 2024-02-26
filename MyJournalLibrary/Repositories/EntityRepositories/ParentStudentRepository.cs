using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories
{
	public class ParentStudentRepository : EntityRepository<ParentStudent>
	{
		public ParentStudentRepository(DbContext context) : base(context)
		{
		}

		public ICollection<ParentStudent> GetByStudent(Student student)
		{
			return _context.Set<ParentStudent>()
				.Where(ps => ps.StudentId == student.Id)
				.ToList();
		}

		public void RemoveStudent(List<ParentStudent> parentStudents)
		{
			RemoveRange(parentStudents);
		}
	}
}

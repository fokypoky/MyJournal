using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories
{
	public class ParentStudentRepository : EntityRepository<ParentStudent>
	{
		public ParentStudentRepository(DbContext context) : base(context)
		{
		}

		public ParentStudent GetByParentAndStudentContacts(Contact parentContacts, Contact studentContacts)
		{
			return _context.Set<ParentStudent>()
				.FirstOrDefault(ps =>
					ps.Student.Contacts.Id == studentContacts.Id && ps.Parent.Contacts.Id == parentContacts.Id);
		}

		public ICollection<ParentStudent> GetByStudent(Student student)
		{
			return _context.Set<ParentStudent>()
				.Where(ps => ps.StudentId == student.Id)
				.ToList();
		}

		public ICollection<ParentStudent> GetByParent(Parent parent)
		{
			return _context.Set<ParentStudent>()
				.Where(ps => ps.ParentId == parent.Id)
				.ToList();
		}

		public ICollection<ParentStudent> GetWithContactsByStudent(Student student)
		{
			return _context.Set<ParentStudent>()
				.Where(ps => ps.StudentId == student.Id)
				.Include(ps => ps.Parent)
				.ThenInclude(p => p.Contacts)
				.ToList();
		}

		public ICollection<ParentStudent> GetWithStudentContactsByParentContacts(Contact parentContacts)
		{
			return _context.Set<ParentStudent>()
				.Where(ps => ps.Parent.ContactsId == parentContacts.Id)
				.Include(ps => ps.Student)
				.ThenInclude(s => s.Contacts)
				.ToList();
		} 
	}
}

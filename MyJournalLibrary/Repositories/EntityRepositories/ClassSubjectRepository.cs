using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories
{
	public class ClassSubjectRepository : EntityRepository<ClassSubject>
	{
		public ClassSubjectRepository(DbContext context) : base(context)
		{
		}

		public void RemoveAllBySubject(Subject subject)
		{
			_context.Set<ClassSubject>()
				.RemoveRange(_context.Set<ClassSubject>().Where(cs => cs.SubjectId == subject.Id));
			_context.SaveChanges();
		}
	}
}

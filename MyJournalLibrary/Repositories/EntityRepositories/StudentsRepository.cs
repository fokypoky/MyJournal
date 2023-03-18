using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class StudentsRepository : EntityRepository<Student>
{
    public StudentsRepository(DbContext context) : base(context)
    {
    }
}
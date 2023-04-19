using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalLibrary.Services;

public class ClassService : ClassRepository
{
    public ClassService(DbContext context) : base(context)
    {
    }
}
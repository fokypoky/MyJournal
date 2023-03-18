using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalLibrary.Services;

public class SubjectsService : SubjectsRepository
{
    public SubjectsService(DbContext context) : base(context)
    {
    }
}
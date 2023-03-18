using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalLibrary.Services;

public class ParentsService : ParentsRepository
{
    public ParentsService(DbContext context) : base(context)
    {
    }
}
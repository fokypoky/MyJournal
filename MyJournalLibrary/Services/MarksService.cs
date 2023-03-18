using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalLibrary.Services;

public class MarksService : MarksRepository
{
    public MarksService(DbContext context) : base(context)
    {
    }
}
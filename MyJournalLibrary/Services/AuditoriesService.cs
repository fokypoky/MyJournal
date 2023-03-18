using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Repositories.EntityRepositories;

namespace MyJournalLibrary.Services;

public class AuditoriesService : AuditoriesRepository
{
    public AuditoriesService(DbContext context) : base(context)
    {
    }
}
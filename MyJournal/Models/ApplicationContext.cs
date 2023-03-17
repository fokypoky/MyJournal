using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.EntitiesConfiguration;
using MyJournalLibrary.Entities;


namespace MyJournal.Models;

public class ApplicationContext : DbContext
{
    public DbSet<Auditory> Auditories { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeSubject> EmployeeSubjects { get; set; }
    public DbSet<Mark> Marks { get; set; }
    public DbSet<Parent> Parents { get; set; }
    public DbSet<ParentStudent> ParentStudents { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<Timetable> Timetable { get; set; }

    public static string ConnectionString { get; set; } =
        "Host=localhost;Port=5432;Username=postgres;Password=toor;Database=MyJournalDB";
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(ConnectionString);
        optionsBuilder.LogTo(m => Debug.WriteLine(m));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuditoryConfiguration());
        modelBuilder.ApplyConfiguration(new ClassConfiguration());
        modelBuilder.ApplyConfiguration(new ContactConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeSubjectConfiguration());
        modelBuilder.ApplyConfiguration(new MarkConfiguration());
        modelBuilder.ApplyConfiguration(new ParentConfiguration());
        modelBuilder.ApplyConfiguration(new ParentStudentConfiguration());
        modelBuilder.ApplyConfiguration(new StudentConfiguration());
        modelBuilder.ApplyConfiguration(new SubjectConfiguration());
        modelBuilder.ApplyConfiguration(new TaskConfiguration());
        modelBuilder.ApplyConfiguration(new TimetableConfiguration());
    }
}
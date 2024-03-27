using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;
using MyJournalLibrary.EntitiesConfiguration;
using System;
using System.Diagnostics;
using Task = MyJournalLibrary.Entities.Task;

namespace MyJournalAdmin.Infrastructure.Repositories
{
	public class ApplicationContext : DbContext
	{
		public virtual DbSet<Auditory> Auditories { get; set; }
		public virtual DbSet<Class> Classes { get; set; }
		public virtual DbSet<Contact> Contacts { get; set; }
		public virtual DbSet<Employee> Employees { get; set; }
		public virtual DbSet<Mark> Marks { get; set; }
		public virtual DbSet<Parent> Parents { get; set; }
		public virtual DbSet<Student> Students { get; set; }
		public virtual DbSet<Subject> Subjects { get; set; }
		public virtual DbSet<Task> Tasks { get; set; }
		public virtual DbSet<Timetable> Timetables { get; set; }
		public virtual DbSet<MyJournalLibrary.Entities.UserRole> UserRoles { get; set; }
		public virtual DbSet<ClassSubject> ClassSubjects { get; set; }
		public static string ConnectionString { get; set; } =
			"Host=localhost;Port=5432;Username=postgres;Password=toor;Database=MyJournalDB";

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(ConnectionString);
			AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
			optionsBuilder.LogTo(m => Debug.WriteLine(m));
			optionsBuilder.EnableSensitiveDataLogging();
			optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new AuditoryConfiguration());
			modelBuilder.ApplyConfiguration(new ClassConfiguration());
			modelBuilder.ApplyConfiguration(new ContactConfiguration());
			modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
			modelBuilder.ApplyConfiguration(new MarkConfiguration());
			modelBuilder.ApplyConfiguration(new ParentConfiguration());
			modelBuilder.ApplyConfiguration(new StudentConfiguration());
			modelBuilder.ApplyConfiguration(new SubjectConfiguration());
			modelBuilder.ApplyConfiguration(new TaskConfiguration());
			modelBuilder.ApplyConfiguration(new TimetableConfiguration());
			modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
		}
	}
}

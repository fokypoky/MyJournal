﻿using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class SubjectsRepository : EntityRepository<Subject>
{
    public SubjectsRepository(DbContext context) : base(context)
    {
    }

    public bool IsSubjectTitleExists(Subject subject)
    {
	    return _context.Set<Subject>()
		    .FirstOrDefault(s => s.SubjectTitle == subject.SubjectTitle) is not null;
    }
    
    public ICollection<Subject> GetAll()
    {
        return _context.Set<Subject>().ToList();
    }

    public ICollection<Subject> GetByEmployee(Employee employee)
    {
        return _context.Set<Subject>()
	        .Where(s => s.EmployeeSubjects.Any(es => es.EmployeeId == employee.Id))
	        .ToList();
    }

    public ICollection<Subject> GetWithClassesByEmployee(Employee employee)
    {
        return _context
            .Set<Subject>()
            .Include(s => s.Classes)
            .Where(s => s.Employees.Contains(employee)).ToList();
    }

    public ICollection<Subject> GetByClass(Class @class)
    {
        return _context.Set<Subject>()
            .Where(s => s.ClassSubjects.Any(cs => cs.ClassId == @class.Id))
            .ToList();
    }

    public ICollection<Subject> GetByStudent(Student student)
    {
        return _context.Set<Subject>()
	        .Where(s => s.ClassSubjects.Any(cs => cs.ClassId == student.ClassId))
	        .ToList();
    }
}
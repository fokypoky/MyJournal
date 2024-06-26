﻿using System.Collections;
using Microsoft.EntityFrameworkCore;
using MyJournalLibrary.Entities;

namespace MyJournalLibrary.Repositories.EntityRepositories;

public class StudentsRepository : EntityRepository<Student>
{
    public StudentsRepository(DbContext context) : base(context)
    {
    }

    public ICollection<Student> GetAllByClass(Class @class)
    {
        return _context.Set<Student>()
	        .Where(s => s.ClassId == @class.Id)
	        .ToList();
    }

    public ICollection<Student> GetByClass(Class @class)
    {
        return _context.Set<Student>()
            .Include(s => s.Contacts)
            .Include(s => s.Marks)
                .ThenInclude(m => m.Task)
            .Where(s => s.ClassId == @class.Id)
            .ToList();
    }

    public ICollection<Student> GetAllWithClassAndContacts()
    {
	    return _context.Set<Student>()
		    .Include(s => s.Contacts)
		    .Include(s => s.Class)
		    .ToList();
    }

    public Student GetByIdWithContactsClassAndSubjects(int studentId)
    {
        return _context.Set<Student>()
            .Where(s => s.Id == studentId)
            .Include(s => s.Contacts)
            .Include(s => s.Class)
            .ThenInclude(m => m.Subjects)
            .FirstOrDefault();
    }

    public ICollection<Student>? GetWithContactsByParent(Parent parent)
    {
	    return _context.Set<Student>()
		    .Where(s => s.ParentStudents.Any(ps => ps.ParentId == parent.Id))?
		    .Include(s => s.Contacts)
		    .ToList();
    }

    public Student? GetByContacts(Contact contact)
    {
	    return _context.Set<Student>()
		    .FirstOrDefault(s => s.ContactsId == contact.Id);
    }

    public void UpdateNoTracking(Student existingStudent, Student updatedStudent)
    {
	    _context.Set<Student>().Entry(existingStudent).CurrentValues.SetValues(updatedStudent);
	    _context.SaveChanges();
    }

    public ICollection<Student> GetAllWithClassAndContactsNoTracking()
    {
	    return _context.Set<Student>().AsNoTracking()
		    .Include(s => s.Contacts)
		    .Include(s => s.Class)
		    .ToList();
	}
}
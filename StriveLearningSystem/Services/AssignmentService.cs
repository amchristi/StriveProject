using Data.Context;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Data.DTOs;

namespace Services
{
    public class AssignmentService
    {
        private readonly ClassDbContext _classDbContext;
        public AssignmentService(ClassDbContext classDbContext)
        {
            _classDbContext = classDbContext;
        }

        //Takes a student user id and will return all assignments associated with that id.
        public List<Assignment> GetStudentAssignmentsByUserId(int UserID)
        {
            List<Assignment> assignments = (from a in _classDbContext.Assignments
                                            join uc in _classDbContext.UserCourses on a.CourseID equals uc.CourseID
                                            where uc.UserID == UserID
                                            select a).ToList();
            return assignments;
        }

        //Takes a teacher UserID and will return all assignments associated on the Courses table with that UserID
        public List<Assignment> GetTeacherAssignmentsByUserId(int UserID)
        {
            List<Assignment> assignments = (from a in _classDbContext.Assignments
                                            join c in _classDbContext.Courses on a.CourseID equals c.CourseID
                                            where c.TeacherID == UserID
                                            select a).ToList();
            return assignments;
        }

        public List<CalendarEvent> GetCalendarItems(int userId, string userRole)
        {
            if (userRole == "Student")
            {
                var events = (from a in _classDbContext.Assignments
                              join uc in _classDbContext.UserCourses on a.CourseID equals uc.CourseID
                              where uc.UserID == userId
                              select new CalendarEvent
                              {
                                  Id = a.AssignmentID.ToString(),
                                  AllDay = true,
                                  Start = a.DueDate,
                                  Title = a.AssignmentTitle
                              }).ToList();
                return events;
            }
            else
            {
                var events = (from a in _classDbContext.Assignments
                              join c in _classDbContext.Courses
                                on a.CourseID equals c.CourseID
                              where c.TeacherID == userId
                              select new CalendarEvent
                              {
                                  Id = a.AssignmentID.ToString(),
                                  AllDay = true,
                                  Start = a.DueDate,
                                  Title = a.AssignmentTitle
                              }).ToList();
                return events;
            }
            
        }
    }
}

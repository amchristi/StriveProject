using Data.Context;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Data.DTOs;
using System.Threading.Tasks;

namespace Services
{
    public class AssignmentService
    {
        private readonly ClassDbContext _classDbContext;
        public AssignmentService(ClassDbContext classDbContext)
        {
            _classDbContext = classDbContext;
        }

        //Return a single assignemnt by assignmentId
        public Assignment GetAssignmentByAssignmentID(int AssignmentID)
        {
            var assignment = (from c in _classDbContext.Assignments
                              where c.AssignmentID == AssignmentID
                              select c).FirstOrDefault<Assignment>();
            return assignment;
        }
        //Takes a student user id and will return all assignments associated with that id.
        public List<Assignment> GetStudentAssignmentsByUserId(int UserID)
        {
            var gradedAssignments = (from g in _classDbContext.Grades
                                     join a in _classDbContext.Assignments on g.AssignmentID equals a.AssignmentID
                                     where (g.UserID == UserID)
                                     select a.AssignmentID).AsQueryable();

            List<Assignment> assignments = (from a in _classDbContext.Assignments
                                            join uc in _classDbContext.UserCourses on a.CourseID equals uc.CourseID
                                            where (uc.UserID == UserID)
                                            //&& (a.AssignmentID not in (select g.AssignmentID from g in _classDbContext.Grades))
                                            && (!gradedAssignments.Contains(a.AssignmentID))
                                            select a).OrderBy(m => m.DueDate).Take(5).ToList();
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

        //Return a sorted list of ungraded assignments by teacher 
        public List<AssignmentsToGradeDTO> GetTeacherUngradedAssignmentsByUserId(int UserID)
        {
            List<AssignmentsToGradeDTO> assignments = (from a in _classDbContext.Assignments
                                                       join c in _classDbContext.Courses on a.CourseID equals c.CourseID
                                                       join g in _classDbContext.Grades on a.AssignmentID equals g.AssignmentID
                                                       where c.TeacherID == UserID && !g.IsGraded
                                                       orderby a.DueDate
                                                       select new AssignmentsToGradeDTO
                                                       {
                                                           AssignmentName = a.AssignmentTitle,
                                                           CourseId = c.CourseID,
                                                           CourseName = c.Title,
                                                           DateTurnedIn = g.DateTurnedIn.Value,
                                                           DueDate = a.DueDate,
                                                           StudentId = g.UserID,
                                                           StudentName = "",
                                                           GradeId = g.GradeID
                                                       }).ToList();
            return assignments;
        }

        public async Task<Assignment> UpdateAssignment(Assignment assignment)
        {
            _classDbContext.Assignments.Update(assignment);
            await _classDbContext.SaveChangesAsync();

            return assignment;
        }

        public List<AssignmentSubmissionListDTO> GetAssignmentSubmissions(int assignmentId)
        {
            var users = (from a in _classDbContext.Assignments
                         join uc in _classDbContext.UserCourses
                         on a.CourseID equals uc.CourseID
                         join u in _classDbContext.Users
                         on uc.UserID equals u.UserID
                         where a.AssignmentID == assignmentId
                         select u).ToList();

            var grades = _classDbContext.Grades.Where(m => m.AssignmentID == assignmentId).ToList();

            List<AssignmentSubmissionListDTO> submissions = new List<AssignmentSubmissionListDTO>();

            foreach (var user in users)
            {
                AssignmentSubmissionListDTO assignmentSubmission = new AssignmentSubmissionListDTO();
                assignmentSubmission.StudentId = user.UserID;
                assignmentSubmission.StudentName = user.FirstName + " " + user.LastName;

                var grade = grades.FirstOrDefault(m => m.UserID == user.UserID);
                if (grade != null)
                {
                    assignmentSubmission.GradeId = grade.GradeID;
                    assignmentSubmission.IsGraded = grade.IsGraded;
                    assignmentSubmission.Score = grade.Score.HasValue ? grade.Score.Value : (int?)null;
                    assignmentSubmission.DateTurnedIn = grade.DateTurnedIn.Value;
                }

                submissions.Add(assignmentSubmission);
            }

            return submissions;
        }

        //Returns a list of all the assignments by courseID
        public List<Assignment> GetAssigmentByCourseID(int courseID)
        {
            var assignmentsForCourse = (from a in _classDbContext.Assignments
                                        where a.CourseID == courseID
                                        select a).ToList<Assignment>();
            return assignmentsForCourse;
        }

        //Takes a assignment object and enters it into the database and returns an object
        public async Task<Assignment> AddNewAssignment(Assignment newAssignment)
        {
            // Check if course number exists
            Course course = (from c in _classDbContext.Courses
                             where c.CourseID == newAssignment.CourseID
                             select c).FirstOrDefault<Course>();
            if (course != null && course.CourseID == newAssignment.CourseID)
            {
                var addedAssignment = _classDbContext.Add(newAssignment);
                await _classDbContext.SaveChangesAsync();
                return newAssignment;
            }
            else
            {
                return null;
            }



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

                var classesDays = (from uc in _classDbContext.UserCourses
                                   join c in _classDbContext.Courses
                                    on uc.CourseID equals c.CourseID
                                   where uc.UserID == userId
                                   select c).ToList();

                classesDays.ForEach(c =>
                {
                    CalendarEvent calendarEvent = new CalendarEvent();
                    calendarEvent.AllDay = true;
                    calendarEvent.Title = c.Title;
                    calendarEvent.DaysOfWeek = new List<int>();

                    char[] days = c.MeetingDays.ToUpper().ToCharArray();

                    if (days.Contains('M'))
                    {
                        calendarEvent.DaysOfWeek.Add(1);
                    }
                    if (days.Contains('T'))
                    {
                        calendarEvent.DaysOfWeek.Add(2);
                    }
                    if (days.Contains('W'))
                    {
                        calendarEvent.DaysOfWeek.Add(3);
                    }
                    if (days.Contains('R'))
                    {
                        calendarEvent.DaysOfWeek.Add(4);
                    }
                    if (days.Contains('F'))
                    {
                        calendarEvent.DaysOfWeek.Add(5);
                    }

                    calendarEvent.ClassNames = new string[] {"courseCalendarEvent"};

                    events.Add(calendarEvent);

                });

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


                var classesDays = (from  c in _classDbContext.Courses
                                   where c.TeacherID == userId
                                   select c).ToList();

                classesDays.ForEach(c =>
                {
                    CalendarEvent calendarEvent = new CalendarEvent();
                    calendarEvent.AllDay = true;
                    calendarEvent.Title = c.Title;
                    calendarEvent.DaysOfWeek = new List<int>();

                    char[] days = c.MeetingDays.ToUpper().ToCharArray();

                    if (days.Contains('M'))
                    {
                        calendarEvent.DaysOfWeek.Add(1);
                    }
                    if (days.Contains('T'))
                    {
                        calendarEvent.DaysOfWeek.Add(2);
                    }
                    if (days.Contains('W'))
                    {
                        calendarEvent.DaysOfWeek.Add(3);
                    }
                    if (days.Contains('R'))
                    {
                        calendarEvent.DaysOfWeek.Add(4);
                    }
                    if (days.Contains('F'))
                    {
                        calendarEvent.DaysOfWeek.Add(5);
                    }

                    calendarEvent.ClassNames = new string[] { "courseCalendarEvent" };

                    events.Add(calendarEvent);

                });

                return events;
            }

        }

        public List<int> GetGradesByAssignmentId(int assignmentId)
        {
            var grades = (from assignment in _classDbContext.Assignments
                          join grade in _classDbContext.Grades
                          on assignment.AssignmentID equals grade.AssignmentID
                          where assignment.AssignmentID == assignmentId
                          && grade.Score.HasValue
                          select grade.Score.Value).ToList();

            return grades;
        }
    }
}

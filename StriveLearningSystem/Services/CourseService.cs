using Data.Context;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class CourseService
    {
        private readonly ClassDbContext _classDbContext;
        public CourseService(ClassDbContext classDbContext)
        {
            _classDbContext = classDbContext;
        }
        // Returns the course object from a courseId
        public Course GetCourseById(int courseId)
        {
            Course course = (from c in _classDbContext.Courses
                             where c.CourseID == courseId
                             select c).FirstOrDefault<Course>();
            return course;
        }
        // Takes a user id that is a student and returns a list of courses 
        public List<Course> GetCoursesByStudentID(int inputUserID)
        {

            List<Course> userCourseList = (from uc in _classDbContext.UserCourses
                                           join c in _classDbContext.Courses on uc.CourseID equals c.CourseID
                                           where uc.UserID == inputUserID
                                           select c).ToList();
            return userCourseList;
        }

        // Takes a user id and returns a list of courses taught by that teacher
        public List<Course> GetCourseTaughtByTeacher(int inputUserID)
        {

            List<Course> userCourseList = (from c in _classDbContext.Courses
                                           where c.TeacherID == inputUserID
                                           select c).ToList();
            return userCourseList;
        }

        //Takes a course object and enters it into the database and returns an object with the CourseID
        public async Task<Course> AddNewCourse(Course newCourse)
        {
            var addedCourse = _classDbContext.Add(newCourse);
            await _classDbContext.SaveChangesAsync();
            return newCourse;
         
        }

        //Takes in a course 
        public async Task<Course> UpdateCourse(Course updatedCourse)
        {
            Course checkIfExists = (from c in _classDbContext.Courses
                                    where c.CourseID == updatedCourse.CourseID
                                    select c).FirstOrDefault<Course>();
            if(checkIfExists == null)
            {
                throw new Exception("Course does not exists.");
            }
            var addedCourse = _classDbContext.Update(updatedCourse);
            await _classDbContext.SaveChangesAsync();
            return updatedCourse;

        }

        //Deletes course associated with the course ID throws exception if there is no course with that ID.
        public async Task<int> DeleteCourse(int courseToDeleteID)
        {
            Course courseToDelete = (from c in _classDbContext.Courses
                                    where c.CourseID == courseToDeleteID
                                    select c).FirstOrDefault<Course>();
            if (courseToDelete == null)
            {
                throw new Exception("Course does not exists.");
                
            }

            //Get all the assignments associated with the course
            List<Assignment> assignmentsToDelete = (from a in _classDbContext.Assignments
                                                    where a.CourseID == courseToDeleteID
                                                    select a).ToList<Assignment>();


            //Get all grades associated with the assignments from the course

            foreach (Assignment i in assignmentsToDelete)
            {
                IEnumerable<Grade> tempGrades = (from g in _classDbContext.Grades
                                          where g.AssignmentID == i.AssignmentID
                                          select g).ToList<Grade>();
                //Delete the found grades
                if(tempGrades != null)
                {
                    _classDbContext.RemoveRange(tempGrades);
                }
                tempGrades = null;
                
            }
            if(assignmentsToDelete != null)
            {
                _classDbContext.RemoveRange(assignmentsToDelete);
            }


            //Delete all Announcements associated with course
            IEnumerable<Announcement> announcementsToDelete = (from a in _classDbContext.Announcements
                                                               where a.CourseID == courseToDeleteID
                                                               select a).ToList<Announcement>();
            if(announcementsToDelete != null)
            {
                _classDbContext.RemoveRange(announcementsToDelete);
            }

            //Delete all userCourses associated with course
            IEnumerable<UserCourse> userCoursesToDelete = (from uc in _classDbContext.UserCourses
                                                               where uc.CourseID == courseToDeleteID
                                                               select uc).ToList<UserCourse>();
            if (userCoursesToDelete != null) 
            {
                _classDbContext.RemoveRange(userCoursesToDelete);
            } 

            _classDbContext.Remove(courseToDelete);
            await _classDbContext.SaveChangesAsync();
            return courseToDeleteID;
      

        }

    }
}

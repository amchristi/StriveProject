using Data.Context;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
        public List<Course> GetClasses(int inputUserID)
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
    }
}

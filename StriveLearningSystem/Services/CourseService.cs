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

        public async Task<Course> DeleteCourse(Course courseToRemove)
        {
            Course checkIfExists = (from c in _classDbContext.Courses
                                    where c.CourseID == courseToRemove.CourseID
                                    select c).FirstOrDefault<Course>();
            if (checkIfExists == null)
            {
                throw new Exception("Course does not exists.");
            }

            //Delete all assignments associated with course
            //Delete all grades associated with course
            //Delete all Announcements associated with course
            //Delete all userCourses associated with course
            _classDbContext.Remove(courseToRemove);
            await _classDbContext.SaveChangesAsync();
            return courseToRemove;

        }

    }
}

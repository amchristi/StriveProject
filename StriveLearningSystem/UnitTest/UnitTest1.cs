using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private AssignmentService _assignmentService;
        private CourseService _courseservice;
        private ClassDbContext _dbContext;
        private UserService _userService;

        public UnitTest1()
        {


        }

        [TestMethod]
        public void AssignmentIsNotNullForStudent()
        {
            var assignments = _assignmentService.GetStudentAssignmentsByUserId(16);
            Assert.IsNotNull(assignments);
        }

        [TestMethod]
        public void CourseNotNull()
        {
            Assert.IsNotNull(_courseservice.getCourses());
        }

        [TestMethod]
        public void UngradedAssignmentIsNotNullForTeacher()
        {
            var assignments = _assignmentService.GetTeacherUngradedAssignmentsByUserId(3);
            Assert.IsNotNull(assignments);
        }

        [TestMethod]
        public void CoursesIsNotNullForTeacher()
        {
            var courses = _courseservice.GetCourseTaughtByTeacher(3);
            Assert.IsNotNull(courses);
        }

        [TestMethod]
        public void AssignmentIsNotNullForTeacher()
        {
            var assignments = _assignmentService.GetTeacherAssignmentsByUserId(3);
            Assert.IsNotNull(assignments);
        }

        [TestMethod]
        public void UserNotNULL()
        {
            Assert.IsTrue(_userService.GetAllUsers() != null);
        }


        // Inserts a new assignment and tests to ensure that it is added and retrieved from the database correctly.
        [TestMethod]
        public async Task CheckAssignmentAddingAndRetriving()
        {
            Assignment testAssignment = new Assignment();
            //testAssignment.AssignmentID = 999;
            testAssignment.CourseID = 1;
            testAssignment.DueDate = DateTime.Now;
            testAssignment.AssignmentTitle = "testing";
            testAssignment.AssignmentDescription = "testDescription";
            testAssignment.AssignmentType = "test";
            testAssignment.TotalPossible = 10;

            var returnedAssignment = await _assignmentService.AddNewAssignment(testAssignment);
            List<Assignment> assignmentList = (_assignmentService.GetAssigmentByCourseID(1));

            Assignment checkAssignment = null;

            for (int i = 0; i < assignmentList.Count; i++)
            {
                if (assignmentList[i].AssignmentID == returnedAssignment.AssignmentID)
                {
                    checkAssignment = assignmentList[i];
                    break;
                }
            }

            Assert.IsTrue(testAssignment.CourseID == checkAssignment.CourseID &&
               testAssignment.AssignmentID == checkAssignment.AssignmentID &&
               testAssignment.DueDate == checkAssignment.DueDate &&
               testAssignment.AssignmentTitle == checkAssignment.AssignmentTitle &&
               testAssignment.AssignmentDescription == checkAssignment.AssignmentDescription &&
               testAssignment.AssignmentType == checkAssignment.AssignmentType &&
               testAssignment.TotalPossible == checkAssignment.TotalPossible);

            _dbContext.Remove(testAssignment);
            await _dbContext.SaveChangesAsync();
        }

        // Tries to input a assignment with an incorrect course number
        [TestMethod]
        public async Task CheckAssignmentWithInvalidCourse()
        {
            Assignment testAssignment = new Assignment();
            testAssignment.CourseID = 0;
            testAssignment.DueDate = DateTime.Now;
            testAssignment.AssignmentTitle = "testing";
            testAssignment.AssignmentDescription = "testDescription";
            testAssignment.AssignmentType = "test";
            testAssignment.TotalPossible = 10;

            var returnedAssignment = await _assignmentService.AddNewAssignment(testAssignment);
            Assert.IsNull(returnedAssignment);
        }

        [DataTestMethod]
        [DataRow("classid1")]
        [DataRow("classid2")]
        public void DummyTest(string classID)
        {
            //This will change the classID to the different data point 

            var assignments = _assignmentService.GetStudentAssignmentsByUserId(16);
            Assert.IsNotNull(assignments);
        }

        [TestInitialize]
        public void StartUp()
        {
            var contextOptions = new DbContextOptionsBuilder().UseSqlServer("Server = titan.cs.weber.edu,10433; User Id=Strive; Password=Password*1; Database=LMS_Strive").Options;
            _dbContext = new ClassDbContext(contextOptions);
            _assignmentService = new AssignmentService(_dbContext);
            _courseservice = new CourseService(_dbContext);
            _userService = new UserService(_dbContext);

        }

        [TestCleanup]
        public void TearDown()
        {

        }

        [ClassInitialize]
        public static void InitializingClass(TestContext testContext)
        {



        }
    }
}
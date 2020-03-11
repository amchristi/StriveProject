using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;

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
        public void UserNotNULL()
        {
            Assert.IsTrue(_userService.GetAllUsers() != null);
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

        }

        [TestCleanup]
        public void TearDown()
        {

        }

        [ClassInitialize]
        public void InitializingClass()
        {
            var contextOptions = new DbContextOptionsBuilder().UseSqlServer("Server = titan.cs.weber.edu,10433; User Id=Strive; Password=Password*1; Database=LMS_Strive").Options;
            _dbContext = new ClassDbContext(contextOptions);
            _assignmentService = new AssignmentService(_dbContext);
            _courseservice = new CourseService(_dbContext);
            _userService = new UserService(_dbContext);
        }
    }
}

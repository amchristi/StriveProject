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

        public UnitTest1()
        {
            var contextOptions = new DbContextOptionsBuilder().UseSqlServer("Server = titan.cs.weber.edu,10433; User Id=Strive; Password=Password*1; Database=LMS_Strive").Options;
            var _dbContext = new ClassDbContext(contextOptions);
            _assignmentService = new AssignmentService(_dbContext);
        }

        [TestMethod]
        public void AssignmentIsNotNullForStudent()
        {
            var assignments = _assignmentService.GetStudentAssignmentsByUserId(17);
            Assert.IsNotNull(assignments);
        }
    }
}

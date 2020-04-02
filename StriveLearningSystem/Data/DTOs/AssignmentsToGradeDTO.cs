using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTOs
{
    public class AssignmentsToGradeDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string AssignmentName { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime DateTurnedIn { get; set; }
        public string StudentName { get; set; }
        public int StudentId { get; set; }
        public int GradeId { get; set; }
    }
}

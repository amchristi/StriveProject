using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Assignment
    {
        [Key]
        public int AssignmentID { get; set; }
        public int CourseID { get; set; }
        public DateTime DueDate { get; set; }
        public string AssignmentDescription { get; set;}
        public string AssignmentType { get; set; }

    }
}

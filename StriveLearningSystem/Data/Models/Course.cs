using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Models
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }
        public String Description { get; set; }
        public int TeacherID { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string MeetingDays { get; set; }
        public int CreditHours { get; set; }
        
    }
}

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
        public String Teacher { get; set; } //This field is redundant and should be removed once references to it are removed
        public String Description { get; set; }
        public int TeacherID { get; set; }

    }
}

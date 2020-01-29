using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class UserCourse
    {
        [Key]
        public int UserID { get; set; }
        [Key]
        public int CourseID { get; set; }
    }
}

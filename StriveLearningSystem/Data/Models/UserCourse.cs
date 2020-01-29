using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class UserCourse
    {
        public int UserID { get; set; }
        public int CourseID { get; set; }
    }
}

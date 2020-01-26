using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Models
{
    public class Course
    {
        [Key]
        public String ClassID { get; set; }
        public String Teacher { get; set; }
        public String Description { get; set; }
        public List<User> Students { get; set; }
               
    }
}

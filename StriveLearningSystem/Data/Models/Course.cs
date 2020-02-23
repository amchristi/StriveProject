using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Models
{
    public class Course: IEquatable<Course>
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



        public bool Equals(Course other)
        {
            if (other == null) return false;
            return other.CourseID == this.CourseID;
        }

        public int GetHashCode(Course obj)
        {
            return obj.CourseID;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Models
{
    public class CourseInformationDTO: IEquatable<CourseInformationDTO>
    {
        [Key]
        public int CourseID { get; set; }
        public string Description { get; set; }
        public int TeacherID { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string MeetingDays { get; set; }
        public int CreditHours { get; set; }
        public string TeacherName { get; set; }



        public bool Equals(CourseInformationDTO other)
        {
            if (other == null) return false;
            return other.CourseID == this.CourseID;
        }

        public int GetHashCode(CourseInformationDTO obj)
        {
            return obj.CourseID;
        }
    }
}

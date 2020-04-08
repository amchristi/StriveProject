using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Announcement
    {
        [Key]
        public int AnnouncementID { get; set; }
        public int CourseID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.Now.AddDays(3);

    }
}

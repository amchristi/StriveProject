using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class TempGrade
    {
        [Key]
        public int GradeID { get; set; }
        public int AssignmentID { get; set; }
        public int UserID { get; set; }
        public int Score { get; set; }
        public DateTime DateTurnedIn { get; set; }
        public DateTime DateGraded { get; set; }
        public bool IsGraded { get; set; }
        public String TextSubmission { get; set; }
        public string FileURl { get; set; }
        public bool IsFile { get; set; }
    }
}

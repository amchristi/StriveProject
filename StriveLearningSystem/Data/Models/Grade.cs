using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Grade
    {
        [Key]
        public int GradeID { get; set; }
        public int AssignmentID { get; set; }
        public int UserID { get; set; }
        public int? Score { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateTurnedIn { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateGraded { get; set; }
        public bool IsGraded { get; set;}
        public string TextSubmission { get; set; }
        public string FileURl { get; set; }
        public bool IsFile { get; set; }
    }
}

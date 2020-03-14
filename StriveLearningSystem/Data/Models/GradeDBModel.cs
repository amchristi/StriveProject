using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class GradeDBModel
    {
        [Key]
        public int GradeID { get; set; }
        public int AssignmentID { get; set; }
        public int UserID { get; set; }
        public int Score { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateTurnedIn { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateGraded { get; set; }
        public bool IsGraded { get; set;}
        public String TextSubmission { get; set; }
        public String FileURl { get; set; }
        public bool IsFile { get; set; }
    }
}

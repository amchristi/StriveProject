using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTOs
{
    public class AssignmentSubmissionListDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int GradeId { get; set; }
        public int? Score { get; set; }
        public DateTime? DateTurnedIn { get; set; }
        public bool IsGraded { get; set; }
    }
}

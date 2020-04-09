using Data.Context;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Services
{
    public class GradeService
    {

        private readonly ClassDbContext _classDbContext;
        private Grade gradeDB;
        public GradeService(ClassDbContext classDbContext)
        {
            _classDbContext = classDbContext;
        }

      
        public async Task<Grade> SubmitAssignment(Grade tempGrade)
        {
            //gradeDB = ConvertoDBModel(tempGrade);
            var grade = _classDbContext.Add(tempGrade);
            await _classDbContext.SaveChangesAsync();
            return tempGrade;

        }


        // Takes in a fileAssignment and writes creates the file on the server and returns the url.
        public FileAssignment UploadAssignmentFile(FileAssignment fileAssignment)
        {
            // Save assignment on the server and update the Url in the FileAssignment object and pass it back
            string cwd = Directory.GetCurrentDirectory();
            //Store only the AssignmentFiles and filename in the database
            fileAssignment.URL = "\\" + "AssignmentFiles\\" + fileAssignment.Name;
            // Add in the current working directory to save the file on the server
            var path = cwd + fileAssignment.URL;
            File.WriteAllBytes(path, fileAssignment.Data);
            
            return fileAssignment;

            
        }

        // Check if grade is submitted. Return new grade object if exists.
        public Grade CheckForGrade(Grade grade)
        {
            Grade retGrade = (from g in _classDbContext.Grades
                              where (g.AssignmentID == grade.AssignmentID)
                              && (g.UserID == grade.UserID)
                              select g).FirstOrDefault<Grade>();
            if (retGrade == null)
                return grade;

            return retGrade;
        }
        public bool Download(String dowload)
        {

            return false;
        }
        public Grade GetGrade(int gradeId)
        {
            return _classDbContext.Grades.FirstOrDefault(m => m.GradeID == gradeId);
        }

        public async Task<Grade> UpdateGrade(Grade grade)
        {
            _classDbContext.Grades.Update(grade);
            await _classDbContext.SaveChangesAsync();

            return grade;
        }
    }
}

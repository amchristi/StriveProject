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
        private GradeDBModel gradeDB;
        public GradeService(ClassDbContext classDbContext)
        {
            _classDbContext = classDbContext;
        }

        
        public async Task<TempGrade> SubmitAssignmentFile(TempGrade tempGrade)
        {
            gradeDB = ConvertoDBModel(tempGrade);
            var addgrade = _classDbContext.Add(gradeDB);
            await _classDbContext.SaveChangesAsync();
              
            return tempGrade;
            
        }


        public async  Task<TempGrade> SubmitAssignmentText(TempGrade tempGrade)
        {
            gradeDB = ConvertoDBModel(tempGrade);
            var addgrade = _classDbContext.Add(tempGrade);
            await _classDbContext.SaveChangesAsync();
            return tempGrade;

        }

       
        public GradeDBModel ConvertoDBModel(TempGrade tempGrade)
        {
             
            GradeDBModel dBModel = new GradeDBModel();         
            if (tempGrade.TextSubmission == null)
            {              
                dBModel.FileURl = tempGrade.FileURl;
                dBModel.IsFile = true;
            }
            else
            {
                dBModel.TextSubmission = tempGrade.TextSubmission;
            }
            dBModel.AssignmentID = tempGrade.AssignmentID;
            dBModel.UserID = tempGrade.UserID;
            dBModel.DateTurnedIn = DateTime.Now;
                       
            return dBModel;
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


    }
}

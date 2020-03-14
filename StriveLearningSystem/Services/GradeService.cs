using Data.Context;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

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

        //upload assignment
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

        //This I would call the class to store the file and get back a fileURL
        public GradeDBModel ConvertoDBModel(TempGrade tempGrade)
        {
             
            GradeDBModel dBModel = new GradeDBModel();
            //dBMode FileURL is of type instead of Type Byte[] for the 
            if (tempGrade.TextSubmission == null)
            {
                dBModel.FileURl = "ADD URL HERE";//After storing the file put the URL here thats it.
            }
            else
            {
                dBModel.TextSubmission = tempGrade.TextSubmission;
            }
            dBModel.AssignmentID = tempGrade.AssignmentID;
            dBModel.UserID = tempGrade.UserID;
            return dBModel;
        }
    }
}

using Data.Context;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Services
{
    public class AssignmentService
    {
        private readonly ClassDbContext _classDbContext;
        public AssignmentService(ClassDbContext classDbContext)
        {
            _classDbContext = classDbContext;
        }

        
    }
}

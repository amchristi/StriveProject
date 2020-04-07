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
    public class AnnouncementService
    {

        private readonly ClassDbContext _classDbContext;
        public AnnouncementService(ClassDbContext classDbContext)
        {
            _classDbContext = classDbContext;
        }

      
        public async Task<Announcement> CreateAnnouncement(Announcement tempAnnouncement)
        {
            _classDbContext.Add(tempAnnouncement);
            await _classDbContext.SaveChangesAsync();
            return tempAnnouncement;

        }
    }
}

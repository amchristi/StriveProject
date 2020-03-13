using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;

namespace Data.Context
{
    public class ClassDbContext : DbContext
    { 
        public ClassDbContext(DbContextOptions options):base(options) { }
        public DbSet<Course> Courses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCourse>()
                .HasKey(o => new { o.CourseID, o.UserID });
        }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<GradeDBModel> Grades { get; set; }
        public DbSet<Announcement> Announcements { get; set; }

      

    }
}


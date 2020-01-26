using Data.Context;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class UserService
    {
        private readonly ClassDbContext _classDbContext;

        public UserService(ClassDbContext classDbContext)
        {
            _classDbContext = classDbContext;
        }

        public async Task<List<User>> GetUser()
        {
            //Can be deleted once the page that uses it is deleted
            //Go to DB Context and Get users
            List<User> usersList = new List<User>();
            usersList.Add(new User() { Name = "Trevor" });


            return usersList;
        }

        // Authenticate User
        // Takes a password and email and returns a user object associated with those credentials.
        public User AuthenticateUser(string password, string email)
        {
            User user = _classDbContext.Users
                        .Where(u => u.Email == email && u.Password == password)
                        .FirstOrDefault<User>();
            return user;
        }

        // Register new User 
        // Takes a user object, checks if it the email is unique and adds to database.
        // Returns the newUserObject with an id field if add was successful otherwise the user object is returned unchanged.
        public async Task<List<User>> AddNewUser(User newUser)
        {
         
            List<User> userList = new List<User>();

            // Check if email is already in use
            User checkUnique = _classDbContext.Users
                               .Where(u => u.Email == newUser.Email)
                               .FirstOrDefault<User>();

            if (checkUnique == null)
            {
                // Email is unique add to database
                _classDbContext.Add(newUser);
                await _classDbContext.SaveChangesAsync();

                // Update the newUser object so it includes the ID
                newUser = _classDbContext.Users
                         .Where(u => u.Email == newUser.Email)
                         .FirstOrDefault<User>();
            }

            // Add the newUser object to the return list. 
            userList.Add(newUser);
            return userList;
        }

        // Takes a user object and returns a list of the courses for that user.
        public List<Course> GetClasses(User inputUser) 
        {
            User student = _classDbContext.Users
                           .Where(u => u.UserId == inputUser.UserId)
                           .FirstOrDefault<User>();
            List<Course> courseList = student.Courses;
            return courseList;
        }

    }
}

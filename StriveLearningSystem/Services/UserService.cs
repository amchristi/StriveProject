using Data.Context;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public List<User> GetAllUsers()
        {
            return _classDbContext.Users.ToList();
        }

        // Authenticate User
        // Takes a password and email and returns a user object associated with those credentials.
        public User AuthenticateUser(string password, string email)
        {
            var hashedPassword = HashPassword("strivesalt", password);

            User user = _classDbContext.Users
                        .Where(u => u.Email == email && u.Password == hashedPassword)
                        .FirstOrDefault<User>();

            if (user == null)
            {
                throw new Exception("Invalid username or password");
            }
            return user;
        }

        // Register new User 
        // Takes a user object, checks if it the email is unique and adds to database.
        // Returns the newUserObject with an id field if add was successful otherwise the user object is returned unchanged.
        public async Task<User> AddNewUser(User newUser)
        {
            // Check if email is already in use
            User checkUnique = _classDbContext.Users
                               .Where(u => u.Email == newUser.Email)
                               .FirstOrDefault<User>();

            if (checkUnique == null)
            {
                //Hash password
                newUser.Password = HashPassword("strivesalt", newUser.Password);

                // Email is unique add to database
                _classDbContext.Add(newUser);
                await _classDbContext.SaveChangesAsync();

                // Update the newUser object so it includes the ID
                newUser = _classDbContext.Users
                         .Where(u => u.Email == newUser.Email)
                         .FirstOrDefault<User>();
            }
            else
            {
                throw new Exception("User already exists");
            }

            return newUser;
        }

        

        //Take a user object with at least a userId and fill in all the fields and return.
        public User GetProfile(User user)
        {
            User user1 =  _classDbContext.Users
                         .Where(u => u.UserID == user.UserID)
                         .FirstOrDefault<User>();

            return user1;
        }

        public async Task<User> UpdateProfile(User user)
        {
            _classDbContext.Add(user);
            await _classDbContext.SaveChangesAsync();

            User user1 = _classDbContext.Users
                         .Where(u => u.UserID == user.UserID)
                         .FirstOrDefault<User>();

            return user1;
        }

        public string HashPassword(string salt, string password)
        {
            Rfc2898DeriveBytes HashedPass = new Rfc2898DeriveBytes(password,
                System.Text.Encoding.UTF8.GetBytes(salt), 10000);
            return Convert.ToBase64String(HashedPass.GetBytes(25));
        }

        //Takes a student user id and will return all assignments associated with that id.
        public List<Assignment> GetStudentAssignmentsByUserId(int UserID) 
        {
            List<Assignment> assignments = (from a in _classDbContext.Assignments
                                            join uc in _classDbContext.UserCourses on a.CourseID equals uc.CourseID
                                            where uc.UserID == UserID
                                            select a).ToList();
            return assignments;
        }

        //Takes a teacher UserID and will return all assignments associated on the Courses table with that UserID
        public List<Assignment> GetTeacherAssignmentsByUserId(int UserID)
        {
            List<Assignment> assignments = (from a in _classDbContext.Assignments
                                            join c in _classDbContext.Courses on a.CourseID equals c.CourseID
                                            where c.TeacherID == UserID
                                            select a).ToList();
            return assignments;
        }


        //Return a sorted list of ungraded assignments by teacher 
        public List<Assignment> GetTeacherUngradedAssignmentsByUserId(int UserID)
        {
            List<Assignment> assignments = (from a in _classDbContext.Assignments
                                            join c in _classDbContext.Courses on a.CourseID equals c.CourseID
                                            join g in _classDbContext.Grades on a.AssignmentID equals g.AssignmentID
                                            where c.TeacherID == UserID && !g.IsGraded
                                            orderby a.DueDate
                                            select a).ToList();
            return assignments; 
        }
        
    }
}

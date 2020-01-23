using Data.Models;
using System;
using System.Collections.Generic;

namespace Services
{
    public class UserService
    {
        public UserService()
        {

        }

        public List<User> GetUser()
        {
            //Go to DB Context and Get users
            List<User> usersList = new List<User>();
            usersList.Add(new User() { Name = "Trevor" });
            return usersList;
        }
    }
}

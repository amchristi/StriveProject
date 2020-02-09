using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        public bool IsTeacher { get; set; }
        public int Phone { get; set; }
        public string Address { get; set; } 
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public string External_Link2 { get; set; }
    }
}

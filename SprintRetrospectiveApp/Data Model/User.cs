/**Purpose: this class models data related to user's info
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SprintRetrospectiveApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public List<int> ProjectIdCollection { get; set; }

        // Nick Version
        //public User(int Id, string FirstName, string LastName, string Role)
        //{
        //    this.Id = Id;
        //    this.FirstName = FirstName;
        //    this.LastName = LastName;
        //    this.Role = Role;
        //}

        // Jason Version
        public User(int Id, string FirstName, string LastName, string Role, List<int> ProjectIdCollection)
        {
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Role = Role;
            this.ProjectIdCollection = ProjectIdCollection;
        }
    }
}

/**Purpose: this class models data captured when user create a new project
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SprintRetrospectiveApp.Models
{
    public class Project
    {
        
        public int Id { get; set; }
       
        public string ProjectName { get; set; }
       
        public string Description { get; set; }
        
        public string TeamName { get; set; }
        
        public string TeamNumber { get; set; }
        public string HyperlinkClickup { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public double InitialVelocity { get; set; }
        
        public double HoursPerStoryPoint { get; set; }

        public List<UserStory> UserStoryCollection { get; set; }

        public List<Sprint> SprintCollection { get; set; }

        //public List<User> UserCollection { get; set; }

        //public Project(int Id, string ProjectName, string Description, string TeamName, string TeamNumber, string HyperlinkClickup, DateTime StartDate, double InitialVelocity, double HoursPerStoryPoint, List<UserStory> UserStoryCollection, List<Sprint> SprintCollection, List<User> UserCollection)
           
        //{
        //    this.Id = Id;
        //    this.ProjectName = ProjectName;
        //    this.Description = Description;
        //    this.TeamName = TeamName;
        //    this.TeamNumber = TeamNumber;
        //    this.HyperlinkClickup = HyperlinkClickup;
        //    this.StartDate = StartDate;
        //    this.InitialVelocity = InitialVelocity;
        //    this.HoursPerStoryPoint = HoursPerStoryPoint;
        //    this.UserStoryCollection = UserStoryCollection;
        //    this.SprintCollection = SprintCollection;
        //    this.UserCollection = UserCollection;
        //}

        public Project(int Id, string ProjectName, string Description, string TeamName, string TeamNumber, string HyperlinkClickup, DateTime StartDate, double InitialVelocity, double HoursPerStoryPoint, List<UserStory> UserStoryCollection, List<Sprint> SprintCollection)

        {
            this.Id = Id;
            this.ProjectName = ProjectName;
            this.Description = Description;
            this.TeamName = TeamName;
            this.TeamNumber = TeamNumber;
            this.HyperlinkClickup = HyperlinkClickup;
            this.StartDate = StartDate;
            this.InitialVelocity = InitialVelocity;
            this.HoursPerStoryPoint = HoursPerStoryPoint;
            this.UserStoryCollection = UserStoryCollection;
            this.SprintCollection = SprintCollection;
        }

        public override string ToString()
        {
            return $"Id: {Id}\nProject Name: {ProjectName}\nDescription: {Description}\nTeam Name: {TeamName}";
        }
    }
}

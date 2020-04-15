/**
 * Purpose: this class models data captured when user create a new user story
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SprintRetrospectiveApp.Models
{
    public class UserStory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SprintId { get; set; }
        public int StoryPoint { get; set; }
        public string Description { get; set; }
        public double InitialEstimatedHours { get; set; }
        public double ActualWorkHours { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public string Status { get; set; }

        public List<Subtask> SubtaskCollection { get; set; }


        public UserStory(int Id, string description, int storyPoint)
        {
            this.Id = Id;
            this.UserId = 0;
            this.SprintId = 0;
            this.StoryPoint = storyPoint;
            this.Description = description;
            this.InitialEstimatedHours = 0;
            this.ActualWorkHours = 0;
            this.LastUpdatedTime = DateTime.Now;
            this.Status = "";
            this.SubtaskCollection = new List<Subtask>();

        }

        public UserStory(int Id, int UserId, int SprintId, int StoryPoint, string Description, double InitialEstimatedHours, double ActualWorkHours, DateTime LastUpdatedTime, string Status, List<Subtask> SubtaskCollection)
        {
            this.Id = Id;
            this.UserId = UserId;
            this.SprintId = SprintId;
            this.StoryPoint = StoryPoint;
            this.Description = Description;
            this.InitialEstimatedHours = InitialEstimatedHours;
            this.ActualWorkHours = ActualWorkHours;
            this.LastUpdatedTime = LastUpdatedTime;
            this.Status = Status;
            this.SubtaskCollection = SubtaskCollection;
        }

        public override string ToString()
        {
            //return $"Id: {Id}\nUserId: {UserId}\nDescription: {Description}\nInitialEstimatedHours: {InitialEstimatedHours}\nActualWorkHours: {ActualWorkHours}\nStatus: {Status}";
            return $"Id: {Id}\nSprintId: {SprintId}\nDescription: {Description}\nInitialEstimatedHours: {InitialEstimatedHours}\nActualWorkHours: {ActualWorkHours}\nStatus: {Status}";
        }

        
    }
}

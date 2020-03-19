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
        public int SprintId { get; set; }
        public string Description { get; set; }
        public double InitialEstimatedHours { get; set; }
        public double ActualWorkHours { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public string Status { get; set; }

        public List<Subtask> SubtaskCollection { get; set; }


        public UserStory(int Id, int SprintId, string Description, double InitialEstimatedHours, double ActualWorkHours, DateTime LastUpdatedTime, string Status, List<Subtask> SubtaskCollection)
        {
            this.Id = Id;
            this.SprintId = SprintId;
            this.Description = Description;
            this.InitialEstimatedHours = InitialEstimatedHours;
            this.ActualWorkHours = ActualWorkHours;
            this.LastUpdatedTime = LastUpdatedTime;
            this.Status = Status;
            this.SubtaskCollection = SubtaskCollection;
        }

        public override string ToString()
        {
            return $"Id: {Id}\nDescription: {Description}\nInitialEstimatedHours: {InitialEstimatedHours}\nActualWorkHours: {ActualWorkHours}\nStatus: {Status}";
        }
    }
}

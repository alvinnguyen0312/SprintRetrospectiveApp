/**
 * Purpose: this class models data relevant to subtasks created and assigned in each userstory
 * 
 */
using System;
namespace SprintRetrospectiveApp.Models
{
    public class Subtask
    {
        public int Id { get; set; }

        public DateTime LastUpdatedTime { get; set; }

        public string Description { get; set; }

        public double ActualWorkHours { get; set; }

        public Subtask(int Id, DateTime LastUpdatedTime, string Description, double ActualWorkHours)
        {
            this.Id = Id; 
            this.LastUpdatedTime = LastUpdatedTime;
            this.Description = Description;
            this.ActualWorkHours = ActualWorkHours;
        }

        public override string ToString()
        {
            return $"Description: {Description}\nActualWorkHours: {ActualWorkHours}";
        }
    }
}

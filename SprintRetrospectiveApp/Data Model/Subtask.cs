/**
 * Purpose: this class models data relevant to subtasks created and assigned in each userstory
 * 
 */
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SprintRetrospectiveApp.Models
{
    public class Subtask
    {
        public int Id { get; set; }

        public DateTime LastUpdatedTime { get; set; }

        public string Description { get; set; }
        
        public double InitialEstimatedHours { get; set; }

        public double ActualWorkHours { get; set; }

        public Subtask(int Id, DateTime LastUpdatedTime, string Description, double InitialEstimatedHours, double ActualWorkHours)
        {
            this.Id = Id; 
            this.LastUpdatedTime = LastUpdatedTime;
            this.Description = Description;
            this.InitialEstimatedHours = InitialEstimatedHours;
            this.ActualWorkHours = ActualWorkHours;
        }

        public override string ToString()
        {
            return $"ID: {Id}\nDescription: {Description}\nInitialEstimatedHours: {InitialEstimatedHours}\nActualWorkHours: {ActualWorkHours}";
        }
    }
}

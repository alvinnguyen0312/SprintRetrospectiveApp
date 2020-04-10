using SprintRetrospectiveApp.Data_Manipulation;
using System;

namespace SprintRetrospectiveApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ProjectCRU proj = new ProjectCRU();
            //Get all user story by project ID
            var allUserStories = proj.GetAllUserStoryByProject(0);
            foreach (var us in allUserStories)
            {
                Console.WriteLine(us.ToString());
            }

            //Get all User IDs by Project ID
            var allUserIDs = proj.GetAllUsersByProject(0);
            foreach (var ui in allUserIDs)
            {
                Console.WriteLine(ui);
            }

            //Get a list of user's full name
            var usersFullName = proj.GetUserFullNameList();
            foreach (var n in usersFullName)
            {
                Console.WriteLine(n);
            }

            //Get all stories by User ID
            var listStories = proj.GetAllUserStoryByUserID(0, 0);
            foreach (var s in listStories)
            {
                Console.WriteLine(s.Description);
            }

            //Get total estimated time by user ID
            double totalTime = proj.GetTotalEstimatedTimeByUser(3, 0);
            Console.WriteLine(totalTime);

            //Get total estimated time by user ID and storyID
            double totalTime1 = proj.GetTotalEstimatedTimeByUser(0, 0);
            Console.WriteLine(totalTime1);

            //Get user story by Sprint ID
            var userStoryBySprint = proj.GetAllUserStoryBySprintID(0, 0);
            foreach (var s in userStoryBySprint)
            {
                Console.WriteLine(s.Description);
            }

            //Update velocity for a specific sprint
            //Console.WriteLine("Before Update:");
            //foreach(var a in proj.GetAllSprintsByProject(0))
            //{
            //    Console.WriteLine(a.ToString());
            //}
            //proj.UpdateVelocityForSprint(30, 2, 0);//set new velo to 30 from sprint 2
            //Console.WriteLine("After Update:");
            //foreach (var a in proj.GetAllSprintsByProject(0))
            //{
            //    Console.WriteLine(a.ToString());
            //}

            //get all user stories by its status (Planned, Started, Delivered)
            var userstories = proj.GetAllUserStoryByStatus("Testing", 0);
            foreach (var s in userstories)
            {
                Console.WriteLine(s);
            }

            //get all subtask by user story ID
            var subTasklist = proj.GetAllSubtasksByUserStory(0, 0);
            foreach (var s in subTasklist)
            {
                Console.WriteLine(s.ToString());
            }

            //test estimate/actual time for subtask by subtask, story
            //double time = proj.GetActualTimeBySubTaskAndUserStory(1, 2, 0);
            //double time = proj.GetEstimatedTimeBySubTaskAndUserStory(1, 2, 0);
            //double time = proj.GetEstimatedTimeByUserStory(2, 0);
            //double time = proj.GetActualTimeByUserStory(2, 0);

            //Console.WriteLine(time);

            //update story status
            //proj.UpdateStotyStatus("Delivered", 1, 0);
            //var story = proj.GetAllUserStoryByStatus("Delivered", 0);
            //foreach(var s in story)
            //{
            //    Console.WriteLine(s);
            //}
        }
    }
}

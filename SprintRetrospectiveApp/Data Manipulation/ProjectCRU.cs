using SprintRetrospectiveApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SprintRetrospectiveApp.Data_Manipulation
{
    class ProjectCRU
    {
        public List<Project> projectList = new List<Project>();
        public List<User> userList = new List<User>(); 

        public ProjectCRU()
        {
            //Load all project into container projectList
            //TODO
            //Testing
            List<UserStory> userStoryColl = new List<UserStory>();
            List<Sprint> sprintColl = new List<Sprint>();

            //Add first project
            projectList.Add(new Project(1, "My First Project", "Project managing app", "JAN", "53", "https://app.clickup.com/1234698/d/b?p=-1", DateTime.UtcNow, 60.00, 2, userStoryColl, sprintColl));
            //Add two user stories, each user story has a different collection of subtask
            //First subtask collection
            List<Subtask> subTaskColl1 = new List<Subtask>();
            subTaskColl1.Add(new Subtask(1, 1, DateTime.Now, "Create a basic menu subtask 1", 2.0, 2.5));
            subTaskColl1.Add(new Subtask(2, 2, DateTime.Now, "Create a basic menu subtask 2", 3.0, 3.5));
            subTaskColl1.Add(new Subtask(3, 2, DateTime.Now, "Create a basic menu subtask 3", 4.0, 4.5));

            //Second subtask collection
            List<Subtask> subTaskColl2 = new List<Subtask>();
            subTaskColl2.Add(new Subtask(1, 1, DateTime.Now, "Create a basic homepage subtask 1", 2.0, 2.5));
            subTaskColl2.Add(new Subtask(2, 1, DateTime.Now, "Create a basic homepage subtask 2", 3.0, 3.5));
            subTaskColl2.Add(new Subtask(3, 3, DateTime.Now, "Create a basic homepage subtask 3", 4.0, 4.5));

            UserStory us1 = new UserStory(1, 1, "Provide a basic menu system for the application", 5.5, 4.0, DateTime.Now, "Planned", subTaskColl1);
            UserStory us2 = new UserStory(2, 2, "Provide a basic homepage for the application", 6.5, 7.0, DateTime.Now, "Delivered", subTaskColl2);
            userStoryColl.Add(us1);
            userStoryColl.Add(us2);
            //Add users to the List
            userList.Add(new User(1, "Alvin", "Nguyen", "Developer"));
            userList.Add(new User(2, "Jason", "Oh", "Developer"));
            userList.Add(new User(3, "Nic", "Prezio", "Developer"));

            //print collection
            sprintColl.Add(new Sprint(1, 20));
            sprintColl.Add(new Sprint(2, 20));
            sprintColl.Add(new Sprint(3, 20));
        }

        /// <summary>
        /// Get a collection of user story by project ID
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<UserStory> GetAllUserStoryByProject(int projectId)
        {
            return projectList[projectId].UserStoryCollection;
        }

        /// <summary>
        /// Get list of UserID of all users being involved in the project (means having subtasks in that project)
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<int> GetAllUsersByProject(int projectId)
        {
            var allUserStories = GetAllUserStoryByProject(projectId);
            //loop through each user story
            foreach(var us in allUserStories)
            {
                //look for distinct userID in subtasks list
                var userIDList = us.SubtaskCollection.Select(st => st.UserId).Distinct().ToList();
                return userIDList;
            }
            return null;
        }

        /// <summary>
        /// Get all sprints of one project 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<Sprint>GetAllSprintsByProject(int projectId)
        {
            return projectList[projectId].SprintCollection;
        }

        /// <summary>
        /// Get list of users in full name
        /// </summary>
        /// <returns></returns>
        public List<string>GetUserFullNameList()
        {
            return userList.Select(u => u.FirstName + ", " + u.LastName).ToList();
        }

        /// <summary>
        /// Get list of user story by user ID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>

        public List<UserStory>GetAllUserStoryByUserID(int userID, int projectID)
        {
            List<UserStory> storyList = new List<UserStory>();
            //get all userstory by projectID
            var allUserStoriesByProject = GetAllUserStoryByProject(projectID);
            //loop through each user story
            foreach (var us in allUserStoriesByProject)
            {
                //check if user has any subtask in each story
                var hasSubtask = (us.SubtaskCollection.Where(st => st.UserId == userID).ToList().Count > 0) ? true : false;
                if (hasSubtask)
                {
                    storyList.Add(us);
                }
            }
            return storyList;
        }
        /// <summary>
        /// Get user stories by Sprint ID
        /// </summary>
        /// <param name="spintID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public List<UserStory> GetAllUserStoryBySprintID(int spintID, int projectID)
        {
            //get all userstory by projectID
            var allUserStoriesByProject = GetAllUserStoryByProject(projectID);
            //loop through each user story 
            return allUserStoriesByProject.Where(us => us.SprintId == spintID).ToList();          
        }

        /// <summary>
        /// get all user stories by its status (Planned, Started, Delivered)
        /// </summary>
        /// <param name="status"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public List<UserStory>GetAllUserStoryByStatus(string status, int projectID)
        {
            // get all userstory by projectID
            var allUserStoriesByProject = GetAllUserStoryByProject(projectID);
            return allUserStoriesByProject.Where(us => us.Status == status).ToList();
        }

        /// <summary>
        /// Get total estimated time by user ID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public double GetTotalEstimatedTimeByUser(int userID,int projectID)
        {
            double totalTime = 0.0;
            //get all userstory by projectID
            var allUserStoriesByProject = GetAllUserStoryByProject(projectID);
            //loop through each user story
            foreach (var us in allUserStoriesByProject)
            {
                //check if user has any subtask in each story
                var hasSubtask = (us.SubtaskCollection.Where(st => st.UserId == userID).ToList().Count > 0) ? true : false;
                if (hasSubtask)
                {
                    totalTime += us.SubtaskCollection.Where(st => st.UserId == userID).Select(st => st.InitialEstimatedHours).Sum();
                }
            }
            return totalTime;
        }

        /// <summary>
        /// Get total actual time by user ID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public double GetTotalActualTimeByUser(int userID, int projectID)
        {
            double totalTime = 0.0;
            //get all userstory by projectID
            var allUserStoriesByProject = GetAllUserStoryByProject(projectID);
            //loop through each user story
            foreach (var us in allUserStoriesByProject)
            {
                //check if user has any subtask in each story
                var hasSubtask = (us.SubtaskCollection.Where(st => st.UserId == userID).ToList().Count > 0) ? true : false;
                if (hasSubtask)
                {
                    totalTime += us.SubtaskCollection.Where(st => st.UserId == userID).Select(st => st.ActualWorkHours).Sum();
                }
            }
            return totalTime;
        }

        /// <summary>
        /// Get total estimated time by user ID and Story Id
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public double GetTotalEstimatedTimeByUserIdAndStoryId(int userID, int storyId, int projectID)
        {
            double totalTime = 0.0;
            //get all userstory by projectID
            var allUserStoriesByProject = GetAllUserStoryByProject(projectID);

            //get selected userstory
            var selectedStory = allUserStoriesByProject.Where(us => us.Id == storyId).FirstOrDefault();
            
            //check if user has any subtask in selected story
            var hasSubtask = (selectedStory.SubtaskCollection.Where(st => st.UserId == userID).ToList().Count > 0) ? true : false;
            if (hasSubtask)
            {
                totalTime += selectedStory.SubtaskCollection.Where(st => st.UserId == userID).Select(st => st.InitialEstimatedHours).Sum();
            }
            
            return totalTime;
        }

        /// <summary>
        /// Get total actual time by user ID and storyId
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public double GetTotalActualTimeByUserIdAndStoryId(int userID, int storyID, int projectID)
        {
            double totalTime = 0.0;
            //get all userstory by projectID
            var allUserStoriesByProject = GetAllUserStoryByProject(projectID);

            //get selected userstory
            var selectedStory = allUserStoriesByProject.Where(us => us.Id == storyID).FirstOrDefault();

            //check if user has any subtask in selected story
            var hasSubtask = (selectedStory.SubtaskCollection.Where(st => st.UserId == userID).ToList().Count > 0) ? true : false;
            if (hasSubtask)
            {
                totalTime += selectedStory.SubtaskCollection.Where(st => st.UserId == userID).Select(st => st.ActualWorkHours).Sum();
            }

            return totalTime;
        }

        /// <summary>
        /// Get all subtasks by user story ID
        /// </summary>
        /// <param name="userStoryID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public List<Subtask> GetAllSubtasksByUserStory(int userStoryID, int projectID)
        {
            //get all userstory by projectID
            var allUserStoriesByProject = GetAllUserStoryByProject(projectID);

            return allUserStoriesByProject.Where(us => us.Id == userStoryID).First().SubtaskCollection.ToList();
        }

        /// <summary>
        /// get actual work time by user story and sub task
        /// </summary>
        /// <param name="storyID"></param>
        /// <param name="subTaskID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public double GetActualTimeBySubTaskAndUserStory(int storyID, int subTaskID, int projectID)
        {
            //get all userstory by projectID
            var allUserStoriesByProject = GetAllUserStoryByProject(projectID);

            //get selected userstory
            var selectedStory = allUserStoriesByProject.Where(us => us.Id == storyID).FirstOrDefault();

            return selectedStory.SubtaskCollection.Where(st => st.Id == subTaskID).FirstOrDefault().ActualWorkHours;

        }

        /// <summary>
        /// Get estimated time by subtask and user story
        /// </summary>
        /// <param name="storyID"></param>
        /// <param name="subTaskID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public double GetEstimatedTimeBySubTaskAndUserStory(int storyID, int subTaskID, int projectID)
        {
            //get all userstory by projectID
            var allUserStoriesByProject = GetAllUserStoryByProject(projectID);

            //get selected userstory
            var selectedStory = allUserStoriesByProject.Where(us => us.Id == storyID).FirstOrDefault();

            return selectedStory.SubtaskCollection.Where(st => st.Id == subTaskID).FirstOrDefault().InitialEstimatedHours;

        }

        /// <summary>
        /// Get total estimated time for a specific user story
        /// </summary>
        /// <param name="storyID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public double GetEstimatedTimeByUserStory(int storyID, int projectID)
        {
            //get all userstory by projectID
            var allUserStoriesByProject = GetAllUserStoryByProject(projectID);

            //get selected userstory
            var selectedStory = allUserStoriesByProject.Where(us => us.Id == storyID).FirstOrDefault();

            return selectedStory.SubtaskCollection.Select(st => st.InitialEstimatedHours).Sum();

        }

        /// <summary>
        /// Get total actual time for a specific user story
        /// </summary>
        /// <param name="storyID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public double GetActualTimeByUserStory(int storyID, int projectID)
        {
            //get all userstory by projectID
            var allUserStoriesByProject = GetAllUserStoryByProject(projectID);

            //get selected userstory
            var selectedStory = allUserStoriesByProject.Where(us => us.Id == storyID).FirstOrDefault();

            return selectedStory.SubtaskCollection.Select(st => st.ActualWorkHours).Sum();

        }

        /// <summary>
        /// Update velocity for current sprint and other sprints after that
        /// </summary>
        /// <param name="newVelocity"></param>
        /// <param name="sprintID"></param>
        /// <param name="projectID"></param>
        public void UpdateVelocityForSprint(double newVelocity, int sprintID, int projectID)
        {
            // get all Sprints by project ID
            var allSprints = projectList[projectID].SprintCollection;
            //update velocity of the selected and other prints onwards
            for(int i = 0; i < allSprints.Count; ++i)
            {
                if (allSprints[i].Id >= sprintID)
                    allSprints[i].Velocity = newVelocity;
            }
            ReloadData();// reload Data = write back new data to json and re-read data back to main data container.
        }

        /// <summary>
        /// update story status
        /// </summary>
        /// <param name="newStatus"></param>
        /// <param name="storyID"></param>
        /// <param name="projectID"></param>
        public void UpdateStotyStatus(string newStatus, int storyID, int projectID)
        {
            //get all userstory by projectID
            var allUserStoriesByProject = GetAllUserStoryByProject(projectID);

            //update new status
            allUserStoriesByProject.Where(us => us.Id == storyID).FirstOrDefault().Status = newStatus;

            ReloadData();
        }

        /// <summary>
        /// update estimated time by subtask
        /// </summary>
        /// <param name="newTime"></param>
        /// <param name="subTaskID"></param>
        /// <param name="StoryID"></param>
        /// <param name="projectID"></param>
        public void UpdateEstimatedTimeBySubTask(double newTime, int subTaskID, int StoryID, int projectID)
        {
            //get all userstory by projectID
            var allSubTasks = GetAllSubtasksByUserStory(StoryID, projectID);

            allSubTasks.Where(st => st.Id == subTaskID).FirstOrDefault().InitialEstimatedHours = newTime;

            ReloadData();

        }

        /// <summary>
        /// update actual time by subtask
        /// </summary>
        /// <param name="newTime"></param>
        /// <param name="subTaskID"></param>
        /// <param name="StoryID"></param>
        /// <param name="projectID"></param>
        public void UpdateActualTimeBySubTask(double newTime, int subTaskID, int StoryID, int projectID)
        {
            //get all userstory by projectID
            var allSubTasks = GetAllSubtasksByUserStory(StoryID, projectID);

            allSubTasks.Where(st => st.Id == subTaskID).FirstOrDefault().ActualWorkHours = newTime;

            ReloadData();

        }

        public void ReloadData()
        {
            //TODO:
        }
    }
}

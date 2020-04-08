using SprintRetrospectiveApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SprintRetrospectiveApp.Data_Load;

namespace SprintRetrospectiveApp.Data_Manipulation
{
    public class ProjectCRU
    {

        private List<Project> projectList = new List<Project>();
        private List<User> userList = new List<User>();

        // Alvin Version
        //private List<int> userProjectList1 = new List<int>();
        //private List<int> userProjectList2 = new List<int>();

        //public ProjectCRU()
        //{
        //    //Load all project into container projectList
        //    //TODO
        //    //Testing
        //    List<UserStory> userStoryColl = new List<UserStory>();
        //    List<Sprint> sprintColl = new List<Sprint>();

        //    //Add first project
        //    projectList.Add(new Project(0, "My First Project", "Project managing app", "JAN", "53", "https://app.clickup.com/1234698/d/b?p=-1", DateTime.UtcNow, 60.00, 2, userStoryColl, sprintColl));
        //    //Add two user stories, each user story has a different collection of subtask
        //    //First subtask collection
        //    List<Subtask> subTaskColl1 = new List<Subtask>();
        //    subTaskColl1.Add(new Subtask(0, 0, DateTime.Now, "Create a basic menu subtask 1", 2.0, 2.5));
        //    subTaskColl1.Add(new Subtask(1, 1, DateTime.Now, "Create a basic menu subtask 2", 3.0, 3.5));
        //    subTaskColl1.Add(new Subtask(2, 1, DateTime.Now, "Create a basic menu subtask 3", 4.0, 4.5));

        //    //Second subtask collection
        //    List<Subtask> subTaskColl2 = new List<Subtask>();
        //    subTaskColl2.Add(new Subtask(0, 0, DateTime.Now, "Create a basic homepage subtask 1", 2.0, 2.5));
        //    subTaskColl2.Add(new Subtask(1, 2, DateTime.Now, "Create a basic homepage subtask 2", 3.0, 3.5));
        //    subTaskColl2.Add(new Subtask(2, 2, DateTime.Now, "Create a basic homepage subtask 3", 4.0, 4.5));

        //    UserStory us1 = new UserStory(0, 0, "Provide a basic menu system for the application", 5.5, 4.0, DateTime.Now, "Planned", subTaskColl1);
        //    UserStory us2 = new UserStory(1, 1, "Provide a basic homepage for the application", 6.5, 7.0, DateTime.Now, "Delivered", subTaskColl2);
        //    userStoryColl.Add(us1);
        //    userStoryColl.Add(us2);

        //    //print collection
        //    sprintColl.Add(new Sprint(0, 20));
        //    sprintColl.Add(new Sprint(1, 20));
        //    sprintColl.Add(new Sprint(2, 20));


        //    projectList.Add(new Project(1, "My Second Project", "Project managing app", "JAN", "53", "https://app.clickup.com/1234698/d/b?p=-1", DateTime.UtcNow, 60.00, 2, userStoryColl, sprintColl));

        //    userProjectList1.Add(0);
        //    //Add users to the List
        //    userList.Add(new User(0, "Alvin", "Nguyen", "Developer", userProjectList1));
        //    userList.Add(new User(1, "Jason", "Oh", "Developer", userProjectList1));



        //    userProjectList2.Add(0);
        //    userProjectList2.Add(1);

        //    userList.Add(new User(2, "Nic", "Prezio", "Developer", userProjectList2));
        //    userList.Add(new User(3, "Brian", "Turford", "ProjectManager", userProjectList2));



        public ProjectCRU()
        {
            projectList = ReadWrite_Two_Objects.ReadDataFile("projects").projects;
            userList = ReadWrite_Two_Objects.ReadDataFile("users").users;
        }
       
   

        /// <summary>
        /// Get a collection of projects
        /// </summary>
        /// <returns></returns>
        public List<Project> GetAllProjects()
        {
            return projectList;
        }

        /// <summary>
        /// Get a collection of users
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            return userList;
        }


        public User GetUserById(int userId)
        {
            var user = userList.Where(u => u.Id == userId).Distinct().FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Get list of Projects of each user being involved in the project
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Project> GetAllProjectsByUser(int userId)
        {

            //look for distinct userID in user list
            User user = userList.Where(u => u.Id == userId).FirstOrDefault();

            List<Project> userProjectList = new List<Project>();

            foreach (var projectId in user.ProjectIdCollection)
            {
                var pr = projectList.Where(p => p.Id == projectId).FirstOrDefault();
                userProjectList.Add(pr);
            }

            return userProjectList;

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
        public List<int> GetAllUserIdsByProject(int projectId)
        
        {
            var allUserStories = GetAllUserStoryByProject(projectId);
            return allUserStories.Select(st => st.UserId).Distinct().ToList();
            
        }

        /// <summary>
        /// Get list of UserID of all users being involved in the project (means having subtasks in that project)
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<User> GetAllUsersByProject(int projectId)
        {
            var userIdsByProject = GetAllUserIdsByProject(projectId);

            List<User> userListBytProject = new List<User>();

            foreach (var userId in userIdsByProject)
            {
                var pr = userList.Where(u => u.Id == userId).FirstOrDefault();
                userListBytProject.Add(pr);
            }

            return userListBytProject;
        }


        /// <summary>
        /// Get all users in a sprint
        /// </summary>
        /// <param name="sprintID"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<int> GetAllUserIdsBySprint(int sprintID, int projectId)
        {
            var allUserStories = GetAllUserStoryBySprintID(sprintID, projectId);
            return allUserStories.Select(st => st.UserId).Distinct().ToList();
        }



        /// <summary>
        /// Get all sprints of one project 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<Sprint> GetAllSprintsByProject(int projectId)
        {
            return projectList[projectId].SprintCollection;
        }

        /// <summary>
        /// Get list of users in full name
        /// </summary>
        /// <returns></returns>
        public List<string> GetUserFullNameList()
        {
            return userList.Select(u => u.FirstName + ", " + u.LastName).ToList();
        }

        /// <summary>
        /// Get list of users in full name
        /// </summary>
        /// <returns></returns>
        public List<string> GetUserFullNameListByProject(int ProjectID)
        {
            var userIdsByProject = GetAllUserIdsByProject(ProjectID);

            List<string> userFullNameListBytProject = new List<string>();

            foreach (var userId in userIdsByProject)
            {
                var pr = userList.Where(u => u.Id == userId).FirstOrDefault();
                userFullNameListBytProject.Add(pr.FirstName + " " + pr.LastName);
            }

            return userFullNameListBytProject;
        }

        public User GetUserByUserStory(int userStoryID, int projectId)
        {

            var userStories = GetAllUserStoryByProject(projectId);

            var userId = userStories.Where(us => us.Id == userStoryID).Select(u => u.UserId).FirstOrDefault();

            var user = userList.Where(u => u.Id == userId).FirstOrDefault();

            return user;

        }



        /// <summary>
        /// Get list of user story by user ID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>

        public List<UserStory> GetAllUserStoryByUserID(int userID, int projectID)
        {
            //List<UserStory> storyList = new List<UserStory>();
            //get all userstory by projectID
            var allUserStoriesByProject = GetAllUserStoryByProject(projectID);

            return allUserStoriesByProject.Where(st => st.UserId == userID).ToList();

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
        public List<UserStory> GetAllUserStoryByStatus(string status, int projectID)
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
        public double GetTotalEstimatedTimeByUser(int userID, int projectID)
        {
            double totalTime = 0.0;
            //get all userstory by projectID and userID
            var allUserStoriesByUser = GetAllUserStoryByUserID(userID, projectID);
            //loop through each user story
            foreach (var us in allUserStoriesByUser)
            {
                //check if user has any subtask in each story
                var hasSubtask = (us.SubtaskCollection.Count > 0) ? true : false;
                if (hasSubtask)
                {
                    totalTime += us.SubtaskCollection.Select(st => st.InitialEstimatedHours).Sum();
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
            //get all userstory by projectID and userID
            var allUserStoriesByUser = GetAllUserStoryByUserID(userID, projectID);
            //loop through each user story
            foreach (var us in allUserStoriesByUser)
            {
                //check if user has any subtask in each story
                var hasSubtask = (us.SubtaskCollection.Count > 0) ? true : false;
                if (hasSubtask)
                {
                    totalTime += us.SubtaskCollection.Select(st => st.ActualWorkHours).Sum();
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
            var hasSubtask = (selectedStory.SubtaskCollection.Count > 0) ? true : false;
            if (hasSubtask)
            {
                totalTime += selectedStory.SubtaskCollection.Select(st => st.InitialEstimatedHours).Sum();
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
            var hasSubtask = (selectedStory.SubtaskCollection.Count > 0) ? true : false;
            if (hasSubtask)
            {
                totalTime += selectedStory.SubtaskCollection.Select(st => st.ActualWorkHours).Sum();
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
            for (int i = 0; i < allSprints.Count; ++i)
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
        public void UpdateStoryStatus(string newStatus, int storyID, int projectID)
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

        /// <summary>
        /// return accuracy score by user ID and story ID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="storyID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public double GetScoreByUserIdAndStoryId(int userID, int storyID, int projectID)
        {
            double totalActualTime = GetTotalActualTimeByUserIdAndStoryId(userID, storyID, projectID);
            double totalEstimatedTime = GetTotalEstimatedTimeByUserIdAndStoryId(userID, storyID, projectID);
            //formular (actual - estimated)/actual * 100 -- if negative result means less than % the estimated time, or positive means more than % the estimated time
            return (totalActualTime - totalEstimatedTime) / totalActualTime * 100;
        }

        /// <summary>
        /// get accuracy score by userID (for all userstories in one project)
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="projectID"></param>
        public double GetScoreByUserId(int userID, int projectID)
        {
            double totalActualTime = GetTotalActualTimeByUser(userID, projectID);
            double totalEstimatedTime = GetTotalEstimatedTimeByUser(userID, projectID);
            //formular (actual - estimated)/actual * 100 -- if negative result means less than % the estimated time, or positive means more than % the estimated time
            return (totalActualTime - totalEstimatedTime) / totalActualTime * 100;
        }
        /// <summary>
        /// this method will calculate the estimated accuracy of project velocity (based on current/selected sprint)
        /// </summary>
        /// <param name="projectID"></param>
        public double GetEstimatedAccuracy(int projectID, int sprintID)
        {
            //get actual velocity
            double actualVelocity = GetActualVelocity(projectID, sprintID);
            //get initial velocity
            double initialVelocity = projectList[projectID].InitialVelocity;
            return (actualVelocity - initialVelocity) / actualVelocity * 100;
        }

        /// <summary>
        /// return total actual time per sprint
        /// </summary>
        /// <param name="sprintID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public double GetTotalActualTimeBySprint(int sprintID, int projectID)
        {
            double totalTime = 0.0;
            //get all userstory by sprintID
            var allUserStoriesBySprint = GetAllUserStoryBySprintID(sprintID, projectID);
            //loop through each user story
            foreach (var us in allUserStoriesBySprint)
            {
                totalTime += us.SubtaskCollection.Select(st => st.ActualWorkHours).Sum();
            }
            return totalTime;
        }
        /// <summary>
        /// Get actual velocity based on current project and selected sprint
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="sprintID"></param>
        /// <returns></returns>
        public double GetActualVelocity(int projectID, int sprintID)
        {
            //get team size per sprint
            int teamSize = GetAllUserIdsBySprint(sprintID, projectID).Count;

            //get hours per story point
            double hoursPerStoryPoint = projectList[projectID].HoursPerStoryPoint;
            //get total actual time for current (selected sprint)
            double totalActualTimeForSelectedSprint = GetTotalActualTimeBySprint(sprintID, projectID);
            double actualHoursPerPersonPerSprint = totalActualTimeForSelectedSprint / teamSize;
            //convert to point
            double actualPointsPerPersonPerSprint = actualHoursPerPersonPerSprint / hoursPerStoryPoint;

            return actualPointsPerPersonPerSprint * teamSize;
        }
        /// <summary>
        /// Add new project to project list
        /// </summary>
        /// <param name="ProjectName"></param>
        /// <param name="Description"></param>
        /// <param name="TeamName"></param>
        /// <param name="TeamNumber"></param>
        /// <param name="HyperlinkClickup"></param>
        /// <param name="StartDate"></param>
        /// <param name="InitialVelocity"></param>
        /// <param name="HoursPerStoryPoint"></param>
        /// <param name="UserStoryCollection"></param>
        /// <param name="SprintCollection"></param>
        public void AddProject(string ProjectName, string Description, string TeamName, string TeamNumber, string HyperlinkClickup, DateTime StartDate, double InitialVelocity, double HoursPerStoryPoint, List<UserStory> UserStoryCollection, List<Sprint> SprintCollection)
        {
            //if the newly added project is the first one, then set ID to 1, or else set to ID of the previous project in the list plus 1
            int projectID = (projectList.Count > 0) ? projectList[projectList.Count - 1].Id + 1 : 1;
            //add project to the list 
            projectList.Add(new Project(projectID, ProjectName, Description, TeamName, TeamNumber, HyperlinkClickup, StartDate, InitialVelocity, HoursPerStoryPoint, new List<UserStory>(), new List<Sprint>()));
        }
        /// <summary>
        /// This method used to assign user to a story
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="storyID"></param>
        /// <param name="projectID"></param>
        public void AddMemberToUserStory(int userID, int storyID, int projectID)
        {
            //get all user stories by project
            var allStories = GetAllUserStoryByProject(projectID);
            foreach (var us in allStories)
            {
                //look for a specific user story
                if (us.Id == storyID)
                {
                    //then assign userID 
                    us.UserId = userID;
                }
            }

        }

        public void UpdateEstimatedTimeByUserStory(int storyID, int projectID, double newEstimatedTime)
        {
            var allStories = GetAllUserStoryByProject(projectID);
            allStories.Where(story => story.Id == storyID).FirstOrDefault().InitialEstimatedHours = newEstimatedTime;
            ReloadData();
        }

        public void UpdateStorySprint(int storyID, int projectID, int sprintID)
        {
            var allStories = GetAllUserStoryByProject(projectID);
            allStories.Where(story => story.Id == storyID).FirstOrDefault().SprintId = sprintID;
            ReloadData();
        }

        public void AddTaskToUserStory(int storyID, int projectID, int UserId, DateTime LastUpdatedTime, string Description, double InitialEstimatedHours, double ActualWorkHours)
        {
            //get all user stories by project
            var allStories = GetAllUserStoryByProject(projectID);
            foreach (var us in allStories)
            {
                //look for a specific user story
                if (us.Id == storyID)
                {
                    var subTaskId = new Random().Next(1000);
                    us.SubtaskCollection.Add(new Subtask(subTaskId, LastUpdatedTime, Description, InitialEstimatedHours, ActualWorkHours));
                }
            }
        }

        public void ReloadData()
        {
            //TODO:
        }


    }
}

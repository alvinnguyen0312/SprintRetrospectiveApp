using SprintRetrospectiveApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace SprintRetrospectiveApp.Data_Load
{
    public class ReadWrite_Two_Objects
    {
        public static (List<Project> projects, List<User> users) ReadDataFile(string token)
        {
            Project projectObject = null;
            List<UserStory> storyList = null;
            List<Sprint> sprintList = null;
            List<Subtask> subtaskList = null;
            List<User> userList = null;
            List<Project> projectList = null;
            List<int> projectIdList = null;

            try
            {
                //read the file
                string path = Path.GetDirectoryName(Directory.GetCurrentDirectory());
                string file = File.ReadAllText(path + "\\data.json");
                dynamic jsonResults = JsonConvert.DeserializeObject(file);

                if (token == "projects")
                {
                    projectList = new List<Project>();
                    //loop through the objects in the results 
                    //add their values to a respective object
                    foreach (var item in jsonResults.Projects)
                    {
                        //once we're inside the project we can make a new project
                        //object using the information from that item
                        //first step is converting the user story array, subtask, users and sprint
                        //arrays into lists
                        storyList = new List<UserStory>();
                        sprintList = new List<Sprint>();

                        foreach (var userStoryItem in item.UserStoryCollection)
                        {
                            subtaskList = new List<Subtask>();

                            foreach (var subtaskItem in userStoryItem.SubtaskCollection)
                            {
                                subtaskList.Add(new Subtask((int)subtaskItem.Id,
                                                            (DateTime)subtaskItem.LastUpdatedTime,
                                                            (string)subtaskItem.Description,
                                                            (double)subtaskItem.ActualWorkHours));
                            }

                            storyList.Add(new UserStory((int)userStoryItem.Id,
                                                        (int)userStoryItem.UserId,
                                                        (int)userStoryItem.SprintId,
                                                        (int)userStoryItem.StoryPoint,
                                                        (string)userStoryItem.Description,
                                                        (double)userStoryItem.InitialEstimatedHours,
                                                        (double)userStoryItem.ActualWorkHours,
                                                        (DateTime)userStoryItem.LastUpdatedTime,
                                                        (string)userStoryItem.Status,
                                                        (List<Subtask>)subtaskList));
                        }
                        foreach (var sprintItem in item.SprintCollection)
                        {
                            sprintList.Add(new Sprint((int)sprintItem.Id,
                                                      (double)sprintItem.Velocity));
                        }

                        //then we need to create a new project object using that information
                        //and add it to the project list
                        projectObject = new Project(
                            (int)item.Id,
                            (string)item.ProjectName,
                            (string)item.TeamName,
                            (string)item.TeamNumber,
                            (string)item.HyperlinkClickup,
                            (DateTime)item.StartDate,
                            (double)item.InitialVelocity,
                            (double)item.HoursPerStoryPoint,
                            (List<UserStory>)storyList,
                            (List<Sprint>)sprintList);

                        projectList.Add(projectObject);

                    } // end project
                }
                else
                {
                    userList = new List<User>();

                    foreach (var item in jsonResults.Users)
                    {

                        projectIdList = new List<int>();

                        foreach (var projectId in item.ProjectIdCollection)
                        {
                            projectIdList.Add(Convert.ToInt32(projectId));
                        }

                        userList.Add(new User((int)item.Id,
                                                  (string)item.FirstName,
                                                  (string)item.LastName,
                                                  (string)item.Role,
                                                  (List<int>)projectIdList));

                    }
                }




            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was a problem in the JSON reader: {ex.Message}");
            }

            //return the list
            return (projectList, userList);
        }

        public static void WriteToDataFile(List<Project> projects, List<User> users)
        {
            string json = "";
            try
            {
                //for the json strings add a property string to the 
                //front so that you can access it in the read method
                json += "{ \"Projects\": " + JsonConvert.SerializeObject(projects.ToArray());
                json += ", \"Users\": " + JsonConvert.SerializeObject(users.ToArray()) + " }";

                //this is a temporary path for testing
                //uncomment the last two lines 
                //when you are ready to immplement it (this will overwrite the data.json file)
                //File.WriteAllText(@"c:\temp\path.json", json);
                string path = Path.GetDirectoryName(Directory.GetCurrentDirectory());
                File.WriteAllText(path + "\\data.json", json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was a problem in JSON writer: {ex.Message}");
            }

        }
    }
}
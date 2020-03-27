using SprintRetrospectiveApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace SprintRetrospectiveApp.Data_Load
{
    class ReadWrite
    {
        public static List<Project> ReadDataFile()
        {
            Project projectObject = null;
            List<UserStory> storyList = null;
            List<Sprint> sprintList = null;
            List<Subtask> subtaskList = null;
            List<User> userList = null;
            List<Project> projectList = null;

            try
            {
                projectList = new List<Project>();

                //read the file
                string path = Path.GetDirectoryName(Directory.GetCurrentDirectory());
                string file = File.ReadAllText(path + "\\data.json");
                dynamic jsonResults = JsonConvert.DeserializeObject(file);

                //loop through the objects in the results 
                //add their values to a respective object
                foreach (var obj in jsonResults)
                {
                    //this will get into the json object now we
                    //need to go into each project in the initial object
                    foreach(var project in obj)
                    {
                        foreach(var item in project)
                        {
                            //once we're inside the project we can make a new project
                            //object using the information from that item
                            //first step is converting the user story array, subtask, users and sprint
                            //arrays into lists
                            storyList = new List<UserStory>();
                            sprintList = new List<Sprint>();
                            subtaskList = new List<Subtask>();
                            userList = new List<User>();
                            foreach(var userStoryItem in item.user_stories)
                            {
                                foreach(var subtaskItem in userStoryItem.subtasks)
                                {
                                    subtaskList.Add(new Subtask((int)subtaskItem.subtask_id,
                                                                (int)subtaskItem.user_id,
                                                                (DateTime)subtaskItem.last_updated,
                                                                (string)subtaskItem.description,
                                                                (double)subtaskItem.initial_estimated_hours,
                                                                (double)subtaskItem.actual_hours_worked));
                                }

                                storyList.Add(new UserStory((int)userStoryItem.story_id,
                                                            (int)userStoryItem.sprint_id,
                                                            (string)userStoryItem.description,
                                                            (double)userStoryItem.initial_estimated_hours,
                                                            (double)userStoryItem.actual_hours_worked,
                                                            (DateTime)userStoryItem.last_updated,
                                                            (string)userStoryItem.status, 
                                                            (List<Subtask>)subtaskList));
                            }
                            foreach(var sprintItem in item.sprints)
                            {
                                sprintList.Add(new Sprint((int)sprintItem.sprint_id, 
                                                          (double)sprintItem.velocity));
                            }
                            foreach(var userItem in item.users)
                            {
                                userList.Add(new User((int)userItem.user_id,
                                                      (string)userItem.first_name,
                                                      (string)userItem.last_name,
                                                      (string)userItem.role));
                            }

                            //then we need to create a new project object using that information
                            //and add it to the project list
                            projectObject = new Project(
                                (int)item.project_id,
                                (string)item.project_name,
                                (string)item.description,
                                (string)item.team_name,
                                (string)item.team_number,
                                (string)item.click_up_link,
                                (DateTime)item.start_date,
                                (double)item.initial_volicity,
                                (double)item.hours_per_storypoint,
                                (List<UserStory>)storyList,
                                (List<Sprint>)sprintList,
                                (List<User>)userList);

                            projectList.Add(projectObject);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was a problem in the JSON reader: {ex.Message}");
            }

            //return the list
            return projectList;
        }
    }
}

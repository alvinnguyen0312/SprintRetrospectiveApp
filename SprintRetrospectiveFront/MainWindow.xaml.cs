using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using SprintRetrospectiveApp.Data_Manipulation;
using SprintRetrospectiveApp.Models;

namespace SprintRetrospectiveFront
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProjectCRU project;
        private List<Project> loggedInUserProjects;
        private Project currentProject;
        private List<User> allUsers;
        private List<User> teamMembersInScore;
        private List<string> userFullNameListBytProject;
        private List<UserStory> userStoriesInScore;
        private List<UserStory> userStoriesByProject;
        private List<Sprint> allSprintsByProject;
        private List<Subtask> allSubtasksByUersStory;
        private User loggedInUser;
        private int currentProjectId;
        private int loggedInUserId;
        private int selectedTeamMemberIdInScore;
        private int selectedUserStoryIdInScore;
        private int selectedSprintInVelocityUpdate;
        private int selectedUserStoryIdInUserStory;

        private List<string> comboBoxCategories = new List<string>() { "Sprints", "Users", "Status" };
        private string currentSelectedCategory;
        private List<string> comboBoxSubCategories;
        private List<string> comboBoxSubCategoriesStatus = new List<string>() { "All", "Planned", "Started", "Delivered" };
        private List<string> comboBoxUserStoryStatus = new List<string>() { "All", "Planned", "Started", "Delivered" };



        public MainWindow()
        {
            InitializeComponent();
            project = new ProjectCRU();
            LoadUsers();
        }

        private void LoadUsers()
        {
            // Get the list from the server
            allUsers = project.GetAllUsers();

            ComboBoxUser.ItemsSource = allUsers;
            ComboBoxMember.ItemsSource = allUsers;
        }

        // User Login DropList
        private void ComboBoxUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loggedInUserId = Convert.ToInt32(ComboBoxUser.SelectedValue);

            //projectIdList = userList.Where(u => u.Id == currentUserId).SelectMany(u => u.ProjectIdCollection).ToList();

            loggedInUserProjects = project.GetAllProjectsByUser(loggedInUserId);

            loggedInUser = project.GetUserById(loggedInUserId);
            //userProjectList = new List<Project>();

            //foreach (var projectId in projectIdList)
            //{
            //    var pr = allProjectList.Where(p => p.Id == projectId).FirstOrDefault();
            //    userProjectList.Add(pr);
            //}

            // Biding User to the Project DropList
            ComboBoxProject.ItemsSource = loggedInUserProjects;

            // Make the velocity update button enable based on the user role
            if (loggedInUser.Role != "ProjectManager")
            {
                ButtonUpdateVelocity.IsEnabled = false;
                txtNewVelocity.IsReadOnly = true;
                ComboBoxSprint.IsEnabled = false;
            }
        }

        // Current Project DropList
        private void ComboBoxProject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            currentProjectId = Convert.ToInt32(ComboBoxProject.SelectedValue);

            //// ... Set SelectedItem as Window Title.
            //string value = comboBox.SelectedItem as string;
            //Console.WriteLine("Selected: " + value);
            teamMembersInScore = project.GetAllUsersByProject(currentProjectId);
            //currentProject = projectList.Where(p => p.ProjectName == value).FirstOrDefault();

            // Binding all team members to the Team Member DropList in the Score Section
            ComboBoxTeamMember.ItemsSource = teamMembersInScore;

            //txtEstimatedAccuracy.Text = project.Get

            // Save to the sprints list in the list
            allSprintsByProject = project.GetAllSprintsByProject(currentProjectId);
            userStoriesByProject = project.GetAllUserStoryByProject(currentProjectId);

            // Binding the sprint list to the Sprint DropList in the Manager Section
            ComboBoxSprint.ItemsSource = allSprintsByProject;

            // Binding all user stories to the All User Stories List in the All User Stories Section
            UserStoriesList.ItemsSource = userStoriesByProject;

            // Binding all user storis to the User Stories DropList in the UserStory Section
            ComboBoxSelectedUserStory.ItemsSource = userStoriesByProject;

            ComboBoxAssignedMember.ItemsSource = teamMembersInScore;

            // Make the velocity update button enable based on the user role
            if (loggedInUser.Role == "ProjectManager")
            {            
                ButtonUpdateVelocity.IsEnabled = true;
                txtNewVelocity.IsReadOnly = false;
                ComboBoxSprint.IsEnabled = true;
            }
           

        }

        #region dashboard_score_section
        private void ComboBoxTeamMember_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTeamMemberIdInScore = Convert.ToInt32(ComboBoxTeamMember.SelectedValue);    

  
            userStoriesInScore = project.GetAllUserStoryByUserID(selectedTeamMemberIdInScore, currentProjectId);

            userStoriesInScore.Add(new UserStory(-1, -1, -1, "All User Stories", 0, 0, DateTime.Now, "", new List<Subtask>()));

            ComboBoxUserStory.ItemsSource = userStoriesInScore;

            // Reset
            txtTotalEstimatedTime.Text = "";
            txtTotalActualTime.Text = "";
            txtScore.Text = "";
        }

        private void ComboBoxUserStory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUserStoryIdInScore = Convert.ToInt32(ComboBoxUserStory.SelectedValue);

            if(selectedUserStoryIdInScore == -1)
            {
                txtTotalEstimatedTime.Text = project.GetTotalEstimatedTimeByUser(selectedTeamMemberIdInScore, currentProjectId).ToString() + " hour(s)";
                txtTotalActualTime.Text = project.GetTotalActualTimeByUser(selectedTeamMemberIdInScore, currentProjectId).ToString() + " hour(s)";
                txtScore.Text = project.GetScoreByUserId(selectedTeamMemberIdInScore, currentProjectId).ToString();
            }
            else
            {
                txtTotalEstimatedTime.Text = project.GetTotalEstimatedTimeByUserIdAndStoryId(selectedTeamMemberIdInScore, selectedUserStoryIdInScore, currentProjectId).ToString() + " hour(s)";
                txtTotalActualTime.Text = project.GetTotalActualTimeByUserIdAndStoryId(selectedTeamMemberIdInScore, selectedUserStoryIdInScore, currentProjectId).ToString() + " hour(s)";
                txtScore.Text = project.GetScoreByUserIdAndStoryId(selectedTeamMemberIdInScore, selectedUserStoryIdInScore, currentProjectId).ToString();
            }  
           

        }
        #endregion


        #region sashBoard_project_velocities_update_section
        private void ComboBoxSprint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSprintInVelocityUpdate = Convert.ToInt32(ComboBoxSprint.SelectedValue);
        }

        private void ButtonUpdateVelocity_Click(object sender, RoutedEventArgs e)
        {
            if(txtNewVelocity.Text.Length != 0)
            {
                double newVelocity = Convert.ToDouble(txtNewVelocity.Text);
                project.UpdateVelocityForSprint(newVelocity, selectedSprintInVelocityUpdate, currentProjectId);
            }
            else
            {
                MessageBox.Show("Please insert new Velocity");
            }
            
        }
        #endregion


        #region dashboard_all_user_stories_section
        private void ComboBoxCategory_Loaded(object sender, RoutedEventArgs e)
        {
         
            // Get the ComboBox reference
            var comboBox = sender as ComboBox;

            // Assign the ItemSource to the List
            comboBox.ItemsSource = comboBoxCategories;

            // Make the first item selected
            //comboBox.SelectedIndex = 0;

        }

        // Category DropList
        private void ComboBoxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Reset
            currentSelectedCategory = "";
            ComboBoxSubCategory.ItemsSource = null;

            var comboBox = sender as ComboBox;
            var selectedComboBoxItem = comboBox.SelectedItem as string;
            currentSelectedCategory = selectedComboBoxItem;

            switch (selectedComboBoxItem)
            {

                case "Sprints":

                    allSprintsByProject = project.GetAllSprintsByProject(currentProjectId);
                    //MessageBox.Show("Selected Sprint");
                    ComboBoxSubCategory.ItemsSource = allSprintsByProject;
                   
                    break;

                case "Users":

                    //MessageBox.Show("Selected User");

                    userFullNameListBytProject = new List<string>();

                    foreach (var teamMember in teamMembersInScore)
                    {      
                        userFullNameListBytProject.Add(teamMember.FirstName + " " + teamMember.LastName);
                    }
                    
                    ComboBoxSubCategory.ItemsSource = userFullNameListBytProject;
                    break;
                case "Status":
                    //MessageBox.Show("Selected Status");
                    ComboBoxSubCategory.ItemsSource = comboBoxSubCategoriesStatus;
                    
                    break;
                default:
                    break;

                
            }
        }

        private void ComboBoxSubCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            var comboBox = sender as ComboBox;  
            
            switch(currentSelectedCategory)
            {
                case "Sprints":
                    var sprintSubItem = comboBox.SelectedItem; 

                    var selectedSprint = (Sprint)allSprintsByProject.Where(p => p == sprintSubItem).FirstOrDefault();
                    
                    var selectedUserStoriesBySprint = project.GetAllUserStoryBySprintID(selectedSprint.Id, currentProjectId);                 

                    UserStoriesList.ItemsSource = selectedUserStoriesBySprint;
                    break;
                case "Users":
                    var usersSubItem = comboBox.SelectedItem as string;
                    int selectedUserIdx = 0;

                    for(var i = 0; i < userFullNameListBytProject.Count; i++)
                    {
                        if (usersSubItem == userFullNameListBytProject[i])
                            selectedUserIdx = i;
                    }

                    var selectedUserStoriesByUsers = project.GetAllUserStoryByUserID(selectedUserIdx, currentProjectId);
                    UserStoriesList.ItemsSource = selectedUserStoriesByUsers;
                    break;
                case "Status":
                    var statusSubItem = comboBox.SelectedItem as string;

                    switch(statusSubItem)
                    {
                      
                        case "All":
                            var selectedUserStoriesByAll = project.GetAllUserStoryByProject(currentProjectId);
                            UserStoriesList.ItemsSource = selectedUserStoriesByAll;
                            break;
                        case "Planned":
                            var selectedUserStoriesByPlanned = project.GetAllUserStoryByStatus("Planned", currentProjectId);
                            UserStoriesList.ItemsSource = selectedUserStoriesByPlanned;
                            break;
                        case "Started":
                            var selectedUserStoriesByStarted = project.GetAllUserStoryByStatus("Started", currentProjectId);
                            UserStoriesList.ItemsSource = selectedUserStoriesByStarted;
                            break;
                        case "Delivered":
                            var selectedUserStoriesByDelivered = project.GetAllUserStoryByStatus("Delivered", currentProjectId);
                            UserStoriesList.ItemsSource = selectedUserStoriesByDelivered;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            
        }
        #endregion


        #region user_story_section
        private void ComboBoxSelectedUserStory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Reset
            selectedUserStoryIdInUserStory = 0;
            allSubtasksByUersStory = null;

            var selectedUserStoryId = Convert.ToInt32(ComboBoxSelectedUserStory.SelectedValue);

            selectedUserStoryIdInUserStory = selectedUserStoryId;

            var selectedUserStory = userStoriesByProject[selectedUserStoryId];

            var user = project.GetUserByUserStory(selectedUserStoryId, currentProjectId);

            
            txtCurrentUserStoryAssignedMember.Text = user.FirstName + " " + user.LastName;
            txtCurrentUserStorySprint.Text = selectedUserStory.SprintId.ToString();
            txtCurrentUserStoryStatus.Text = selectedUserStory.Status.ToString();

            ComboBoxUpdateSprint.ItemsSource = allSprintsByProject;
            ComboBoxUpdateStatus.ItemsSource = comboBoxUserStoryStatus;

            // How to use story point


            txtStoryEstimatedTime.Text = selectedUserStory.InitialEstimatedHours.ToString();
            txtStoryActualTime.Text = selectedUserStory.ActualWorkHours.ToString();


            // Sub tasks
            allSubtasksByUersStory = project.GetAllSubtasksByUserStory(selectedUserStoryId, currentProjectId);
            ComboBoxCurrentTaskList.ItemsSource = project.GetAllSubtasksByUserStory(selectedUserStoryId, currentProjectId);
            


            //txtCurrentAssignedMember.Text = project.GetUserFullNameListByProject
            //txtTotalEstimatedTime.Text = project.GetTotalEstimatedTimeByUserIdAndStoryId(selectedTeamMemberIdInScore, selectedUserStoryIdInScore, currentProjectId).ToString() + " hour(s)";
            //txtTotalActualTime.Text = project.GetTotalActualTimeByUserIdAndStoryId(selectedTeamMemberIdInScore, selectedUserStoryIdInScore, currentProjectId).ToString() + " hour(s)";
            //txtScore.Text = project.GetScoreByUserIdAndStoryId(selectedTeamMemberIdInScore, selectedUserStoryIdInScore, currentProjectId).ToString();

        }

        private void ComboBoxCurrentTaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSubTaskId = Convert.ToInt32(ComboBoxCurrentTaskList.SelectedValue);

            var selectedSubTask = allSubtasksByUersStory[selectedSubTaskId];

            txtTaskActualTime.Text = selectedSubTask.ActualWorkHours.ToString();

        }

        #endregion


        //private void LoadProjects()
        //{
        //    // Get the list from the server
        //    projectList = project.GetAllProjects();

        //    // Assign the list to the ComboBox
        //    ComboBoxProject.ItemsSource = projectList;

        //}


        //private void ComboBoxProject_Loaded(object sender, RoutedEventArgs e)
        //{
        //    // https://www.dotnetperls.com/combobox-wpf

        //    // Get the list from the server
        //    projectList = project.GetAllProjects();

        //    // Get the ComboBox reference.
        //    var comboBox = sender as ComboBox;

        //    // Assign the ItemsSource to the List.
        //    var list = projectList.Select(p => p.ProjectName).ToList();

        //    comboBox.ItemsSource = list;

        //    //// Make the first item selected.
        //    //comboBox.SelectedIndex = 0;

        //}






        /// <summary>
        /// Terminate the application
        /// </summary>
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       
    }// end MainWindow
}
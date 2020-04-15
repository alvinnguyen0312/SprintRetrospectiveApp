using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
        private List<User> newTeamMembersByProject;
        private List<string> userFullNameListBytProject;
        private List<UserStory> userStoriesInScore;
        private List<UserStory> userStoriesByProject;
        private List<UserStory> newUserStoriesByProject;
        private List<Sprint> allSprintsByProject;
        private List<Subtask> allSubtasksByUersStory;
        private User loggedInUser;
        private User addingMemberToProject;
        private int currentProjectId;
        private int loggedInUserId;
        private int selectedTeamMemberIdInScore;
        private int selectedUserStoryIdInScore;
        private int selectedSprintInVelocityUpdate;
        private int selectedUserStoryIdInUserStory;
        private int selectedSubTaskIdInUserStory;
        private int selectedUpdateSprintInUserStory;
        private string selectedUpdateStatusInUserStory;
        private int selectedAssigningUserInUserStory;

        private List<string> comboBoxCategories = new List<string>() { "Sprints", "Users", "Status" };
        private string currentSelectedCategory;
        private List<string> comboBoxSubCategoriesStatus = new List<string>() { "All", "Planned", "Started", "Delivered" };
        private List<string> comboBoxUserStoryStatus = new List<string>() { "Planned", "Started", "Delivered" };

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
            ResetUIByUser();

            loggedInUserId = Convert.ToInt32(ComboBoxUser.SelectedValue);

            // Get the user object based on the userId
            loggedInUser = project.GetUserById(loggedInUserId);

            // Make the velocity update button enable based on the user role
            if (loggedInUser.Role == "ProjectManager")
            {
                // Set the project list based on the role
                loggedInUserProjects = project.GetAllProjectsByUser(loggedInUserId);
                loggedInUserProjects.Add(new Project(-1, "Create a new Project",  "", "", "", DateTime.Now, 0, 0, new List<UserStory>(), new List<Sprint>()));
            }
            else
            {
                loggedInUserProjects = project.GetAllProjectsByUser(loggedInUserId);
       
            }

            // Biding User to the Project DropList
            ComboBoxProject.ItemsSource = new List<Project>();
            ComboBoxProject.ItemsSource = loggedInUserProjects;     
        }

        private void ResetUIByUser()
        {

            // Reset UI [ScoreSection]
            ComboBoxTeamMember.IsEnabled = false;
            ComboBoxUserStory.IsEnabled = false;
            txtTotalEstimatedTime.IsReadOnly = true;
            txtTotalEstimatedTime.Text = "";
            txtTotalActualTime.IsReadOnly = true;
            txtTotalActualTime.Text = "";
            txtScore.IsReadOnly = true;
            txtScore.Text = "";

            // Reset UI [ProjectVelocityUpdate]
            ButtonUpdateVelocity.IsEnabled = false;
            txtEstimatedAccuracy.Text = "";
            txtActualVelocity.Text = "";
            txtNewVelocity.IsReadOnly = true;
            txtNewVelocity.Text = "";
            ComboBoxSprint.IsEnabled = false;

            // Reset UI [AllUserStories]
            ComboBoxCategory.IsEnabled = false;
            ComboBoxSubCategory.IsEnabled = false;
            UserStoriesList.IsEnabled = false;
            UserStoriesList.ItemsSource = null;

            // Reset UI [Project]
            ButtonAddTeamMember.IsEnabled = false;
            ButtonAddNewProject.IsEnabled = false;
            txtTeamName.IsReadOnly = true;
            txtTeamName.Text = "";
            txtTeamNumber.IsReadOnly = true;
            txtTeamNumber.Text = "";
            txtProjectName.IsReadOnly = true;
            txtProjectName.Text = "";
            txtProjectStartDate.IsReadOnly = true;
            txtProjectStartDate.Text = "";
            txtInitialVelocity.IsReadOnly = true;
            txtInitialVelocity.Text = "";
            txtNewVelocity.Text = "";
            txtHoursPerPoint.IsReadOnly = true;
            txtHoursPerPoint.Text = "";
            txtHyperLink.IsReadOnly = true;
            txtHyperLink.Text = "";
            ComboBoxMember.IsEnabled = false;
            ButtonHyperLink.IsEnabled = false;

            // Reset UI [NewUserStory]
            txtNewUserStory.IsReadOnly = true;
            txtNewUserStory.Text = "";
            txtNewUserStoryPoint.IsReadOnly = true;
            txtNewUserStoryPoint.Text = "";
            ButtonAddNewUserStory.IsEnabled = false;
            ButtonAddNewProject.IsEnabled = false;

            // Reset UI [UserStory]
            ComboBoxSelectedUserStory.IsEnabled = false;
            ComboBoxAssignedMember.IsEnabled = false;
            ComboBoxUpdateSprint.IsEnabled = false;
            ComboBoxUpdateStatus.IsEnabled = false;

            txtCurrentUserStoryAssignedMember.Text = "";
            txtCurrentUserStorySprint.Text = "";
            txtCurrentUserStoryStatus.Text = "";
            txtStoryPoint.IsReadOnly = true;
            txtStoryPoint.Text = "";
            txtStoryEstimatedTime.Text = "";
            txtStoryActualTime.IsReadOnly = true;
            txtStoryActualTime.Text = "";
            txtNewTask.IsReadOnly = true;
            txtNewTask.Text = "";
            ButtonAddTask.IsEnabled = false;
            ComboBoxCurrentTaskList.IsEnabled = false;
            txtTaskActualTime.IsReadOnly = true;
            txtTaskActualTime.Text = "";
            ButtonUpdateTask.IsEnabled = false;
            ButtonUpdateUserStory.IsEnabled = false;

            // Reset the variables
            currentProject = null;
            currentProjectId = -1;
            newTeamMembersByProject = new List<User>();
            newUserStoriesByProject = new List<UserStory>();
            

        }

        // Current Project DropList
        private void ComboBoxProject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetUIByUser();

            // Reset
            teamMembersInScore = new List<User>();
            allSprintsByProject = new List<Sprint>();
            userStoriesByProject = new List<UserStory>();
            currentProject = null;

            currentProjectId = -2;
            currentProjectId = Convert.ToInt32(ComboBoxProject.SelectedValue);

            // If the project manager wants to maintain the existing project
            if (currentProjectId != -1 && loggedInUser.Role == "ProjectManager")
            {
                // Reset UI [ScoreSection]
                ComboBoxTeamMember.IsEnabled = true;
                ComboBoxUserStory.IsEnabled = true;
                txtTotalEstimatedTime.IsReadOnly = false;
                txtTotalActualTime.IsReadOnly = false;
                txtScore.IsReadOnly = false;

                // Reset UI [ProjectVelocityUpdate]
                ButtonUpdateVelocity.IsEnabled = true;
                txtNewVelocity.IsReadOnly = false;
                ComboBoxSprint.IsEnabled = true;

                // Reset UI [AllUserStories]
                ComboBoxCategory.IsEnabled = true;
                ComboBoxSubCategory.IsEnabled = true;
                UserStoriesList.IsEnabled = true;

                // Reset UI [Project]
                ButtonAddTeamMember.IsEnabled = false;
                ButtonAddNewProject.IsEnabled = false;
                txtTeamName.IsReadOnly = true;
                txtTeamNumber.IsReadOnly = true;
                txtProjectName.IsReadOnly = true;
                txtProjectStartDate.IsReadOnly = true;
                txtInitialVelocity.IsReadOnly = true;
                txtHoursPerPoint.IsReadOnly = true;
                txtHyperLink.IsReadOnly = true;
                ComboBoxMember.IsEnabled = false;
                ButtonHyperLink.IsEnabled = false;

                // Reset UI [NewUserStory]
                txtNewUserStory.IsReadOnly = true;
                txtNewUserStoryPoint.IsReadOnly = true;
                ButtonAddNewUserStory.IsEnabled = false;
                ButtonAddNewProject.IsEnabled = false;

                // Reset UI [UserStory]
                ComboBoxSelectedUserStory.IsEnabled = true;
                ComboBoxAssignedMember.IsEnabled = true;
                ComboBoxUpdateSprint.IsEnabled = true;
                ComboBoxUpdateStatus.IsEnabled = true;
                txtStoryPoint.IsReadOnly = true;
                txtNewTask.IsReadOnly = false;
                ButtonAddTask.IsEnabled = true;
                ComboBoxCurrentTaskList.IsEnabled = true;
                txtTaskActualTime.IsReadOnly = false;
                ButtonUpdateTask.IsEnabled = true;
                ButtonUpdateUserStory.IsEnabled = true;
                TeamMembersAddList.IsEnabled = false;

                // Get all team member from the database
                teamMembersInScore = new List<User>();
                teamMembersInScore = project.GetAllUsersByProject(currentProjectId);

                // Binding all team members to the Team Member DropList in the Score Section
                ComboBoxTeamMember.ItemsSource = new List<User>();
                ComboBoxTeamMember.ItemsSource = teamMembersInScore;

                // Save to the sprints list in the list
                allSprintsByProject = project.GetAllSprintsByProject(currentProjectId);
                userStoriesByProject = project.GetAllUserStoryByProject(currentProjectId);

                // Binding the sprint list to the Sprint DropList in the Manager Section
                ComboBoxSprint.ItemsSource = new List<Sprint>();
                ComboBoxSprint.ItemsSource = allSprintsByProject;

                // Binding all user stories to the All User Stories List in the All User Stories Section
                UserStoriesList.ItemsSource = new List<UserStory>();
                UserStoriesList.ItemsSource = userStoriesByProject;

                // Binding all user storis to the User Stories DropList in the UserStory Section
                ComboBoxSelectedUserStory.ItemsSource = new List<UserStory>(); ;
                ComboBoxSelectedUserStory.ItemsSource = userStoriesByProject;

                ComboBoxAssignedMember.ItemsSource = new List<User>();
                ComboBoxAssignedMember.ItemsSource = teamMembersInScore;

                // Initialize Project Information
                currentProject = project.GetProjectByID(currentProjectId);
                txtTeamName.Text = currentProject.TeamName;
                txtTeamNumber.Text = currentProject.TeamNumber;
                txtProjectName.Text = currentProject.ProjectName;
                txtProjectStartDate.Text = currentProject.StartDate.ToString();
                txtInitialVelocity.Text = project.GetInitialVelocity(currentProjectId).ToString();
                txtNewUpdatedVelocity.Text = project.GetNewVelocity(currentProjectId).ToString();
                txtHoursPerPoint.Text = currentProject.HoursPerStoryPoint.ToString();
                txtHyperLink.Text = currentProject.HyperlinkClickup;

            }
            // If the project manager wants to CREATE a NEW project
            else if (currentProjectId == -1 && loggedInUser.Role == "ProjectManager")
            {
                // Reset Project Information
                txtTeamName.Text = "";
                txtTeamNumber.Text = "";
                txtProjectName.Text = "";
                txtProjectStartDate.Text = "e.g. Jan 1, 2020";
                txtInitialVelocity.Text = "";
                txtNewUpdatedVelocity.Text = "";
                txtHoursPerPoint.Text = "";
                txtHyperLink.Text = "";

                // Reset UI [ScoreSection]
                ComboBoxTeamMember.IsEnabled = false;
                ComboBoxUserStory.IsEnabled = false;
                txtTotalEstimatedTime.IsReadOnly = true;
                txtTotalActualTime.IsReadOnly = true;
                txtScore.IsReadOnly = true;

                // Reset UI [ProjectVelocityUpdate]
                ButtonUpdateVelocity.IsEnabled = false;
                txtNewVelocity.IsReadOnly = true;
                ComboBoxSprint.IsEnabled = false;

                // Reset UI [AllUserStories]
                ComboBoxCategory.IsEnabled = false;
                ComboBoxSubCategory.IsEnabled = false;
                UserStoriesList.IsEnabled = false;

                // Reset UI [Project]
                ButtonAddTeamMember.IsEnabled = true;
                ButtonAddNewProject.IsEnabled = true;
                txtTeamName.IsReadOnly = false;
                txtProjectName.IsReadOnly = false;
                txtProjectStartDate.IsReadOnly = false;
                txtInitialVelocity.IsReadOnly = false;
                txtHoursPerPoint.IsReadOnly = false;
                txtHyperLink.IsReadOnly = false;
                ComboBoxMember.IsEnabled = true;
                ButtonHyperLink.IsEnabled = false;
                TeamMembersAddList.IsEnabled = true;
                txtProjectStartDate.Text = "e.g. Jan 1, 2020";

                // Reset UI [NewUserStory]
                txtNewUserStory.IsReadOnly = false;
                txtNewUserStoryPoint.IsReadOnly = false;
                ButtonAddNewUserStory.IsEnabled = true;
                ButtonAddNewProject.IsEnabled = true;

                // Reset UI [UserStory]
                ComboBoxSelectedUserStory.IsEnabled = false;
                ComboBoxAssignedMember.IsEnabled = false;
                ComboBoxUpdateSprint.IsEnabled = false;
                ComboBoxUpdateStatus.IsEnabled = false;
                txtStoryPoint.IsReadOnly = true;
                txtNewTask.IsReadOnly = true;
                ButtonAddTask.IsEnabled = false;
                ComboBoxCurrentTaskList.IsEnabled = false;
                txtTaskActualTime.IsReadOnly = true;
                ButtonUpdateTask.IsEnabled = false;
                ButtonUpdateUserStory.IsEnabled = false;

                currentProject = null;
            }
            // If the team members want to maintaine the existing the project    
            else if (currentProjectId != -1 && loggedInUser.Role != "ProjectManager")
            {
                // Reset UI [ScoreSection]
                ComboBoxTeamMember.IsEnabled = true;
                ComboBoxUserStory.IsEnabled = true;
                txtTotalEstimatedTime.IsReadOnly = false;
                txtTotalActualTime.IsReadOnly = false;
                txtScore.IsReadOnly = false;

                // Reset UI [ProjectVelocityUpdate]
                ButtonUpdateVelocity.IsEnabled = false;
                txtNewVelocity.IsReadOnly = true;
                ComboBoxSprint.IsEnabled = false;

                // Reset UI [AllUserStories]
                ComboBoxCategory.IsEnabled = true;
                ComboBoxSubCategory.IsEnabled = true;
                UserStoriesList.IsEnabled = true;

                // Reset UI [Project]
                ButtonAddTeamMember.IsEnabled = false;
                ButtonAddNewProject.IsEnabled = false;
                txtTeamName.IsReadOnly = true;
                txtTeamNumber.IsReadOnly = true;
                txtProjectName.IsReadOnly = true;
                txtProjectStartDate.IsReadOnly = true;
                txtInitialVelocity.IsReadOnly = true;
                txtHoursPerPoint.IsReadOnly = true;
                txtHyperLink.IsReadOnly = true;
                ComboBoxMember.IsEnabled = false;
                ButtonHyperLink.IsEnabled = true;
                TeamMembersAddList.IsEnabled = false;

                // Reset UI [NewUserStory]
                txtNewUserStory.IsReadOnly = true;
                txtNewUserStoryPoint.IsReadOnly = true;
                ButtonAddNewUserStory.IsEnabled = false;
                ButtonAddNewProject.IsEnabled = false;

                // Reset UI [UserStory]
                ComboBoxSelectedUserStory.IsEnabled = true;
                ComboBoxAssignedMember.IsEnabled = true;
                ComboBoxUpdateSprint.IsEnabled = true;
                ComboBoxUpdateStatus.IsEnabled = true;
                txtStoryPoint.IsReadOnly = true;
                txtNewTask.IsReadOnly = false;
                ButtonAddTask.IsEnabled = true;
                ComboBoxCurrentTaskList.IsEnabled = true;
                txtTaskActualTime.IsReadOnly = false;
                ButtonUpdateTask.IsEnabled = true;
                ButtonUpdateUserStory.IsEnabled = true;

                // Get all team member from the database
                teamMembersInScore = new List<User>();
                teamMembersInScore = project.GetAllUsersByProject(currentProjectId);

                // Binding all team members to the Team Member DropList in the Score Section
                ComboBoxTeamMember.ItemsSource = new List<User>(); ;
                ComboBoxTeamMember.ItemsSource = teamMembersInScore;

                // Save to the sprints list in the list
                allSprintsByProject = project.GetAllSprintsByProject(currentProjectId);
                userStoriesByProject = project.GetAllUserStoryByProject(currentProjectId);

                // Binding the sprint list to the Sprint DropList in the Manager Section
                ComboBoxSprint.ItemsSource = new List<Sprint>();
                ComboBoxSprint.ItemsSource = allSprintsByProject;

                // Binding all user stories to the All User Stories List in the All User Stories Section
                UserStoriesList.ItemsSource = new List<UserStory>();
                UserStoriesList.ItemsSource = userStoriesByProject;

                // Binding all user storis to the User Stories DropList in the UserStory Section
                ComboBoxSelectedUserStory.ItemsSource = new List<UserStory>();
                ComboBoxSelectedUserStory.ItemsSource = userStoriesByProject;

                ComboBoxAssignedMember.ItemsSource = new List<User>(); ;
                ComboBoxAssignedMember.ItemsSource = teamMembersInScore;

                // Initialize Project Information
                currentProject = project.GetProjectByID(currentProjectId);
                txtTeamName.Text = currentProject.TeamName;
                txtTeamNumber.Text = currentProject.TeamNumber;
                txtProjectName.Text = currentProject.ProjectName;
                txtProjectStartDate.Text = currentProject.StartDate.ToString();
                txtInitialVelocity.Text = project.GetInitialVelocity(currentProjectId).ToString();
                txtNewUpdatedVelocity.Text = project.GetNewVelocity(currentProjectId).ToString();
                txtHoursPerPoint.Text = currentProject.HoursPerStoryPoint.ToString();
                txtHyperLink.Text = currentProject.HyperlinkClickup;
            }
            

            // Reset
            newTeamMembersByProject = new List<User>();
            newUserStoriesByProject = new List<UserStory>();
        }

        #region dashboard_score_section
        private void ComboBoxTeamMember_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTeamMemberIdInScore = Convert.ToInt32(ComboBoxTeamMember.SelectedValue);    

  
            userStoriesInScore = project.GetAllUserStoryByUserID(selectedTeamMemberIdInScore, currentProjectId);

            userStoriesInScore.Add(new UserStory(-1, -1, -1, 0, "All User Stories", 0, 0, DateTime.Now, "", new List<Subtask>()));

            ComboBoxUserStory.ItemsSource = new List<UserStory>();
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
                txtTotalEstimatedTime.Text = project.GetEstimatedTimeByUserStory(selectedUserStoryIdInScore, currentProjectId).ToString() + " hour(s)";
                txtTotalActualTime.Text = project.GetTotalActualTimeByUserIdAndStoryId(selectedTeamMemberIdInScore, selectedUserStoryIdInScore, currentProjectId).ToString() + " hour(s)";
                txtScore.Text = project.GetScoreByUserIdAndStoryId(selectedTeamMemberIdInScore, selectedUserStoryIdInScore, currentProjectId).ToString();
            }  
        }
        #endregion

        #region dashboard_project_velocities_update_section
        private void ComboBoxSprint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSprintInVelocityUpdate = Convert.ToInt32(ComboBoxSprint.SelectedValue);

            txtEstimatedAccuracy.Text = project.GetEstimatedAccuracy(currentProjectId, selectedSprintInVelocityUpdate).ToString();
            txtActualVelocity.Text = project.GetActualVelocity(currentProjectId, selectedSprintInVelocityUpdate).ToString();
        }

        private void ButtonUpdateVelocity_Click(object sender, RoutedEventArgs e)
        {
            if(txtNewVelocity.Text.Length != 0)
            {
                double newVelocity = Convert.ToDouble(txtNewVelocity.Text);
                project.UpdateVelocityForSprint(newVelocity, selectedSprintInVelocityUpdate, currentProjectId);
                MessageBox.Show("New Velocity Updated!");

                // Reset
                txtEstimatedAccuracy.Text = "";
                txtActualVelocity.Text = "";
                txtNewVelocity.Text = "";
                ComboBoxSprint.ItemsSource = project.GetAllSprintsByProject(currentProjectId);
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
                    ComboBoxSubCategory.ItemsSource = allSprintsByProject;                 
                    break;

                case "Users":
                    userFullNameListBytProject = new List<string>();
                    foreach (var teamMember in teamMembersInScore)
                    {      
                        userFullNameListBytProject.Add(teamMember.FirstName + " " + teamMember.LastName);
                    }               
                    ComboBoxSubCategory.ItemsSource = userFullNameListBytProject;
                    break;
                case "Status":
                    ComboBoxSubCategory.ItemsSource = comboBoxSubCategoriesStatus;                   
                    break;
                default:
                    break;          
            }
        }

        private void ComboBoxSubCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {         
            var comboBox = sender as ComboBox;

            UserStoriesList.ItemsSource = null;

            switch (currentSelectedCategory)
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


        #region add_maintain_project
        private void ComboBoxMember_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedMemberId = Convert.ToInt32(ComboBoxMember.SelectedValue);   

            addingMemberToProject = allUsers[selectedMemberId];
        }

        private void ButtonAddTeamMember_Click(object sender, RoutedEventArgs e)
        {
            // If no duplicate, add a new member to the list
            if(!newTeamMembersByProject.Contains(addingMemberToProject))
            {
                newTeamMembersByProject.Add(addingMemberToProject);

                TeamMembersAddList.Items.Add(addingMemberToProject.FirstName + " " + addingMemberToProject.LastName);

                txtTeamNumber.Text = newTeamMembersByProject.Count.ToString();

                // Reset
                addingMemberToProject = null;
            }                   
        }

        private void ButtonHyperLink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              
                WebBrowser browser = new WebBrowser();
                Window win = new Window();
                win.Content = browser;
                win.Height = 1000;
                win.Width = 1220;
                win.Show();

                win.Title = "Project URL";
                browser.Navigate(new Uri(currentProject.HyperlinkClickup, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonAddNewUserStory_Click(object sender, RoutedEventArgs e)
        {
            if (txtNewUserStory.Text != String.Empty && txtNewUserStoryPoint.Text != String.Empty)
            {
                var nextAvailableUsetStoryId = (newUserStoriesByProject.Count > 0) ? newUserStoriesByProject[newUserStoriesByProject.Count - 1].Id + 1 : 0;

                var userStory = project.CreateNewUserStory(nextAvailableUsetStoryId, txtNewUserStory.Text, Int32.Parse(txtNewUserStoryPoint.Text));

                newUserStoriesByProject.Add(userStory);

                MessageBox.Show("New user story added!");
            }
            else
            {
                MessageBox.Show("Please insert a new user story and story point!");
            }
        }

        private void ButtonAddNewProject_Click(object sender, RoutedEventArgs e)
        {
            var projectName = txtProjectName.Text;
            var teamName = txtTeamName.Text;
            var teamNumber = txtTeamNumber.Text;
            var hyperlink = txtHyperLink.Text;
            var startDateTime = txtProjectStartDate.Text;
            var initialVelocity = txtInitialVelocity.Text;
            var hoursPerPoint = txtHoursPerPoint.Text;
            var teamMembers = newTeamMembersByProject;

            // Assign the logged User to the team member as Project Manager
            if(loggedInUser.Role == "ProjectManager")
                newTeamMembersByProject.Add(loggedInUser);

            if (projectName != String.Empty && teamName != String.Empty 
                && teamNumber != String.Empty && hyperlink != String.Empty && startDateTime != String.Empty
                && initialVelocity != String.Empty && hoursPerPoint != String.Empty && teamMembers != null)
            {

                project.AddProject(projectName, teamName, teamNumber, hyperlink, DateTime.Parse(startDateTime), 
                    double.Parse(initialVelocity), double.Parse(hoursPerPoint), newUserStoriesByProject, teamMembers);

                //// Reset UI
                txtProjectName.Text ="";
                txtTeamName.Text = "";
                txtTeamNumber.Text = "";
                txtHyperLink.Text = "";
                txtProjectStartDate.Text = "";
                txtInitialVelocity.Text = "";
                txtHoursPerPoint.Text = "";
                TeamMembersAddList.ItemsSource = new List<User>();

                // Reset
                newTeamMembersByProject = new List<User>(); ;
                newUserStoriesByProject = new List<UserStory>();

                MessageBox.Show("New project added!");
            }
            else
            {
                MessageBox.Show("Please provide all information for new project!");
            }
                  
        }
        #endregion

        #region user_story_section
        private void ButtonAddTask_Click(object sender, RoutedEventArgs e)
        {
            if (txtNewTask.Text != String.Empty)
            {
                project.AddTaskToUserStory(selectedUserStoryIdInUserStory, currentProjectId, DateTime.Now, txtNewTask.Text, 0);

                MessageBox.Show("New task added!");

                // Reset
                txtNewTask.Text = "";
            }
            else
            {
                MessageBox.Show("Please insert a new task");
            }
        }

        private void ComboBoxSelectedUserStory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Reset
            txtTaskActualTime.Text = "";
            txtNewTask.Text = "";
            txtStoryPoint.Text = "";
            txtStoryEstimatedTime.Text = "";
            txtStoryActualTime.Text = "";

            allSubtasksByUersStory = new List<Subtask>();

            var selectedUserStoryId = Convert.ToInt32(ComboBoxSelectedUserStory.SelectedValue);

            selectedUserStoryIdInUserStory = selectedUserStoryId;

            var selectedUserStory = userStoriesByProject[selectedUserStoryId];

            var user = project.GetUserByUserStory(selectedUserStoryId, currentProjectId);

            
            txtCurrentUserStoryAssignedMember.Text = user.FirstName + " " + user.LastName;
            txtCurrentUserStorySprint.Text = selectedUserStory.SprintId.ToString();
            txtCurrentUserStoryStatus.Text = selectedUserStory.Status.ToString();


            ComboBoxUpdateSprint.ItemsSource = new List<Sprint>();
            ComboBoxUpdateSprint.ItemsSource = allSprintsByProject;


            ComboBoxUpdateStatus.ItemsSource = new List<string>();
            ComboBoxUpdateStatus.ItemsSource = comboBoxUserStoryStatus;

            txtStoryPoint.Text = selectedUserStory.StoryPoint.ToString();
            txtStoryEstimatedTime.Text = selectedUserStory.InitialEstimatedHours.ToString();
            txtStoryActualTime.Text = selectedUserStory.ActualWorkHours.ToString();


            // Sub tasks
            allSubtasksByUersStory = project.GetAllSubtasksByUserStory(selectedUserStoryId, currentProjectId);

            ComboBoxCurrentTaskList.ItemsSource = new List<Subtask>();
            ComboBoxCurrentTaskList.ItemsSource = project.GetAllSubtasksByUserStory(selectedUserStoryId, currentProjectId);
            
        }

        private void ComboBoxCurrentTaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSubTaskId = Convert.ToInt32(ComboBoxCurrentTaskList.SelectedValue);
            selectedSubTaskIdInUserStory = selectedSubTaskId;
            var selectedSubTask = allSubtasksByUersStory.Where(st => st.Id == selectedSubTaskId).FirstOrDefault();
            txtTaskActualTime.Text = selectedSubTask.ActualWorkHours.ToString();
        }

        private void ButtonUpdateTask_Click(object sender, RoutedEventArgs e)
        {
            if(txtTaskActualTime.Text != "0")
            {
                project.UpdateActualTimeBySubTask(double.Parse(txtTaskActualTime.Text), selectedSubTaskIdInUserStory, selectedUserStoryIdInUserStory, currentProjectId);

                // Reset
                //selectedSubTaskIdInUserStory = 0;
                txtTaskActualTime.Text = "";

                MessageBox.Show("Task actual time updated");
            }
            else
            {
                MessageBox.Show("Please insert a new actual time");
            }

        }

        private void ButtonUpdateUserStory_Click(object sender, RoutedEventArgs e)
        {
            var assigningMemberId = selectedAssigningUserInUserStory;
            var updateSprint = selectedUpdateSprintInUserStory;
            var updateStatus = selectedUpdateStatusInUserStory;
            var storyPoint = txtStoryPoint.Text;
            var storyActualTime = txtStoryActualTime.Text;

            if (assigningMemberId != -1 && updateSprint != -1
                && updateStatus != "" && storyPoint != String.Empty && storyActualTime != String.Empty)
            {

                project.UpdateUserStory(currentProjectId, selectedUserStoryIdInUserStory, assigningMemberId, updateSprint, updateStatus, Int32.Parse(storyPoint), Double.Parse(storyActualTime));

                MessageBox.Show("User story updated!");

                //// Reset
                txtStoryPoint.Text = "";
                txtStoryEstimatedTime.Text = "";
                txtStoryActualTime.Text = "";
                selectedUpdateSprintInUserStory = -1;
                selectedUpdateStatusInUserStory = "";
                selectedAssigningUserInUserStory = -1;
            }
            else
            {
                MessageBox.Show("Please provide all information for new project!");
            }
        }

        private void ComboBoxAssignedMember_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedUpdateSprint = Convert.ToInt32(ComboBoxAssignedMember.SelectedValue);

            selectedAssigningUserInUserStory = -1;
            selectedAssigningUserInUserStory = selectedUpdateSprint;
        }

        private void ComboBoxUpdateSprint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedUpdateSprint = Convert.ToInt32(ComboBoxUpdateSprint.SelectedValue);

            selectedUpdateSprintInUserStory = -1;
            selectedUpdateSprintInUserStory = selectedUpdateSprint;
        }

        private void ComboBoxUpdateStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            var status = comboBox.SelectedItem as string;

            selectedUpdateStatusInUserStory = "";
            selectedUpdateStatusInUserStory = status;
        }
        #endregion

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }   
    }// end MainWindow
}
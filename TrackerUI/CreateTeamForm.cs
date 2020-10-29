using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTeamForm : Form
    {
        private List<PersonModel> availableTeamMembers = GlobalConfig.Connection.GetPersonAll();
        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();

        private string InvalidFormMessage = "This form has invalid information. Please check it and try again.";

        public CreateTeamForm()
        {
            InitializeComponent();

            //CreateSampleData();

            WireUpLists();
        }

        private void CreateSampleData()
        {
            availableTeamMembers.Add(new PersonModel { FirstName = "Tim", LastName = "Corey" });
            availableTeamMembers.Add(new PersonModel { FirstName = "Jim", LastName = "Corey" });

            selectedTeamMembers.Add(new PersonModel { FirstName = "Sam", LastName = "Corey" });
            selectedTeamMembers.Add(new PersonModel { FirstName = "Julie", LastName = "Corey" });
        }


        private void WireUpLists()
        {
            selectMemberDropDown.DataSource = null;

            selectMemberDropDown.DataSource = availableTeamMembers;
            selectMemberDropDown.DisplayMember = "FullName";

            teamMembersListBox.DataSource = null;

            teamMembersListBox.DataSource = selectedTeamMembers;
            teamMembersListBox.DisplayMember = "FullName";
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (FormIsValid() == true)
            {
                var p = new PersonModel();

                p.FirstName = firstNameTextBox.Text;
                p.LastName = lastNameTextBox.Text;
                p.Email = emailTextBox.Text;
                p.PhoneNumber = phoneNumberTextBox.Text;

                // Passing back the person after getting the id from inserting the 
                // person in the database.
                p = GlobalConfig.Connection.createPerson(p);

                availableTeamMembers.Add(p);

                WireUpLists();

                firstNameTextBox.Text = "";
                lastNameTextBox.Text = "";
                emailTextBox.Text = "";
                phoneNumberTextBox.Text = "";
            }
            else
            {
                MessageBox.Show(InvalidFormMessage);

            }
        }


        private bool FormIsValid()
        {
            bool output = true;

            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(lastNameTextBox.Text))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(emailTextBox.Text))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(phoneNumberTextBox.Text))
            {
                return false;
            }

            return output;
        }

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)selectMemberDropDown.SelectedItem;

            if (p != null)
            {
                availableTeamMembers.Remove(p);
                selectedTeamMembers.Add(p);

                WireUpLists();
            }
        }

        private void removeSelectedMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;

            if (p != null)
            {
                selectedTeamMembers.Remove(p);
                availableTeamMembers.Add(p);

                WireUpLists();
            }

        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            if (TeamNameIsValid())
            {
                TeamModel t = new TeamModel();

                t.TeamName = teamNameTextBox.Text;
                t.TeamMembers = selectedTeamMembers;

                t = GlobalConfig.Connection.createTeam(t);

                // TODO - If we aren't closing this form after creation, reset the form
            }
            else
            {
                MessageBox.Show(InvalidFormMessage);
            }



        }

        private bool TeamNameIsValid()
        {
            bool output = true;

            if (string.IsNullOrWhiteSpace(teamNameTextBox.Text))
            {
                output = false;
            }

            return output;
        }


    }
}

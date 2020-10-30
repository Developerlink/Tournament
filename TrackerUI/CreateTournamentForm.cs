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
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
    {
        List<TeamModel> availableTeams = GlobalConfig.Connection.GetTeamAll();
        List<TeamModel> selectedTeams = new List<TeamModel>();
        List<PrizeModel> selectedPrizes = new List<PrizeModel>();

        public CreateTournamentForm()
        {
            InitializeComponent();

            WireUpLists();
        }

        private void WireUpLists()
        {
            availableTeams = availableTeams.OrderBy(t => t.TeamName).ToList();
            selectedTeams = selectedTeams.OrderBy(t => t.TeamName).ToList();

            selectTeamDropDown.DataSource = null;
            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            tournamentTeamsListBox.DataSource = null;
            tournamentTeamsListBox.DataSource = selectedTeams;
            tournamentTeamsListBox.DisplayMember = "TeamName";
            if (tournamentTeamsListBox.Items.Count > 0)
            {
                tournamentTeamsListBox.SelectedIndex = 0;
            }

            prizesListBox.DataSource = null;
            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = (TeamModel)selectTeamDropDown.SelectedItem;

            if (t != null)
            {
                availableTeams.Remove(t);
                selectedTeams.Add(t);
            }

            WireUpLists();
        }

        private void removeSelectedPlayerButton_Click(object sender, EventArgs e)
        {
            TeamModel t = (TeamModel)tournamentTeamsListBox.SelectedItem;

            if (t != null)
            {
                availableTeams.Add(t);
                selectedTeams.Remove(t);
            }

            WireUpLists();
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            // Call the CreatePrizeForm
            // Get back from the form a PrizeModel
            CreatePrizeForm frm = new CreatePrizeForm(this);
            frm.Show();
        }

        public void PrizeComplete(PrizeModel model)
        {
            // Take the model and put it into our list of selected prizes
            selectedPrizes.Add(model);
            WireUpLists();
        }

        private void createNewLinklabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm form = new CreateTeamForm(this);
            form.Show();
        }
        public void TeamComplete(TeamModel model)
        {
            selectedTeams.Add(model);
            WireUpLists();
        }

        private void removeSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel p = (PrizeModel)prizesListBox.SelectedItem;

            if (p != null)
            {
                selectedPrizes.Remove(p);
            }

            WireUpLists();
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            if (TournamentIsValid() == true)
            {
                // Create Tournament entry
                var t = new TournamentModel();
                t.TournamentName = tournamentNameTextBox.Text;
                t.EntryFee = decimal.Parse(entryFeeTextBox.Text);
                t.Prizes = selectedPrizes;
                t.EnteredTeams = selectedTeams;                

                // Create our matchups

                // Create all of the prizes entries
                // Create all of team entries
                GlobalConfig.Connection.CreateTournament(t);

            }
        }

        bool TournamentIsValid()
        {
            bool result = true;

            if (string.IsNullOrWhiteSpace(tournamentNameTextBox.Text))
            {
                result = false;
                MessageBox.Show("You need to enter a name for the tournament");
            }

            if (string.IsNullOrWhiteSpace(entryFeeTextBox.Text))
            {
                result = false;
                MessageBox.Show("You need to enter an amount in as the entry fee");
            }

            decimal feeAmount = 0;

            bool isFeeValid = decimal.TryParse(entryFeeTextBox.Text, out feeAmount);

            if (isFeeValid == false)
            {
                result = false;
                MessageBox.Show("You need to enter a valid number as the entry fee.");
            }



            return result;
        }
    }
}

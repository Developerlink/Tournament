using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {

        public static void CreateRounds(TournamentModel model)
        {
            // Order list randomly
            List<TeamModel> randomizedTeams = RandomizeTeamOrder(model.EnteredTeams);
            int rounds = FindNumberOfRounds(randomizedTeams.Count);
            // Check if it is big enough - if not, add in byes - 2*2*2*2 - 2^4
            int byes = NumberOfByes(rounds, randomizedTeams.Count);

            // Create first round of matchups
            model.Rounds.Add(CreateFirstRound(byes, randomizedTeams));

            // Create every round after that - 8 - 4 - 2 - 1
            CreateOtherRounds(model, rounds);
        }

        private static void CreateOtherRounds(TournamentModel model, int rounds)
        {
            int round = 2;
            List<MatchupModel> previousRound = model.Rounds[0];
            List<MatchupModel> currRound = new List<MatchupModel>();
            MatchupModel currMatchup = new MatchupModel();

            // Starting from round 2. If there are more then keep looping until all 
            // rounds have matched up.
            while (round <= rounds)
            {
                // For each match in the previous round of matchups
                foreach (MatchupModel match in previousRound)
                {
                    // Use that matchup as the parent matchup of this matchup entry. We don't
                    // know who the winner from that matchup is, but we know who the parent matchup is.
                    // So we put in an anonymous team in where only the parent matchup is known. 
                    currMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = match });

                    if (currMatchup.Entries.Count > 1)
                    {
                        currMatchup.MatchupRound = round;
                        currRound.Add(currMatchup);
                        currMatchup = new MatchupModel();
                    }
                }

                // Add the new list of matchups to the tournament
                model.Rounds.Add(currRound);
                // Make the completed list the new previous list
                previousRound = currRound;

                // Reset the current list.
                currRound = new List<MatchupModel>();
                // Change the round number identity.
                round++;
            }
        }

        // The first round gets its own method because it knows how the matchups are going to be.
        private static List<MatchupModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel curr = new MatchupModel();

            // For each team put it into a matchup.
            foreach (TeamModel team in teams)
            {
                curr.Entries.Add(new MatchupEntryModel { TeamCompeting = team });

                // Every matchup has to mathcup entries.
                // If there are 2 entries then the matchup is complete,
                // therefore if entries are more than 1, add the matchup into
                // list of matchups and start over with a new matchup
                // The byes are used on the FIRST teams, that's why if byes > 0 
                // add the current matchup directly to the output, even though the 
                // Entries.count isn't greater than 1 yet, and move ont to the next matchup.
                // Dong it this way takes care of all the byes in the first round.
                if (byes > 0 || curr.Entries.Count > 1)
                {
                    curr.MatchupRound = 1;
                    output.Add(curr);
                    curr = new MatchupModel();

                    if (byes > 0)
                    {
                        byes -= 1;
                    }
                }
            }

            return output;
        }

        private static int NumberOfByes(int rounds, int numberOfTeams)
        {
            int output = 0;
            int totalTeams = 1;

            // Find the total number of teams needed in the very first round.
            // To do that calculate 2^rounds.
            for (int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }

            // Number of byes (dummy teams) will be totalTeams - (actual) numberOfTeams.
            output = totalTeams - numberOfTeams;

            return output;
        }

        private static int FindNumberOfRounds(int teamCount)
        {
            int output = 1;
            int val = 2;

            // val is the number of teams competing in the final round
            // We need at least to teams for a tournament with 1 round.
            // If val is smaller than the number of teams, then we need
            // to add 1 more round and calculate the number of teams a
            // second round can handle, which is the original val * 2
            // and so on it goes until val can accomodate all teams.
            while (val < teamCount)
            {
                output++;

                val *= 2;
            }

            return output;
        }

        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            // Randomizes the order in a list.
            return teams.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}

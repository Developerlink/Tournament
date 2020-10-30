using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess.TextHelper
{
    public static class TextConnectorProcessor
    {
        // The 'this' keyword 
        public static string FullFilePath(this string fileName) // PrizeModels.csv
        {
            // Putting together the filename with the standard folder path to get:
            // C:\\data\TournamentTracker\PrizeModels.csv.
            return $"{ ConfigurationManager.AppSettings["filePath"] }\\{ fileName } ";
        }

        /// <summary>
        /// Reads all lines from file and separates each line 
        /// into a string that are added into a list of strings
        /// and then returns a List<string></string>.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<string> LoadFile(this string file)
        {
            // Returns and empty list if file does not exist. 
            if (File.Exists(file) == false)
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        /// <summary>
        /// Converts a list of strings into a list of PrizeModels.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            foreach (string line in lines)
            {
                // Each column is serarated by a ','.
                string[] columns = line.Split(',');

                PrizeModel p = new PrizeModel();
                // We know ourselves what the columns at index x are. 
                p.Id = int.Parse(columns[0]);
                p.PlaceNumber = int.Parse(columns[1]);
                p.PlaceName = columns[2];
                p.PrizeAmount = decimal.Parse(columns[3]);
                p.PrizePercentage = double.Parse(columns[4]);
                output.Add(p);
            }

            return output;
        }

        /// <summary>
        /// Saves a list of PrizeModels into a text file.
        /// </summary>
        /// <param name="models"></param>
        /// <param name="fileName"></param>
        public static void SaveToPrizeFile(this List<PrizeModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PrizeModel p in models)
            {
                lines.Add($"{ p.Id },{ p.PlaceNumber },{ p.PlaceName },{ p.PrizeAmount },{ p.PrizePercentage }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();

            foreach (string line in lines)
            {
                // Each column is serarated by a ','.
                string[] columns = line.Split(',');

                PersonModel p = new PersonModel();
                // We know ourselves what the columns at index x are. 
                p.Id = int.Parse(columns[0]);
                p.FirstName = columns[1];
                p.LastName = columns[2];
                p.Email = columns[3];
                p.PhoneNumber = columns[4];
                output.Add(p);
            }

            return output;
        }

        public static void SaveToPeopleFile(this List<PersonModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel p in models)
            {
                lines.Add($"{ p.Id },{ p.FirstName },{ p.LastName },{ p.Email },{ p.PhoneNumber }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string peopleFileName)
        {
            // id, team name, list of ids separated by the pipe
            // Read all existing teams from the team file into a list, each string 
            // representing all the info in a team.
            List<TeamModel> output = new List<TeamModel>();
            // Read all the existing people from the people file into a list. 
            List<PersonModel> people = peopleFileName.FullFilePath().LoadFile().ConvertToPersonModels();

            // For each team/string/line split it up into columns via the ',' seperator
            foreach (string line in lines)
            {
                // Each column is serarated by a ','.
                string[] columns = line.Split(',');

                TeamModel t = new TeamModel();
                // We know ourselves what the columns at index x are. 
                t.Id = int.Parse(columns[0]);
                t.TeamName = columns[1];

                // This column is special since it contains list of id's seperated by a '|'
                string[] personIds = columns[2].Split('|');

                // Populate the teams TeamMembers property by adding only the people
                // whose id's match
                foreach (string id in personIds)
                {
                    t.TeamMembers.Add(people.Where(x => x.Id == int.Parse(id)).First());
                }

                // Now the list of teams should be read completely and correctly into output.
                // Add it to the output
                output.Add(t);
            }

            return output;
        }

        public static void SaveToTeamsFile(this List<TeamModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (TeamModel t in models)
            {
                lines.Add($"{ t.Id },{ t.TeamName },{ ConvertPeopleListToString(t.TeamMembers) }");
            }
            
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        private static string ConvertPeopleListToString(List<PersonModel> people)
        {
            string output = "";

            // This in case there are no people which will cause
            // Substring method down below to cause a error.
            if (people.Count == 0)
            {
                return "";
            }

            foreach (PersonModel p in people)
            {
                output += $"{p.Id}|";
            }

            // Removes the last '|' from the string.
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        public static List<TournamentModel> ConvertToTournamentModels(
            this List<string> lines, 
            string tournamentFileName, 
            string teamFileName, 
            string peopleFileName,
            string PrizeFileName)
        {
            // 0 = Id
            // 1 = TournamentName
            // 2 = EntryFee
            // 3 = EnteredTeams
            // 4 = Prizes
            // 5 = Rounds
            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = teamFileName.FullFilePath().LoadFile().ConvertToTeamModels(peopleFileName);
            List<PrizeModel> prizes = new List<PrizeModel>();
            List<MatchupModel> rounds = new List<MatchupModel>();

            foreach (string line in lines)
            {
                string[] columns = line.Split(',');

                var tm = new TournamentModel();
                tm.Id = int.Parse(columns[0]);
                tm.TournamentName = columns[1];
                tm.EntryFee = decimal.Parse(columns[2]);

                string[] teamIds = columns[3].Split('|');
                foreach (string id in teamIds)
                {
                    tm.EnteredTeams.Add(teams.Where(x => x.Id == int.Parse(id)).First());
                }

                string[] prizeIds = columns[4].Split('|');
                foreach (string id in prizeIds)
                {
                    tm.Prizes.Add(prizes.Where(x => x.Id == int.Parse(id)).First());
                }


            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class TournamentModel
    {
        /// <summary>
        /// The tournament's id number
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The tournament's name
        /// </summary>
        public string TournamentName { get; set; }

        /// <summary>
        /// The entry fee.
        /// </summary>
        public decimal EntryFee { get; set; }

        /// <summary>
        /// The list of teams that have entered the tournament.
        /// </summary>
        public List<TeamModel> EnteredTeams { get; set; } = new List<TeamModel>();

        /// <summary>
        /// The list of prizes for this tournament.
        /// </summary>
        public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();

        /// <summary>
        /// The list of rounds and matchups.
        /// </summary>
        public List<List<MatchupModel>> Rounds { get; set; } = new List<List<MatchupModel>>();
    }
}

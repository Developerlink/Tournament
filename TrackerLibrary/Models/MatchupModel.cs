using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchupModel
    {
        /// <summary>
        /// The id for the matchup.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// List of matchups.
        /// </summary>
        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();

        /// <summary>
        /// The winner team.
        /// </summary>
        public TeamModel Winner { get; set; }

        /// <summary>
        /// The matchup round number.
        /// </summary>
        public int MatchupRound { get; set; }
    }
}

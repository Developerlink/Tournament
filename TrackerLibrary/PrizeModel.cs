using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class PrizeModel
    {
        /// <summary>
        /// The id number of the prize.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The place number that receives this prize.
        /// </summary>
        public int PlaceNumber { get; set; }

        /// <summary>
        /// The place number's name.
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// The prize amount.
        /// </summary>
        public decimal PrizeAmount { get; set; }

        /// <summary>
        /// The percentage of total amount allocated to this prize.
        /// </summary>
        public double PrizePercentage { get; set; }
    }
}

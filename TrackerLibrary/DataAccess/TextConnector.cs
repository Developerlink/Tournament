using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {        
        /// <summary>
        /// Saves a new prize as a text file.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PrizeModel createPrize(PrizeModel model)
        {
            model.Id = 1;

            return model;
        }
    }
}

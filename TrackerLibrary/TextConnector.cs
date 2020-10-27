using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class TextConnector : IDataConnection
    {
        // TODO: Wire up the CreatePrize for text files. 
        /// <summary>
        /// 
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

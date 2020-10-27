using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class PersonModel
    {
        /// <summary>
        /// The person's id number.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The person's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The person's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The person's email address.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// The person's phone number
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}

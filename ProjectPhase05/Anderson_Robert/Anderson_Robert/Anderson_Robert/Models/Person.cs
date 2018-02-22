/* Person Class Definition 
   Each definition has a label definition and 
   a variable type definition like the ones below:
   
   [DisplayName("Label Description:")]
   public DataType VariableName { get; set; }
 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Anderson_Robert.Models
{
    public class Person
    {
        [DisplayName("First Name:")]
        public String PersonFirstName { get; set; }

        [DisplayName("Last Name:")]
        public String PersonLastName { get; set; }

        [DisplayName("Email:")]
        public String PersonEmail { get; set; }

        [DisplayName("Phone:")]
        public String PersonPhone { get; set; }

        [DisplayName("Address:")]
        public String PersonAddress { get; set; }

        [DisplayName("User Name:")]
        public String PersonUserName { get; set; }

        [DisplayName("Password:")]
        public String PersonPassword { get; set; }

        public String PersonID { get; set; }

    }
}

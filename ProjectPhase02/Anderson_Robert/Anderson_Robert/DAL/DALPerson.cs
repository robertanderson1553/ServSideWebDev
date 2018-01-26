using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anderson_Robert.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Anderson_Robert.DAL
{
    public class DALPerson
    {
        private IConfiguration configuration;

        public DALPerson(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string AddPerson(Models.Person person)
        {
            //Step #1 - Connect to the DB
            string connStr = configuration.GetConnectionString("MyConnStr");
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            //Step #2 - create a command
            string query = "INSERT INTO [dbo].[Person]([FName],[LName],[email],[phone],[address],[UserName])VALUES(@pFName, @pLName, @pEmail, @pPhone, @pAddress, @pUserName) select SCOPE_IDENTITY() as PersonID;";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@pFName", person.PersonFirstName);
            cmd.Parameters.AddWithValue("@pLName", person.PersonLastName);
            cmd.Parameters.AddWithValue("@pEmail", person.PersonEmail);
            cmd.Parameters.AddWithValue("@pPhone", person.PersonPhone);
            cmd.Parameters.AddWithValue("@pAddress", person.PersonAddress);
            cmd.Parameters.AddWithValue("@pUserName", person.PersonUserName);

            //Step #3 - query the DB
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int personID = Convert.ToInt32(reader[0].ToString());

            //Step #4 - close the reader
            reader.Close();

            //Step #5 - reset the query (create a command)
            cmd.CommandText = "INSERT INTO [dbo].[Credentials]([PersonID],[Password])VALUES(@pPID, @pPassword);";
            cmd.Parameters.AddWithValue("@pPID", personID);
            cmd.Parameters.AddWithValue("@pPassword", person.PersonPassword);

            //Step #6 - Query
            cmd.ExecuteNonQuery();
            
            //Step #7 - close the connection
            conn.Close();

            return personID.ToString();
        }
    }
}

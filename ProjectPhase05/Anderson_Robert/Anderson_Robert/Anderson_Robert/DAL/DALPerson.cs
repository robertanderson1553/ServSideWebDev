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

        internal void UpdateUser(Person person)
        {
            //Step #1 - Connect to the DB
            string connStr = configuration.GetConnectionString("MyConnStr");
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            // Step #2 - create a command
            string query = "UPDATE [dbo].[Person]SET [FName] = @pFName, [LName] = @pLName, [email] = @pEmail, [phone] = @pPhone, [address] = @pAddress, [UserName] = @pUserName WHERE PersonID = @pUID;";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@pFName", person.PersonFirstName);
            cmd.Parameters.AddWithValue("@pLName", person.PersonLastName);
            cmd.Parameters.AddWithValue("@pEmail", person.PersonEmail);
            cmd.Parameters.AddWithValue("@pPhone", person.PersonPhone);
            cmd.Parameters.AddWithValue("@pAddress", person.PersonAddress);
            cmd.Parameters.AddWithValue("@pUserName", person.PersonUserName);
            cmd.Parameters.AddWithValue("@pUID", person.PersonID);

            //Step #3 - query the DB
            cmd.ExecuteNonQuery();

            //Step #4 - close the connection
            conn.Close();
        }

        internal void DeletePerson(int uID)
        {
            //Step #1 - Connect to the DB
            string connStr = configuration.GetConnectionString("MyConnStr");
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            // Step #2 - create a command
            string query = "Delete From [dbo].[Credentials] WHERE PersonID = @pUID;";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@pUID", uID);

            //Step #3 - query the DB
            cmd.ExecuteNonQuery();

            //Step #4 - reset the command
            cmd.CommandText = "Delete From [dbo].[Person] WHERE PersonID = @pUID2;";
            cmd.Parameters.AddWithValue("@pUID2", uID);

            //Step #5 - query the DB again
            cmd.ExecuteNonQuery();

            //Step #6 - close the connection
            conn.Close();
        }

        internal Person getPerson(int uID)
        {
            Person person = new Person();

            //Step #1 - Connect to the DB
            string connStr = configuration.GetConnectionString("MyConnStr");
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            //Step #2 - create a command
            string query = "Select [FName],[LName],[email],[phone],[address],[UserName] From [dbo].[Person] Where PersonID = @pUID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@pUID", uID);
            
            //Step #3 - query the DB
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            person.PersonFirstName = reader["FName"].ToString();
            person.PersonLastName = reader["LName"].ToString();
            person.PersonEmail = reader["email"].ToString();
            person.PersonPhone = reader["phone"].ToString();
            person.PersonAddress = reader["address"].ToString();
            person.PersonUserName = reader["UserName"].ToString();
            person.PersonID = uID.ToString();

            //Step #4 - close the reader
            reader.Close();

            //Step #5 - close the connection
            conn.Close();

            return person;
        }
    }
}

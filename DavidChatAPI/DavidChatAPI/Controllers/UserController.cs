using DavidChatAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DavidChatAPI.Controllers
{
    public class UserController : ApiController
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;

        [HttpGet]
        [Route("User/Register")]
        public User Register()
        {
            DataTable table = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("Client.SendMessage", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            { 
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                adapter.Fill(table);
                connection.Close();
            }
            Guid guid;
            bool result = Guid.TryParse(table.Rows[0].Field<string>("UserID"), out guid);
            
            if(result)
            {
                return new User() { Guid = guid };
            }

            return null;
        }
    }
}

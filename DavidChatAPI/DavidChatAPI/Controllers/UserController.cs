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
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;

        [HttpGet]
        [Route("Register/{name}/{color}")]
        public User Register(string name, ConsoleColor color)
        {
            DataTable table = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("Client.Register", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            { 
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@name", name));
                command.Parameters.Add(new SqlParameter("@color", color));

                connection.Open();
                adapter.Fill(table);
                connection.Close();
            }

            return new User() { Guid = table.Rows[0].Field<Guid>("UserID") };
        }
    }
}

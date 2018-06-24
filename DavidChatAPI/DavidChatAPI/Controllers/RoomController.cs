using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DavidChatAPI.Models;

namespace DavidChatAPI.Controllers
{
    [RoutePrefix("api")]
    public class RoomController : ApiController
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;

        [HttpGet]
        [Route("Rooms/GetRooms/")]
        public Room[] GetRooms()
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("client.GetRooms", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                adapter.Fill(table);
                connection.Close();
            }

            List<Room> rooms = new List<Room>();
            foreach (DataRow row in table.Rows)
            {
                rooms.Add(new Room()
                {
                    Name = row.Field<string>("Name").Trim()
                });
            }

            return rooms.ToArray();
        }
        
        [HttpPost]
        [Route("Rooms/JoinRoom/")]
        public Room JoinRoom([FromBody] Guid userID, [FromBody]string name, [FromBody]string password)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("Client.JoinRoom", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@userID", userID));
                command.Parameters.Add(new SqlParameter("@roomName", name));
                command.Parameters.Add(new SqlParameter("@password", password));
                
                connection.Open();
                adapter.Fill(table);
                connection.Close();
            }

            return new Room()
            {
                Name = name,
                Password = password,
                RoomID = table.Rows[0].Field<Guid>("RoomID")
            };
        }

        [HttpPost]
        [Route("Rooms/CreateRoom/")]
        public Room CreateRoom([FromBody] Guid userID, [FromBody]string name, [FromBody]string password)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("Client.JoinRoom", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@userID", userID));
                command.Parameters.Add(new SqlParameter("@roomName", name));
                command.Parameters.Add(new SqlParameter("@password", password));

                connection.Open();
                adapter.Fill(table);
                connection.Close();
            }

            return new Room()
            {
                Name = name,
                Password = password,
                RoomID = table.Rows[0].Field<Guid>("RoomID")
            };
        }

        [HttpPost]
        [Route("Rooms/LeaveRoom/{userID}/{roomID}")]
        public void CreateRoom(Guid userID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("Client.LeaveRoom", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@userID", userID));

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}

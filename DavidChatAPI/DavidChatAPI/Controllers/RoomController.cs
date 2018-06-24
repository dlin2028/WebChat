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
using WebChat.Models;

namespace DavidChatAPI.Controllers
{
    [RoutePrefix("api")]
    public class RoomController : ApiController
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;

        [HttpGet]
        [Route("Room/GetRooms/")]
        public GetRoomsResponse GetRooms()
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


            List<string> rooms = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                rooms.Add(row.Field<string>("Name"));
            }

            return new GetRoomsResponse() { Rooms = rooms.ToArray() };
        }
        
        [HttpPost]
        [Route("Room/JoinRoom/")]
        public JoinRoomResponse JoinRoom([FromBody] JoinRoom joinRoom)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("Client.JoinRoom", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@userID", joinRoom.UserID));
                command.Parameters.Add(new SqlParameter("@roomName", joinRoom.Name));
                command.Parameters.Add(new SqlParameter("@password", joinRoom.Password));
                
                connection.Open();
                adapter.Fill(table);
                connection.Close();
            }

            if(table.Rows.Count <= 0)
            {
                return new JoinRoomResponse() { Result = false };
            }

            return new JoinRoomResponse()
            {
                Result = true,
                RoomID = table.Rows[0].Field<Guid>("RoomID")
            };
        }

        [HttpPost]
        [Route("Room/CreateRoom/")]
        public CreateRoomResponse CreateRoom([FromBody] CreateRoom createRoom)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("Client.JoinRoom", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@userID", createRoom.UserID));
                command.Parameters.Add(new SqlParameter("@roomName", createRoom.Name));
                command.Parameters.Add(new SqlParameter("@password", createRoom.Password));

                connection.Open();
                adapter.Fill(table);
                connection.Close();
            }

            if (table.Rows.Count <= 0)
            {
                return new CreateRoomResponse() { Result = false };
            }

            return new CreateRoomResponse()
            {
                Result = true,
                RoomID = table.Rows[0].Field<Guid>("RoomID")
            };
        }

        [HttpGet]
        [Route("Room/LeaveRoom/{userID}")]
        public void LeaveRoom(Guid userID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("Client.LeaveRoom", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@userID", userID));

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}

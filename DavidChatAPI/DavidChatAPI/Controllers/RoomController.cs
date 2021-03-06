﻿using System;
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
    [RoutePrefix("api/Room")]
    public class RoomController : ApiController
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;

        [HttpGet]
        [Route("GetRooms")]
        public IEnumerable<Room> GetRooms()
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


            var rooms = new List<Room>();
            foreach (DataRow row in table.Rows)
            {
                rooms.Add(new Room() { Name = row["Name"].ToString() });
            }

            return rooms;
        }
        
        [HttpPost]
        [Route("JoinRoom")]
        public Room JoinRoom([FromBody] RoomInfo joinRoom)
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
                return null;
            }
            
            return new Room()
            {
                RoomID = table.Rows[0].Field<Guid>("RoomID")
            };
        }

        [HttpPost]
        [Route("GetUsers")]
        public IEnumerable<User> GetUsers([FromBody] Room room)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("Client.GetUsers", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@roomID", room.RoomID));

                connection.Open();
                adapter.Fill(table);
                connection.Close();
            }


            if (table.Rows.Count == 0)
            {
                return null;
            }

            var users = new List<User>();
            foreach (DataRow row in table.Rows)
            {
                users.Add(new User() { Name = row.Field<string>("Name"), PublicID = row.Field<int>("PublicID"), Color = row.Field<ConsoleColor>("Color") });
            }

            return users;
        }

        [HttpPost]
        [Route("CreateRoom")]
        public Room CreateRoom([FromBody] RoomInfo roomInfo)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("Client.CreateRoom", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@userID", roomInfo.UserID));
                command.Parameters.Add(new SqlParameter("@roomName", roomInfo.Name));
                command.Parameters.Add(new SqlParameter("@password", roomInfo.Password));

                connection.Open();
                adapter.Fill(table);
                connection.Close();
            }

            if (table.Rows.Count <= 0)
            {
                return null;
            }

            return new Room()
            {
                RoomID = table.Rows[0].Field<Guid>("RoomID")
            };
        }

        [HttpGet]
        [Route("LeaveRoom/{userID}")]
        public IHttpActionResult LeaveRoom(Guid userID)
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

            return Ok();
        }
    }
}

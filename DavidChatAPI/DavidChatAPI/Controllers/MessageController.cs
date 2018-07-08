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
    [RoutePrefix("api/Message")]
    public class MessageController : ApiController
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;

        [HttpPost]
        [Route("SendMessage")]
        public IHttpActionResult SendMessage([FromBody]SendMessage sendMessage)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("Client.SendMessage", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@userID", sendMessage.UserID));
                command.Parameters.Add(new SqlParameter("@roomID", sendMessage.RoomID));
                command.Parameters.Add(new SqlParameter("@text", sendMessage.text));

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            return Ok();
        }


        [HttpPost]
        [Route("GetNewMessages")]
        public IEnumerable<Message> GetNewMessages([FromBody] Room room)
        {
            DataTable table = new DataTable();
            List<Message> messages = new List<Message>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("Client.SendMessage", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@roomID", room.RoomID));

                connection.Open();
                adapter.Fill(table);
                connection.Close();
            }

            foreach (DataRow row in table.Rows)
            {
                messages.Add(new Message()
                {
                    UserPublicID = row.Field<int>("PublicID"),
                    MessageID = row.Field<int>("MessageID"),
                    Text = row.Field<string>("Text"),
                    Time = row.Field<DateTime>("Time")
                });
            }

            return messages;
        }
    }
}

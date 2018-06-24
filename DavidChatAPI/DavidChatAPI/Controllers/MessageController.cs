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
using WebChat.Models;

namespace DavidChatAPI.Controllers
{
    public class MessageController : ApiController
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;

        [HttpPost]
        [Route("Message/SendMessage")]
        public void SendMessage([FromBody]SendMessage sendMessage)
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
        }


        [HttpPost]
        [Route("Message/GetNewMessages")]
        public GetMessageResponse GetNewMessages([FromBody] Guid roomID)
        {
            DataTable table = new DataTable();
            List<ReceivedMessage> messages = new List<ReceivedMessage>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("Client.SendMessage", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@roomID", roomID));

                connection.Open();
                adapter.Fill(table);
                connection.Close();
            }

            foreach (DataRow row in table.Rows)
            {
                messages.Add(new ReceivedMessage()
                {
                    UserPublicID = row.Field<int>("PublicID"),
                    MessageID = row.Field<int>("MessageID"),
                    Text = row.Field<string>("Text"),
                    Time = row.Field<DateTime>("Time")
                });
            }

            if(messages.Count <= 0)
            {
                return new GetMessageResponse() { Messages = messages.ToArray(), Result = false };
            }
            
            return new GetMessageResponse() { Messages = messages.ToArray(), Result = true};
        }
    }
}

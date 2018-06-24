using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Net;
    using System.Web;
    using WebChat.Models;

    public class Message
    {
        public string Text;
        public User User;
        public DateTime Time;
        public Message(string text, User user, DateTime time)
        {
            Text = text;
            User = user;
            Time = time;
        }
    }
    public class User
    {
        public string Name;
        public Guid? Guid;
        public ConsoleColor Color;

        public User(string name, ConsoleColor color)
        {
            Name = name;
            Color = color;
        }
        public User(string name, ConsoleColor color, Guid guid)
        {
            Name = name;
            Color = color;
            Guid = guid;
        }
    }
    public class Room
    {
        public string Name;
        public string Password;
        public Guid Guid;

        public Room(string name, string password, Guid guid)
        {
            Name = name;
            Password = password;
            Guid = guid;
        }
    }
    public class SQLManager
    {
        public User User;
        public Room Room;

        private string baseUri;
        private SqlConnection connection;
        private Dictionary<int, User> users;
        Dictionary<int, Message> messages;

        public SQLManager(string baseUri)
        {
            messages = new Dictionary<int, Message>();
            users = new Dictionary<int, User>();
            this.baseUri = baseUri;
            connection = new SqlConnection(baseUri);
        }

        public bool Register()
        {
            using (WebClient client = new WebClient())
            {
                client.BaseAddress = baseUri;
                RegisterResponse response = (RegisterResponse)JsonConvert.DeserializeObject(client.DownloadString("User/Register"));

                if (!response.Result)
                {
                    return false;
                }

                User.Guid = response.UserID;
            }
            return true;
        }

        public void GetUsers()
        {
            var command = new SqlCommand("client.GetUsers", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@roomID", Room.Guid));

            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                try
                {
                    DataTable table = new DataTable();
                    connection.Open();

                    adapter.Fill(table);

                    users = new Dictionary<int, User>();
                    foreach (DataRow row in table.Rows)
                    {
                        users.Add(row.Field<int>("PublicID"), new User(row.Field<string>("Name"), (ConsoleColor)row.Field<int>("Color")));
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    connection.Close();
                }
            }
        }

        public bool JoinRoom(string name, string password)
        {
            var joinCommand = new SqlCommand("client.JoinRoom", connection);
            joinCommand.CommandType = System.Data.CommandType.StoredProcedure;
            joinCommand.Parameters.Add(new SqlParameter("@userID", User.Guid));
            joinCommand.Parameters.Add(new SqlParameter("@roomName", name));
            joinCommand.Parameters.Add(new SqlParameter("@password", password));

            bool result = false;
            using (SqlDataAdapter adapter = new SqlDataAdapter(joinCommand))
            {
                try
                {
                    connection.Open();

                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    Room = new Room(name, password, table.Rows[0].Field<Guid>("RoomID"));

                    connection.Close();
                    result = true;
                }
                catch (Exception ex)
                {
                    connection.Close();
                    result = false;
                }
            }
            return result;
        }

        public bool CreateRoom(string name, string password)
        {
            var createCommand = new SqlCommand("client.CreateRoom", connection);
            createCommand.CommandType = System.Data.CommandType.StoredProcedure;
            createCommand.Parameters.Add(new SqlParameter("@userID", User.Guid));
            createCommand.Parameters.Add(new SqlParameter("@name", name));
            createCommand.Parameters.Add(new SqlParameter("@password", password));

            bool result = false;
            using (SqlDataAdapter adapter = new SqlDataAdapter(createCommand))
            {
                try
                {
                    connection.Open();

                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    Room = new Room(name, password, table.Rows[0].Field<Guid>("RoomID"));

                    connection.Close();
                    result = true;
                }
                catch (Exception ex)
                {
                    connection.Close();
                    result = false;
                }
            }
            return result;
        }

        public void LeaveRoom()
        {
            var roomsCommand = new SqlCommand("client.LeaveRoom", connection);
            roomsCommand.CommandType = System.Data.CommandType.StoredProcedure;
            roomsCommand.Parameters.Add(new SqlParameter("@userID", User.Guid));

            try
            {
                connection.Open();
                roomsCommand.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
            }
        }

        public string[] GetRooms()
        {
            var roomsCommand = new SqlCommand("client.GetRooms", connection);
            roomsCommand.CommandType = System.Data.CommandType.StoredProcedure;

            SqlDataAdapter adapter = new SqlDataAdapter(roomsCommand);
            DataTable table = new DataTable();
            adapter.Fill(table);

            List<string> rooms = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                rooms.Add(row.Field<string>("Name").Trim());
            }

            return rooms.ToArray();
        }

        public bool SendMessage(string message)
        {
            var roomsCommand = new SqlCommand("client.SendMessage", connection);
            roomsCommand.CommandType = System.Data.CommandType.StoredProcedure;
            roomsCommand.Parameters.Add(new SqlParameter("@userID", User.Guid));
            roomsCommand.Parameters.Add(new SqlParameter("@roomID", Room.Guid));
            roomsCommand.Parameters.Add(new SqlParameter("@text", message));

            try
            {
                connection.Open();
                roomsCommand.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                connection.Close();
                return false;
            }
        }

        public Message[] GetNewMessages()
        {
            var updateCommand = new SqlCommand("client.GetMessages", connection);
            updateCommand.CommandType = System.Data.CommandType.StoredProcedure;
            updateCommand.Parameters.Add(new SqlParameter("@roomID", Room.Guid));

            using (SqlDataAdapter adapter = new SqlDataAdapter(updateCommand))
            {
                try
                {
                    DataTable table = new DataTable();
                    connection.Open();

                    adapter.Fill(table);

                    var newMessages = new List<Message>();
                    foreach (DataRow row in table.Rows)
                    {
                        if (!users.ContainsKey(row.Field<int>("PublicID")))
                        {
                            connection.Close();
                            GetUsers();
                        }
                        if (!messages.ContainsKey(row.Field<int>("MessageID")))
                        {
                            var msg = new Message(row.Field<string>("Text"), users[row.Field<int>("PublicID")], row.Field<DateTime>("Time"));

                            messages.Add(row.Field<int>("MessageID"), msg);
                            newMessages.Add(msg);
                        }
                    }

                    connection.Close();
                    return newMessages.ToArray();
                }
                catch (Exception ex)
                {
                    connection.Close();
                }
            }
            return new Message[0];
        }
    }
}

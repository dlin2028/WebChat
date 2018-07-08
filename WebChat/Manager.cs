﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat
{
    using DavidChatAPI.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;

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
    public class Room
    {
        public string Name;
        public string Password;
        public Guid? Guid;

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
        private Dictionary<int, User> users;
        Dictionary<int, Message> messages;

        public SQLManager(string baseUri)
        {
            messages = new Dictionary<int, Message>();
            users = new Dictionary<int, User>();
            this.baseUri = baseUri;
        }

        public bool Register()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUri + $"User/Register/{User.Guid}/{(int)User.Color}");
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                var output = JsonConvert.DeserializeObject<User>(reader.ReadToEnd());
                User.Guid = output.Guid;
            }

            return true;
        }

        public void GetUsers()
        {
            using (WebClient client = new WebClient())
            {
                client.BaseAddress = baseUri;
                List<User> userList = JsonConvert.DeserializeObject<List<User>>(client.DownloadString("User/GetUsers"));

                foreach (var user in userList)
                {
                    users.Add((int)user.PublicID, user);
                }
            }
        }

        public bool JoinRoom(string name, string password)
        {
            using (WebClient client = new WebClient())
            {
                client.BaseAddress = baseUri;

                RoomInfo roomInfo = new RoomInfo()
                {
                    UserID = User.Guid,
                    Name = name,
                    Password = password
                };

                Room response = JsonConvert.DeserializeObject<Room>(client.UploadString("Room/JoinRoom", JsonConvert.SerializeObject(roomInfo)));
           
                if (response == null)
                {
                    return false;
                }

                Room.Guid = response.Guid;
            }
            return true;
        }

        public bool CreateRoom(string name, string password)
        {
            using (WebClient client = new WebClient())
            {
                client.BaseAddress = baseUri;

                RoomInfo roomInfo = new RoomInfo()
                {
                    UserID = User.Guid,
                    Name = name,
                    Password = password
                };

                Room response = JsonConvert.DeserializeObject<Room>(client.UploadString("Room/CreateRoom", JsonConvert.SerializeObject(roomInfo)));

                if (response == null)
                {
                    return false;
                }

                Room.Guid = response.Guid;
            }
            return true;
        }

        public void LeaveRoom()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUri + $"Room/LeaveRoom/{User.Guid}");
            request.Method = "GET";
            request.GetResponse();
        }

        public string[] GetRooms()
        {
            using (WebClient client = new WebClient())
            {
                client.BaseAddress = baseUri;
                List<Room> roomList = JsonConvert.DeserializeObject<List<Room>>(client.DownloadString("Room/GetRooms"));

                string[] names = new string[roomList.Count];
                for (int i = 0; i < roomList.Count; i++)
                {
                    names[i] = roomList[i].Name.Trim();
                }
                return names;
            }
        }

        public bool SendMessage(string text)
        {
            using (WebClient client = new WebClient())
            {
                SendMessage message = new SendMessage()
                {
                    UserID = User.Guid,
                    RoomID = (Guid)Room.Guid,
                    text = text
                };

                client.BaseAddress = baseUri;
                client.UploadString("Room/CreateRoom", JsonConvert.SerializeObject(message));
            }
            return true;
        }

        public Message[] GetNewMessages()
        {
            using (WebClient client = new WebClient())
            {
                client.BaseAddress = baseUri;
                List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(client.UploadString("User/GetNewMessages", JsonConvert.SerializeObject(Room)));

                return messages.ToArray();
            }
        }
    }
}

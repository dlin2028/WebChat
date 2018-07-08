using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DavidChatAPI.Models
{
    public class Message
    {
        public string Text;
        public int UserPublicID;
        public int MessageID;
        public DateTime Time;
        public string UserName;
        public User User;
    }
}
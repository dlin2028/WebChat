using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DavidChatAPI.Models
{
    public class SendMessage
    {
        public Guid UserID;
        public Guid RoomID;
        public string text;
    }
}
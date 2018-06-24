using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DavidChatAPI.Models
{
    public class CreateRoom
    {
        public Guid UserID;
        public string Name;
        public string Password;
    }
}
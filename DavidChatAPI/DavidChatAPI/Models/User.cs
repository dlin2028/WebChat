using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DavidChatAPI.Models
{
    public class User
    {
        public string Name;
        public int? PublicID;
        public Guid Guid;
        public ConsoleColor Color;
    }
}
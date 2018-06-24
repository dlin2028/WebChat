using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DavidChatAPI.Models
{
    public class Room
    {
        public string Name { get; set; }
        public Guid? RoomID { get; set; }
    }
}
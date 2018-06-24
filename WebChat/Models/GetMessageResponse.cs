using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Models
{
    public class ReceivedMessage
    {
        public string Text;
        public int UserPublicID;
        public int MessageID;
        public DateTime Time;
    }
    public class GetMessageResponse
    {
        public bool Result;
        public ReceivedMessage[] Messages;
    }
}
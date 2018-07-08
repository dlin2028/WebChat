using DavidChatAPI.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebChat
{
    public partial class ChatForm : System.Web.UI.Page
    {
        SQLManager manager;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["manager"] == null)
            {
                Server.Transfer("Signup.aspx", false);
            }
            manager = (SQLManager)Session["manager"];

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            manager.SendMessage(msgBox.Text);
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Message[] messages = manager.GetNewMessages();
            foreach (Message msg in messages)
            {
                ListBox1.Items.Insert(0,new ListItem("[" + msg.User.Color.ToString() + "]" + msg.User.Name + "(" + msg.Time.ToString("M/d/yyyy hh:mm tt") + ") :" + msg.Text));
            }
            ListBox1.SelectedIndex = ListBox1.Items.Count - 1;
        }
        protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
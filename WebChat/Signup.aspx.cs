using DavidChatAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebChat
{
    public partial class Signup : System.Web.UI.Page
    {
        SQLManager manager;
        string[] rooms;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["manager"] == null)
            {
                //SQLManager manager = new SQLManager("Data Source = GMRMLTV; Initial Catalog = DavidChat; User ID = sa; Password = GreatMinds110;");
                Session["manager"] = new SQLManager("http://localhost:53778/api/");
            }
            manager = (SQLManager)Session["manager"];

            if(!IsPostBack)
            {
                string[] colors = Enum.GetNames(typeof(ConsoleColor)).ToArray();
                foreach (string color in colors)
                {
                    colorList.Items.Add(color);
                }

                roomList.Items.Add("");
                rooms = manager.GetRooms();
                Session["rooms"] = rooms;
                foreach (var name in rooms)
                {
                    roomList.Items.Add(name.Trim());
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            manager.User = new User()
            {
                Name = userBox.Text,
                Color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorList.Text)
            };

            manager.Register();

            if (((string[])Session["rooms"]).Contains(roomBox.Text))
            {
                if (manager.JoinRoom(roomBox.Text, pwBox.Text))
                {
                    Server.Transfer("ChatForm.aspx", false);
                }
            }
            else
            {
                if (manager.CreateRoom(roomBox.Text, pwBox.Text))
                {
                    Server.Transfer("ChatForm.aspx", false);
                }
            }
            outLabel.Text = "you fail";
        }

        protected void roomList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(roomList.Text != "")
            {
                roomBox.Text = roomList.Text;
            }
            roomList.Items.Clear();

            roomList.Items.Add("");
            rooms = manager.GetRooms();
            Session["rooms"] = rooms;
            foreach (var name in rooms)
            {
                roomList.Items.Add(name.Trim());
            }
        }
    }
}
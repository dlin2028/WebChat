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
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SQLManager manager = new SQLManager(new User(TextBox1.Text, 0), "Data Source = GMRMLTV; Initial Catalog = DavidChat; User ID = sa; Password = GreatMinds110;");
            manager.Register();
            manager.JoinRoom(TextBox3.Text, TextBox4.Text);
        }
    }
}
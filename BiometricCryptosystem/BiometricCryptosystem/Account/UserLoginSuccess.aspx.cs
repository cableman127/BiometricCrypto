using BiometricCryptosystem.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BiometricCryptosystem.Account
{
    public partial class UserLoginSuccess : System.Web.UI.Page
    {
        tblUserInfo user;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                user = (tblUserInfo)Session["User"];
                if (user == null)
                {
                    Response.Redirect("~/Account/Login.aspx");
                    return;
                }
                MyDataClassesDataContext wcontext = new MyDataClassesDataContext();
                tblUserInfo.printUserData(wcontext, user);
                Byte[] face = tblUserInfo.getFace(wcontext, user);
                string base64String2 = Convert.ToBase64String(face, 0, face.Length);
                Image1.ImageUrl = "data:image/jpeg;base64," + base64String2;

                string key = tblUserInfo.getKey(wcontext, user);
                Label1.Text = "Key: " + key;
            }
        }
    }
}
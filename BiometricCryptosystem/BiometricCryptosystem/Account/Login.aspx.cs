using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using BiometricCryptosystem.Models;
using BiometricCryptosystem.DB;
using System.Diagnostics;

namespace BiometricCryptosystem.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register.aspx";

            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }


            if (!IsPostBack)
            {
                tblUserInfo user = (tblUserInfo)Session["User"];

            }

        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            MyDataClassesDataContext wcontext = new MyDataClassesDataContext();
            if ((tblUserInfo)Session["User"] == null)
            {
                Debug.WriteLine(LoginUser.UserName + " " + LoginUser.Password);
                tblUserInfo user = tblUserInfo.ValidateLogin(wcontext, LoginUser.UserName.Trim(), LoginUser.Password.Trim());
                Session["User"] = user;

                //Debug.WriteLine(user.UserRole);

                if (user == null)
                {
                    tblUserInfo.loginFailed(wcontext, LoginUser.UserName);
                    Response.Write("Invalid Login");
                    Debug.WriteLine("Invalid Login");
                }
                else
                {
                    Session["User"] = user;
                    if (tblUserInfo.isUserBiometricEnrolled(wcontext, user) == true)
                    {
                        Debug.WriteLine(user.ID + " " + user.Password + " ");
                        Response.Redirect("~/Account/EnrollTwo-FactorAuthentication.aspx");
                        return;
                    }
                    else
                    {

                        Response.Redirect("~/Account/Two-FactorAuthenticationSignIn.aspx");
                        return;
                    }
                }
                
            }
            else
            {
                tblUserInfo user = (tblUserInfo)Session["User"];
                if (tblUserInfo.isUserBiometricEnrolled(wcontext, user) == true)
                {
                    Debug.WriteLine(user.ID + " " + user.Password + " ");
                    Response.Redirect("~/Account/EnrollTwo-FactorAuthentication.aspx");
                    return;
                }
                else
                {
                    Response.Redirect("~/Account/Two-FactorAuthenticationSignIn.aspx");
                    return;
                }

            }
        }

    }
}


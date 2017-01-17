using BiometricCryptosystem.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using MLApp;
using System.Collections;
using System.Net;
using System.Drawing;

namespace BiometricCryptosystem.Account
{
    public partial class Two_FactorAuthenticationSignIn : Page
    {
        private MLApp.MLApp matlab;
        private Stopwatch stopwatch;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tblUserInfo user = (tblUserInfo)Session["User"];
                MyDataClassesDataContext wcontext = new MyDataClassesDataContext();
                tblUserInfo.printUserData(wcontext, user);



            }
            log.Text = "";
            matlabLog.Text = "";
            log.Text += "Creating MATLAB object\n";
            matlab = new MLApp.MLApp();
            log.Text += "MATLAB object created\n";
            log.Text += "Creating Stopwatch object\n";
            stopwatch = new Stopwatch();
            log.Text += "Stopwatch object created\n";
            //!!!!!!!!! INPUT YOUR OWN WORKING FOLDER AND MATLABDIR HERE
            log.Text += "Changing MATLAB working dir to C:\\Users\\Tri Le\\Documents\\CS656\\Project\n";
            matlab_Command("cd('C:\\Users\\Tri Le\\Documents\\CS656\\Project')");
            matlab_Command("pwd");

        }
        public String matlab_Command(String cmd)

        {

            stopwatch.Start();
            String ans = matlab.Execute(cmd);
            stopwatch.Stop();
            matlabLog.Text += ">> " + cmd + " [ " + stopwatch.ElapsedMilliseconds.ToString() + " ]\n";
            stopwatch.Reset();
            matlabLog.Text += "\n" + ans;
            return ans;

        }




        protected void UploadBtn_Click(object sender, EventArgs e)
        {

            MyDataClassesDataContext wcontext = new MyDataClassesDataContext();
            tblUserInfo user = (tblUserInfo)Session["User"];
            tblUserInfo.printUserData(wcontext, user);
            //server side checking
            if (FileUpload1.PostedFile.ContentType.ToLower().StartsWith("image") &&
            FileUpload1.HasFile && Page.IsValid)
            {
                Stream imgStream = FileUpload1.PostedFile.InputStream;
                BinaryReader br = new BinaryReader(imgStream);
                Byte[] bytes = br.ReadBytes((Int32)imgStream.Length);

 
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                Image1.ImageUrl = "data:image/jpeg;base64," + base64String;
                Image checkIM = byteArrayToImage(bytes);
                checkIM.Save("C:\\Users\\Tri Le\\Documents\\CS656\\Project\\checkIM.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            }

        }
        protected Byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            Byte[] bytes = ms.ToArray();
            return bytes;
        }

        public Image byteArrayToImage(Byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }


        protected void MatlabBtn_Click(object sender, EventArgs e)
        {
            MyDataClassesDataContext wcontext = new MyDataClassesDataContext();
            tblUserInfo user = (tblUserInfo)Session["User"];
            matlabLog.Text += "\n Matlab Button Clicked";
            matlab_Command("authenticateUser");
            var key = matlab.GetVariable("Key", "base");
            var isPassAuthenticator = matlab.GetVariable("passAuthentication", "base");


            if (isPassAuthenticator == true)

            {
                tblUserInfo userValidated = tblUserInfo.authenticateKey(wcontext, user, (int)key);
                if (userValidated != null)
                {
                    Session["User"] = userValidated;
                    Response.Redirect("~/Account/UserLoginSuccess.aspx");
                    return;
                }
                else
                {
                    Label1.Visible = true;
                }

            }
        }
    }
}
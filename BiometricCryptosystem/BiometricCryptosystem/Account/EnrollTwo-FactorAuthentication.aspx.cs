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
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace BiometricCryptosystem.Account
{
    public partial class EnrollTwo_FactorAuthentication : Page
    {
        private MLApp.MLApp matlab;
        private Stopwatch stopwatch;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tblUserInfo user = (tblUserInfo)Session["User"];
                if (user == null)
                {
                    Response.Redirect("~/Account/Login.aspx");
                    return;
                }
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

        Image GetImage(int width, int height, string filename)
        {
            //Assume the data is entire row of indice
            var fileData = File.ReadAllBytes(filename);

            var bmp = new Bitmap(width, height, PixelFormat.DontCare);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bmp.PixelFormat);

            var ptr = bmpData.Scan0;

            Marshal.Copy(fileData, 0, ptr, fileData.Length);
            bmp.UnlockBits(bmpData);

            return bmp;
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
            matlabLog.Text += "\n" + tblUserInfo.printUserData(wcontext, user);
            //server side checking
            if (FileUpload1.PostedFile.ContentType.ToLower().StartsWith("image") &&
            FileUpload1.HasFile && Page.IsValid)
            {
                Stream imgStream = FileUpload1.PostedFile.InputStream;
                BinaryReader br = new BinaryReader(imgStream);
                Byte[] bytes = br.ReadBytes((Int32)imgStream.Length);

                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                Image1.ImageUrl = "data:image/jpeg;base64," + base64String;
                Image uploadedIM = byteArrayToImage(bytes);
                uploadedIM.Save("C:\\Users\\Tri Le\\Documents\\CS656\\Project\\uploadedIM.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                user.EnrollFace = bytes;
                user.Password = "master123";
                Session["User"] = user;
                tblUserInfo.printUserData(wcontext, user);
                Debug.WriteLine("Debug: " + user.Password);
                bool RowsAffected = tblUserInfo.EnrollAuthentication(wcontext, user);
                if (RowsAffected == true)
                {
                    Response.Write("< BR > The Image was saved");
                }
                else
                {
                    Response.Write("< BR > An error occurred uploading the image");
                }
                //Byte[] face = tblUserInfo.getFace(wcontext, user);
                //string base64String2 = Convert.ToBase64String(face, 0, face.Length);
                //Image2.ImageUrl = "data:image/jpeg;base64," + base64String2;

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
            matlab_Command("enrollUser");
            var a = matlab.GetVariable("Key", "base");
            user.PassKey = a.ToString();
            bool isPass = tblUserInfo.UpdateRecord(wcontext, user);
            if (isPass == true)
            {
                Session["User"] = user;
                Response.Redirect("~/Account/Two-FactorAuthenticationSignIn.aspx");
            }
            else
            {
                Label1.Visible = true;
            }

        }
    }
}
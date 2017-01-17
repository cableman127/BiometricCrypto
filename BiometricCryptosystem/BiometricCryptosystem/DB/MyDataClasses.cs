using System.Data.SqlClient;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
namespace BiometricCryptosystem.DB
{
    partial class MyDataClassesDataContext
    {

        public static string ConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            }
        }
        public static SqlConnection DBConnection
        {
            get
            {
                return new SqlConnection(ConnectionString);
            }
        }
        public static MyDataClassesDataContext Context
        {
            get
            {
                return new MyDataClassesDataContext(DBConnection);
            }
        }

    }


    public partial class tblUserInfo
    {

        public static bool AddUpdateRecord(MyDataClassesDataContext context, tblUserInfo rec)
        {
            MyDataClassesDataContext wcontext = (context == null ? MyDataClassesDataContext.Context : context);
            tblUserInfo existingRec = ReadRecord(wcontext, rec.ID);
            if (existingRec == null) //new record
            {
                return InsertRecord(wcontext, rec);
            }
            else //found existing, update
            {
                return UpdateRecord(wcontext, rec);
            }

        }
        public static bool DeleteRecord(MyDataClassesDataContext context, int ID)
        {
            try
            {
                MyDataClassesDataContext wcontext = (context == null ? MyDataClassesDataContext.Context : context);
                tblUserInfo existingRec = ReadRecord(wcontext, ID);
                if (existingRec != null) //there is a record
                {
                    wcontext.tblUserInfos.DeleteOnSubmit(existingRec);
                    wcontext.SubmitChanges();
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertRecord(MyDataClassesDataContext context, tblUserInfo rec)
        {
            try
            {
                MyDataClassesDataContext wcontext = (context == null ? MyDataClassesDataContext.Context : context);
                tblUserInfo existingRec = ReadRecord(wcontext, rec._ID);
                if (existingRec == null)
                {
                    string checkName = checkLoginName(wcontext, rec);
                    rec.UserName = checkName;
                    //Change password rules
                    rec.Password = "helloworld";
                    wcontext.tblUserInfos.InsertOnSubmit(rec);
                    wcontext.SubmitChanges();
                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateRecord(MyDataClassesDataContext context, tblUserInfo rec)
        {
            try
            {
                MyDataClassesDataContext wcontext = (context == null ? MyDataClassesDataContext.Context : context);
                tblUserInfo existingRec = ReadRecord(wcontext, rec._ID);
                if (existingRec != null)
                {
                    Serializer.Clone<tblUserInfo>(rec, existingRec);
                    wcontext.SubmitChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static tblUserInfo ReadRecord(MyDataClassesDataContext context, int ID)
        {
            try
            {
                MyDataClassesDataContext wcontext = (context == null ? MyDataClassesDataContext.Context : context);
                var query = from n in wcontext.tblUserInfos
                            where n.ID == ID
                            select n;
                if (query != null && query.Count() > 0)
                {
                    return query.First();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool loginFailed(MyDataClassesDataContext context, string loginName)
        {
            MyDataClassesDataContext wcontext = (context == null ? MyDataClassesDataContext.Context : context);
            var query = from n in wcontext.tblUserInfos
                        where n.UserName == loginName
                        select n;
            //Debug.WriteLine(query.Count());
            while (query != null && query.Count() > 0)
            {
                tblUserInfo existingRec = query.First();
                if (existingRec != null)
                {

                    return true;
                }
            }
            return false;
        }

        public static ArrayList getRecords(MyDataClassesDataContext context)
        {
            try
            {
                MyDataClassesDataContext wcontext = (context == null ? MyDataClassesDataContext.Context : context);
                var query = from n in wcontext.tblUserInfos
                            select n;
                ArrayList list = new ArrayList();
                foreach (var course in query)
                {
                    list.Add(course);
                }
                return list;
                //return query;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static tblUserInfo checkInfo(MyDataClassesDataContext context, string email, string userName)
        {
            try
            {
                MyDataClassesDataContext wcontext = (context == null ? MyDataClassesDataContext.Context : context);
                var query = from n in wcontext.tblUserInfos
                                //where n.Email == email
                            where n.UserName == userName
                            select n;
                if (query != null && query.Count() > 0)
                {
                    return query.First();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static tblUserInfo ValidateLogin(MyDataClassesDataContext context, string loginName, string password)
        {
            try
            {
                MyDataClassesDataContext wcontext = (context == null ? MyDataClassesDataContext.Context : context);
                var query = from n in wcontext.tblUserInfos
                            where n.UserName == loginName
                            where n.Password == password
                            select n;
                if (query != null && query.Count() > 0)
                {
                    return query.First();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool isUserBiometricEnrolled(MyDataClassesDataContext context, tblUserInfo user)
        {
            try
            {
                MyDataClassesDataContext wcontext = (context == null ? MyDataClassesDataContext.Context : context);
                var query = from n in wcontext.tblUserInfos
                            where n.ID == user.ID
                            where n.EnrollFace == null
                            where n.PassKey == null
                            select n;
                if (query != null && query.Count() > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool EnrollAuthentication(MyDataClassesDataContext context, tblUserInfo user)
        {
            try
            {
                MyDataClassesDataContext wcontext = (context == null ? MyDataClassesDataContext.Context : context);
                tblUserInfo existingRec = ReadRecord(wcontext, user._ID);
                if (existingRec != null)
                {
                    Serializer.Clone<tblUserInfo>(user, existingRec);
                    wcontext.SubmitChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] VerifyAuthentication(MyDataClassesDataContext context, tblUserInfo user)
        {
            try
            {
                MyDataClassesDataContext wcontext = (context == null ? MyDataClassesDataContext.Context : context);
                var query = from n in wcontext.tblUserInfos
                            where n.UserName == user.UserName
                            where n.Password == user.Password
                            where n.EnrollFace == user.LoginFace
                            select n.EnrollFace.ToArray();
                if (query != null && query.Count() > 0)
                {
                    return query.First();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Byte[] getFace(MyDataClassesDataContext context, tblUserInfo user)
        {
            try
            {
                MyDataClassesDataContext wcontext = (context == null ? MyDataClassesDataContext.Context : context);
                var query = from n in wcontext.tblUserInfos
                            where n.UserName == user.UserName
                            where n.Password == user.Password
                            select n.EnrollFace.ToArray();
                if (query != null && query.Count() > 0)
                {
                    return query.First();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string getKey(MyDataClassesDataContext context, tblUserInfo user)
        {
            try
            {
                MyDataClassesDataContext wcontext = (context == null ? MyDataClassesDataContext.Context : context);
                var query = from n in wcontext.tblUserInfos
                            where n.ID == user.ID
                            select n.PassKey.ToString();
                if (query != null && query.Count() > 0)
                {
                    return query.First();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string checkLoginName(MyDataClassesDataContext context, tblUserInfo rec)
        {
            int incrementer = 0;
            string checkName = rec.UserName.ToString();
            MyDataClassesDataContext wcontext = (context == null ? MyDataClassesDataContext.Context : context);
            var query = from n in wcontext.tblUserInfos
                        where n.UserName == checkName
                        select n;
            Debug.WriteLine(query.Count());
            while (query != null && query.Count() > 0)
            {
                incrementer++;
                checkName = rec.UserName.ToString();
                query = from n in wcontext.tblUserInfos
                        where n.UserName == checkName
                        select n;
            }
            return checkName;
        }

        public static string printUserData(MyDataClassesDataContext context, tblUserInfo rec)
        {

            MyDataClassesDataContext wcontext = (context == null ? MyDataClassesDataContext.Context : context);
            var query = from n in wcontext.tblUserInfos
                        where n.ID == rec.ID
                        select n;
            string data;
            tblUserInfo existingRec = ReadRecord(wcontext, rec._ID);
            data = "ID: " + existingRec.ID.ToString() + " Name: " + existingRec.UserName.ToString() + " Pass: " + existingRec.Password.ToString();

            Debug.WriteLine(data);

            return data;
        }

        public static tblUserInfo authenticateKey(MyDataClassesDataContext context, tblUserInfo rec, int key)
        {
            try
            {
                MyDataClassesDataContext wcontext = (context == null ? MyDataClassesDataContext.Context : context);
                var query = from n in wcontext.tblUserInfos
                            where n.ID == rec.ID
                            where n.PassKey == key.ToString()
                            select n;
                if (query != null && query.Count() > 0)
                {
                    return query.First();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
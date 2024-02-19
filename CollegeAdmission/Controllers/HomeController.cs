using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollegeAdmission.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using WebGrease;
using System.Reflection;
using System.Web.Helpers;
using System.Text;
using System.Security.Cryptography;
using System.Drawing;

namespace CollegeAdmission.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult SignIn()
        {
            return View();
        }

        /// <summary>
        ///  Login for admin and applicants
        /// </summary>
        /// <param name="uc"></param>
        /// <returns> The logged in dashboard for user or admin </returns>
        [HttpPost]
        public ActionResult SignIn(UserClass userClass)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("[dbo].[Login]", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Username", userClass.Username);
                sqlCommand.Parameters.AddWithValue("@Password", ((Encrypt(userClass.Password))));
                SqlDataReader sdr = sqlCommand.ExecuteReader();
                if (sdr.Read())
                {
                    if (userClass.Username == "admin@gmail.com" && userClass.Password == Decrypt(Encrypt(userClass.Password)))
                    {
                        Session["username"] = userClass.Username.ToString();
                        return RedirectToAction("AdminDashboard", "Admin");
                    }
                    else if (userClass.Username != "admin@gmail.com" && userClass.Password == Decrypt(Encrypt(userClass.Password)))
                    {
                        Session["username"] = userClass.Username.ToString();
                        return RedirectToAction("UserDashboard", "User");
                    }
                    else
                    {
                        ViewData["MessageLogin"] = "Login details failed";
                    }
                }
                else
                {
                    ViewData["MessageLogin"] = "Login details failed";
                }
                sqlConnection.Close();
                return View();
            }
            catch(Exception)
            { 
                return View(); 
            }
        }

        public ActionResult SignUp()
        {
            return View();
        }

        /// <summary>
        /// Registration for applicants
        /// </summary>
        /// <param name="uc"></param>
        /// <returns> A successful registration </returns>
        [HttpPost]
        public ActionResult SignUp(UserClass userClass)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("[dbo].[Register]", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@FirstName", userClass.FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", userClass.LastName);
                sqlCommand.Parameters.AddWithValue("@DateOfBirth", userClass.DateOfBirth);
                sqlCommand.Parameters.AddWithValue("@Age", userClass.Age);
                sqlCommand.Parameters.AddWithValue("@Gender", userClass.Gender);
                sqlCommand.Parameters.AddWithValue("@FatherName", userClass.FatherName);
                sqlCommand.Parameters.AddWithValue("@MotherName", userClass.MotherName);
                sqlCommand.Parameters.AddWithValue("@PhoneNumber", userClass.PhoneNumber);
                sqlCommand.Parameters.AddWithValue("@Email", userClass.Email);
                sqlCommand.Parameters.AddWithValue("@Address", userClass.Address);
                sqlCommand.Parameters.AddWithValue("@State", userClass.State);
                sqlCommand.Parameters.AddWithValue("@District", userClass.District);
                sqlCommand.Parameters.AddWithValue("@School", userClass.School);
                sqlCommand.Parameters.AddWithValue("@Marks", userClass.Marks);
                sqlCommand.Parameters.AddWithValue("@Course", userClass.Course);
                sqlCommand.Parameters.AddWithValue("@Department", userClass.Department);
                sqlCommand.Parameters.AddWithValue("@Username", userClass.Username);
                sqlCommand.Parameters.AddWithValue("@Password", Encrypt(userClass.Password));
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                ViewData["Message"] = "User Record " + userClass.Username + " is saved successfully!";
                ModelState.Clear();
                return View();
            }
            catch(Exception)
            {
                return View();
            }
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        /// <summary>
        /// Password reset for users
        /// </summary>
        /// <param name="uc"></param>
        /// <returns> A password updation for users </returns>
        [HttpPost]
        public ActionResult ResetPassword(UserClass userClass)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlcomm = new SqlCommand("[dbo].[Reset_Password]", sqlConnection);
                sqlcomm.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                sqlcomm.Parameters.AddWithValue("@Email", userClass.Email);
                sqlcomm.Parameters.AddWithValue("@Password", Encrypt(userClass.Password));
                sqlcomm.ExecuteNonQuery();
                sqlConnection.Close();
                ViewData["MessageUpdate"] = "User password changed successfully for " + userClass.Email;
                return View();
            }
            catch(Exception)
            {
                return View();
            }
        }

        /// <summary>
        /// Password encryption function using AES algorithm
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns> Encrypted password stored in the database</returns>
        private static string Encrypt(string clearText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        /// <summary>
        /// Password decryption function during login
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns> Encrypted password decrypted during login</returns>
        private static string Decrypt(string cipherText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}
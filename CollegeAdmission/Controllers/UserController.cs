using CollegeAdmission.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using System.Runtime.Remoting.Messaging;
using System.Net;
using System.Reflection;
using System.Web.Helpers;
using System.Web.UI;
using System.Collections;
using CollegeAdmission.DAL;

namespace CollegeAdmission.Controllers
{
    public class UserController : Controller
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;       
        /// <summary>
        /// Retrieve the user's profile from the database based on the currently logged-in user
        /// </summary>
        /// <returns>User's account information</returns>
        [HttpGet]
        public ActionResult Account()
        {
            string loggedInUsername = (Session["username"].ToString());
            DataAccessLayer dataAccessLayer = new DataAccessLayer();
            UserClass userClass = new UserClass();
            userClass = dataAccessLayer.GetUser(loggedInUsername);
            if (userClass == null)
            {
                return HttpNotFound();
            }
            return View(userClass);
        }

        [HttpGet]
        public ActionResult Edit(string Username)
        {
            string loggedInUsername = (Session["username"].ToString());
            DataAccessLayer dataAccessLayer = new DataAccessLayer();
            UserClass userClass = new UserClass();
            userClass = dataAccessLayer.GetUser(loggedInUsername);
            return View(userClass);
        }

        /// <summary>
        /// Update the user's profile details
        /// </summary>
        /// <param name="uc"></param>
        /// <returns> Details are updated in the database</returns>
        [HttpPost]
        public ActionResult Edit(UserClass uc)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("[dbo].[Update_UserProfile]", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@FirstName", uc.FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", uc.LastName);
                sqlCommand.Parameters.AddWithValue("@DateOfBirth", uc.DateOfBirth);
                sqlCommand.Parameters.AddWithValue("@Age", uc.Age);
                sqlCommand.Parameters.AddWithValue("@Gender", uc.Gender);
                sqlCommand.Parameters.AddWithValue("@FatherName", uc.FatherName);
                sqlCommand.Parameters.AddWithValue("@MotherName", uc.MotherName);
                sqlCommand.Parameters.AddWithValue("@PhoneNumber", uc.PhoneNumber);
                sqlCommand.Parameters.AddWithValue("@Email", uc.Email);
                sqlCommand.Parameters.AddWithValue("@Address", uc.Address);
                sqlCommand.Parameters.AddWithValue("@State", uc.State);
                sqlCommand.Parameters.AddWithValue("@District", uc.District);
                sqlCommand.Parameters.AddWithValue("@School", uc.School);
                sqlCommand.Parameters.AddWithValue("@Marks", uc.Marks);
                sqlCommand.Parameters.AddWithValue("@Course", uc.Course);
                sqlCommand.Parameters.AddWithValue("@Department", uc.Department);
                sqlCommand.Parameters.AddWithValue("@Username", uc.Username);
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                ViewData["Message1"] = "User Record " + uc.Username + " is updated successfully!";
                return View();
            }
            catch (Exception)
            {
                return View();
            }          
        }

        public ActionResult UserDashboard()
        {
            return View();
        }

        /// <summary>
        /// User's status is updated when the admin accepts or rejects the application
        /// </summary>
        /// <param name="Username"></param>
        /// <returns> Status from the databse</returns>
        public UserClass StatusUpdate(string Username)
        {
            UserClass userClass = new UserClass();
            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);              
                SqlCommand sqlCommand = new SqlCommand("[dbo].[Status]", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Username", Username);

                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    userClass.Status = reader["Status"].ToString();

                    if (userClass.Status == "Approved")
                    {
                        // Get image path
                        string imgPath = Server.MapPath("~/Content/Images/approvednew1.jpg");
                        // Convert image to byte array
                        byte[] byteData = System.IO.File.ReadAllBytes(imgPath);
                        //Convert byte arry to base64string
                        string imreBase64Data = Convert.ToBase64String(byteData);
                        string imgDataURL = string.Format("data:image/jpg;base64,{0}", imreBase64Data);
                        //Passing image data in viewbag to view
                        ViewBag.ImageData = imgDataURL;
                        ViewData["MessageApprove"] = "Congratulations!" +
                            "Your application has been approved." +
                            "Welcome to Government Engineering College, Palakkad.";
                    }
                    else if (userClass.Status == "Rejected")
                    {
                        // Get image path
                        string imgPathReject = Server.MapPath("~/Content/Images/rejectednew1.jpg");
                        // Convert image to byte array
                        byte[] byteDataReject = System.IO.File.ReadAllBytes(imgPathReject);
                        //Convert byte arry to base64string
                        string imreBase64DataReject = Convert.ToBase64String(byteDataReject);
                        string imgDataURLReject = string.Format("data:image/jpg;base64,{0}", imreBase64DataReject);
                        //Passing image data in viewbag to view
                        ViewBag.ImageDataReject = imgDataURLReject;
                        ViewData["MessageReject"] = "We regret to inform you that your application has been rejected. All the best for your future endeavours.";
                    }
                    else
                    {
                        // Get image path
                        string imgPathPending = Server.MapPath("~/Content/Images/pending.jpg");
                        // Convert image to byte array
                        byte[] byteDataPending = System.IO.File.ReadAllBytes(imgPathPending);
                        //Convert byte arry to base64string
                        string imreBase64DataPending = Convert.ToBase64String(byteDataPending);
                        string imgDataURLPending = string.Format("data:image/jpg;base64,{0}", imreBase64DataPending);
                        //Passing image data in viewbag to view
                        ViewBag.ImageDataPending = imgDataURLPending;
                        ViewData["MessagePending"] = "Your application is still under review." +
                            " Our team is working diligently to process and finalize the results as quickly as possible.";
                    }
                }
                sqlConnection.Close();
                return userClass;
            }
            catch(Exception)
            {
                return userClass;
            }
        }

        /// <summary>
        /// Status is viewed in the View
        /// </summary>      
        [HttpGet]
        public ActionResult Status()
        {         
            string loggedInUsername = (Session["username"].ToString());
            UserClass userClass = StatusUpdate(loggedInUsername);
            return View(userClass);
        }

        /// <summary>
        /// User session logout
        /// </summary>      
        [HttpPost]
        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("SignIn", "Home");
        }
    }
}

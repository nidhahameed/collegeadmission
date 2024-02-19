using CollegeAdmission.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web;
using CollegeAdmission.DAL;

namespace CollegeAdmission.Controllers
{
    public class AdminController : Controller
    {
        /// <summary>
        /// Listing all the applicant's details in the admin dashboard
        /// </summary>
        /// <returns> The user's details</returns>
        [HttpGet]
        public List<UserProfile> GetAll()
        {
            List<UserProfile> users = new List<UserProfile>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("[dbo].[Admin_Dash]", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    UserProfile userProfile = new UserProfile();
                    userProfile.UserID = Convert.ToInt32(reader["UserID"]);
                    userProfile.Username = reader["Username"].ToString();
                    userProfile.FirstName = reader["FirstName"].ToString();
                    userProfile.LastName = reader["LastName"].ToString();
                    userProfile.Age = Convert.ToInt32(reader["Age"]);
                    userProfile.Gender = reader["Gender"].ToString();
                    userProfile.PhoneNumber = reader["PhoneNumber"].ToString();
                    userProfile.Email = reader["Email"].ToString();
                    userProfile.Marks = Convert.ToInt32(reader["Marks"]);
                    userProfile.Course = reader["Course"].ToString();
                    userProfile.Department = reader["Department"].ToString();
                    userProfile.Status = reader["Status"].ToString();
                    users.Add(userProfile);
                }
                sqlConnection.Close();
                return users;
            }
            catch (Exception ) 
            {
                return users;
            }
        }

        /// <summary>
        /// Listing all the applicant's details in the admin dashboard
        /// </summary>
        [HttpGet]
        public ActionResult AdminDashboard()
        {
            List<UserProfile> users = new List<UserProfile>();
            users = GetAll();
            ViewBag.model = users;
            return View(ViewBag.model);
        }

        /// <summary>
        /// The details page for user's
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Details(string ID)
        {                     
            DataAccessLayer dataAccessLayer = new DataAccessLayer();
            UserProfile userProfile = new UserProfile();
            userProfile = dataAccessLayer.GetUserProfile(ID);
            return View(userProfile);
        }

        /// <summary>
        /// Accepting the user's application by updating the status field in the database table  
        /// </summary>
        /// <param name="ID"></param>
        /// <returns> Accept the application</returns>        
        private string connectionString = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
        [HttpPost]
        public ActionResult Accept(string ID)
        {
            try
            { 
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlquery = "update [dbo].[Registration] set Status = 'Approved' where Username = @Username";
                    using (SqlCommand sqlCommand = new SqlCommand(sqlquery, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Username", ID);
                        sqlCommand.Parameters.AddWithValue("@Status", "Approved");
                        int rowsAffected = sqlCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return Json(new { success = true });
                        }
                    }
                }
                return Json(new { success = false });
            }
            catch(Exception)
            {
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// Rejecting the user's application by updating the status field in the database table
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Rejects the application</returns>
        [HttpPost]
        public ActionResult Reject(string ID)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlquery = "update [dbo].[Registration] set Status = 'Rejected' where Username = @Username";
                    using (SqlCommand sqlCommand = new SqlCommand(sqlquery, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Username", ID);
                        sqlCommand.Parameters.AddWithValue("@Status", "Rejected");
                        int rowsAffected = sqlCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return Json(new { success = true });
                        }
                    }
                }
                return Json(new { success = false });
            }
            catch(Exception) { return Json(new { sucess = false }); }
        }

        /// <summary>
        /// Logout function
        /// </summary>
        /// <returns>Session logout</returns>        

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

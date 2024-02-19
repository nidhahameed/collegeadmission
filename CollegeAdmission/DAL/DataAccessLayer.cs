using CollegeAdmission.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollegeAdmission.DAL
{
    public class DataAccessLayer
    {
        /// <summary>
        /// Showing complete details in the admin module when you click View more details.
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns> The user's individual details in the admin module</returns>        
        private string connectionString = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
        public UserProfile GetUserProfile(string UserID)
        {
            UserProfile userProfile = new UserProfile();
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {   
                SqlCommand sqlCommand = new SqlCommand("[dbo].[Get_User]", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    userProfile.Username = reader["Username"].ToString();
                    userProfile.FirstName = reader["FirstName"].ToString();
                    userProfile.LastName = reader["LastName"].ToString();
                    userProfile.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]).Date;
                    userProfile.Age = Convert.ToInt32(reader["Age"]);
                    userProfile.Gender = reader["Gender"].ToString();
                    userProfile.FatherName = reader["FatherName"].ToString();
                    userProfile.MotherName = reader["MotherName"].ToString();
                    userProfile.PhoneNumber = reader["PhoneNumber"].ToString();
                    userProfile.Email = reader["Email"].ToString();
                    userProfile.Address = reader["Address"].ToString();
                    userProfile.State = reader["State"].ToString();
                    userProfile.District = reader["District"].ToString();
                    userProfile.School = reader["School"].ToString();
                    userProfile.Marks = Convert.ToInt32(reader["Marks"]);
                    userProfile.Course = reader["Course"].ToString();
                    userProfile.Department = reader["Department"].ToString();
                    userProfile.Status = reader["Status"].ToString();
                }
                return userProfile;
            }
            catch (Exception)
            {
                return userProfile;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// Viewing user profile details based on currently logged-in user 
        /// </summary>
        public UserClass GetUser(string Username)
        {
            UserClass userClass = new UserClass();
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                SqlCommand sqlCommand = new SqlCommand("[dbo].[Get_User_Profile]", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Username", Username);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    userClass.Username = reader["Username"].ToString();
                    userClass.FirstName = reader["FirstName"].ToString();
                    userClass.LastName = reader["LastName"].ToString();
                    userClass.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]).Date;
                    userClass.Age = Convert.ToInt32(reader["Age"]);
                    userClass.Gender = reader["Gender"].ToString();
                    userClass.FatherName = reader["FatherName"].ToString();
                    userClass.MotherName = reader["MotherName"].ToString();
                    userClass.PhoneNumber = reader["PhoneNumber"].ToString();
                    userClass.Email = reader["Email"].ToString();
                    userClass.Address = reader["Address"].ToString();
                    userClass.State = reader["State"].ToString();
                    userClass.District = reader["District"].ToString();
                    userClass.School = reader["School"].ToString();
                    userClass.Course = reader["Course"].ToString();
                    userClass.Department = reader["Department"].ToString();
                }
                return userClass;
            }
            catch (Exception)
            {
                return userClass;
            }
            finally
            {
                sqlConnection.Close();
            }
        }      
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Data;
using USF_Health_MVC_EF.Models;

namespace USF_Health_MVC_EF.Controllers
{
    public class LoginController : Controller
    {

        public IActionResult Index()
        {

            if (Globals.authenticated == 0)
            {
                SessionInsert();
            }
            

            if (User.Identity.IsAuthenticated)
            {

                SessionUpdate();
                return RedirectToAction("Index", "Home");
                
            }

            return View();
        }

        public void SessionInsert()
        {

            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "usp_sessions_insert";

            //SqlParameter sqlParameter01 = new SqlParameter("username","");
            //sqlParameter01.IsNullable = false;
            //sqlCommand.Parameters.Add(sqlParameter01);

            SqlDataReader sqlDataReader;
            sqlConnection.Open();
            sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.Read())
            {
                Globals.sessionId = Convert.ToInt32(sqlDataReader["ssn_id"]);
                //Globals.currentUserId = Convert.ToInt32(sqlDataReader["usr_id_audit"]);
                //Globals.currentUserName = sqlDataReader["usr_username_audit"].ToString();
            }

            sqlConnection.Close();

            Globals.authenticated = 1;

        }

        public void SessionUpdate()
        {

            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "usp_sessions_update";

            SqlParameter sqlParameter01 = new SqlParameter("ssn_id", Globals.sessionId);
            sqlParameter01.IsNullable = false;
            sqlCommand.Parameters.Add(sqlParameter01);

            SqlParameter sqlParameter02 = new SqlParameter("username", User.Identity.Name);
            sqlParameter02.IsNullable = false;
            sqlCommand.Parameters.Add(sqlParameter02);

            SqlDataReader sqlDataReader;
            sqlConnection.Open();
            sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.Read())
            {
                Globals.sessionId = Convert.ToInt32(sqlDataReader["ssn_id"]);
                Globals.currentUserId = Convert.ToInt32(sqlDataReader["usr_id_audit"]);
                Globals.currentUserName = sqlDataReader["usr_username_audit"].ToString();
            }

            sqlConnection.Close();

            Globals.authenticated = 0;

        }




    }
}

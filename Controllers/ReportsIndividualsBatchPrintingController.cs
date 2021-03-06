﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using USF_Health_MVC_EF.Models;
using System.Web;



namespace USF_Health_MVC_EF.Controllers
{
    public class ReportsIndividualsBatchPrintingController : Controller
    {
       
        private readonly USF_Health_MVC_EFContext _context;
        public ReportsIndividualsBatchPrintingController(USF_Health_MVC_EFContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index(int? id, int? operation) {

        
            if (id != null)
            { 
                    SqlCommand sqlCommand = new SqlCommand();
                    SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "usp_individuals_barcode_insert";

                    SqlParameter sqlParameter01 = new SqlParameter("type", operation);
                    sqlParameter01.IsNullable = false;
                    sqlCommand.Parameters.Add(sqlParameter01);

                    SqlParameter sqlParameter02 = new SqlParameter("is_id", id);
                    sqlParameter02.IsNullable = false;
                    sqlCommand.Parameters.Add(sqlParameter02);

                    SqlParameter sqlParameter03 = new SqlParameter("ssn_id", Globals.sessionId);
                    sqlParameter03.IsNullable = false;
                    sqlCommand.Parameters.Add(sqlParameter03);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();

            }

            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_barcode_select", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            System.Data.DataTable dataTable = new System.Data.DataTable();

            SqlParameter sqlParam01 = new SqlParameter("ssn_id", Globals.sessionId);
            dataAdapter.SelectCommand.Parameters.Add(sqlParam01);

            dataAdapter.Fill(dataTable);

            List<SpIndividualsSamples> listSelected = new List<SpIndividualsSamples>();


            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dr = dataTable.Rows[i];
                SpIndividualsSamples item = new SpIndividualsSamples();

                item.is_id = Int32.Parse(dr["is_id"].ToString());
                item.is_barcode = dr["is_barcode"].ToString();
                item.is_date_collected_text = dr["is_date_collected_text"].ToString();
                item.position = Int32.Parse(dr["position"].ToString());

                listSelected.Add(item);
            }

            ViewBag.is_list_selected = listSelected;
            ViewBag.ssn_id = Globals.sessionId;

            return View();

        }




        [Authorize]
        [HttpGet]
        public IActionResult Print(int? ssn_id)
        {

            SqlConnection sqlConnection = new SqlConnection(Globals.connection);

            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_barcode_select", sqlConnection);
            SqlDataReader sqlDataReader;
            String is_id_list = "";

            SqlParameter sqlParameter01;
            SqlParameter sqlParameter02;
  
            sqlParameter01 = new SqlParameter("type", 2); 
            sqlParameter02 = new SqlParameter("ssn_id", ssn_id);

            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);


            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;

            sqlConnection.Open();
            sqlDataReader = dataAdapter.SelectCommand.ExecuteReader();

            if (sqlDataReader.Read())
            {
                is_id_list = sqlDataReader["is_id_list"].ToString();
            }

            sqlConnection.Close();

            dataAdapter = new SqlDataAdapter("usp_individuals_samples_select", Globals.connection);

            sqlParameter01 = new SqlParameter("type", 7);
            sqlParameter02 = new SqlParameter("is_id_list", is_id_list);

            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);


            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            List<SpIndividualsSamples> list = new List<SpIndividualsSamples>();


            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                SpIndividualsSamples item = new SpIndividualsSamples();
                DataRow dr = dataTable.Rows[i];

                item.is_id = Int32.Parse(dr["is_id"].ToString());
                item.is_barcode = dr["is_barcode"].ToString();
                item.ind_id = Int32.Parse(dr["ind_id"].ToString());
                item.ind_first_name = dr["ind_first_name"].ToString();
                item.ind_last_name = dr["ind_last_name"].ToString();
                item.ind_gender = dr["ind_gender"].ToString();
                item.first_name_last_name = dr["first_name_last_name"].ToString();
                item.is_date_created = dr["is_date_created"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_created"];
                item.is_time_created = dr["is_time_created"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_created"];
                item.is_date_created_text = dr["is_date_created_text"].ToString();
                item.is_date_collected = dr["is_date_collected"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_collected"];
                item.is_time_collected = dr["is_time_collected"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_collected"];
                item.is_date_collected_text = dr["is_date_collected_text"].ToString();
                item.ref_id = Int32.Parse(dr["ref_id"].ToString());
                item.ref_name = dr["ref_name"].ToString();
                item.std_id = Int32.Parse(dr["std_id"].ToString());
                item.std_name = dr["std_name"].ToString();
                item.poo_id = dr["poo_id"] is DBNull ? (int?)null : (int?)Int32.Parse(dr["poo_id"].ToString());
                item.is_details = dr["is_details"].ToString();
                item.position = Int32.Parse(dr["position"].ToString());

                list.Add(item);
            }

            return View(list);
        }



    }
}

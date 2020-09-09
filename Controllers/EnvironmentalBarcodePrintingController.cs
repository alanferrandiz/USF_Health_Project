using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using USF_Health_MVC_EF.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sql;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace USF_Health_MVC_EF.Controllers
{
    public class EnvironmentalBarcodePrintingController : Controller
    {

        private readonly USF_Health_MVC_EFContext _context;
        //private object m;

        public EnvironmentalBarcodePrintingController(USF_Health_MVC_EFContext context)
        {
            _context = context;
        }

        [Authorize("usfhealth_laboratory")]
        public ActionResult Index()
        {
           
            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_places_select", Globals.connection); //I think the problem is here, how am I fixing it though? --Fixed but why can't it be put into a class?
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            List<SpPlaces> list = new List<SpPlaces>();


            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                SpPlaces item = new SpPlaces();
                DataRow dr = dataTable.Rows[i];

                item.pla_id = Int32.Parse(dr["pla_id"].ToString());
                item.pla_date_created_text = dr["pla_date_created_text"].ToString();
                item.pla_name = dr["pla_name"].ToString();
                item.pla_location_reference = dr["pla_location_reference"].ToString();
                item.pla_campus = dr["pla_campus"].ToString();
                item.pla_details = dr["pla_details"].ToString();                
                item.ps_count = Int32.Parse(dr["ps_count"].ToString());
               
                list.Add(item);
            }

            return View( list);

        }

        [Authorize("usfhealth_laboratory")]
        public IActionResult Create(int? id, string? pla_name, string? pla_location_reference, string? pla_campus)
        {
          
            if (ModelState.IsValid)
            {

                    ViewBag.pla_id = id;
                    ViewBag.pla_name = pla_name;
                    ViewBag.pla_location_reference = pla_location_reference;
                    ViewBag.pla_campus = pla_campus;

            }
            return View();
        }

        [Authorize("usfhealth_laboratory")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ps_id,ps_barcode,ps_date_created,ps_time_created,ps_date_collected,ps_time_collected,pla_id,ps_details")] PlaceSample placeSample)
        {
            if (ModelState.IsValid)
            {


                SqlCommand sqlCommand = new SqlCommand();
                SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "usp_places_samples_insert";

                SqlParameter sqlParameter01 = new SqlParameter("usr_id_created", Globals.currentUserId);
                sqlParameter01.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter01);
                
                SqlParameter sqlParameter02 = new SqlParameter("ps_date_collected", placeSample.ps_date_collected);
                sqlParameter02.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter02);

                SqlParameter sqlParameter03 = new SqlParameter("ps_time_collected", placeSample.ps_time_collected);
                sqlParameter03.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter03);
            
                SqlParameter sqlParameter04 = new SqlParameter("usr_id_collected", Globals.currentUserId);
                sqlParameter04.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter04);

                SqlParameter sqlParameter05 = new SqlParameter("pla_id", placeSample.pla_id);
                sqlParameter05.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter05);

                SqlParameter sqlParameter06 = new SqlParameter("ps_details", placeSample.ps_details);
                sqlParameter05.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter06);

                SqlParameter sqlParameter07 = new SqlParameter("usr_id_audit", Globals.currentUserId);
                sqlParameter07.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter07);

                SqlParameter sqlParameter08 = new SqlParameter("ssn_id", Globals.sessionId);
                sqlParameter08.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter08);

                SqlDataReader sqlDataReader;
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    placeSample.ps_id = Convert.ToInt32(sqlDataReader["ps_id"]);
                }

                sqlConnection.Close();

                return RedirectToAction("Print", "EnvironmentalBarcodePrinting", new { id = placeSample.pla_id, ps_id = placeSample.ps_id});
                //return View(individualSample);
            }
            else
                return View();


        }


        [Authorize("usfhealth_laboratory")]
        [HttpGet]
        public IActionResult Print(int? id, int? ps_id)
        {
       
            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_places_samples_select", Globals.connection);

            SqlParameter sqlParameter01;
            SqlParameter sqlParameter02;

            if (ps_id == null) 
            {
                sqlParameter01 = new SqlParameter("type", 2);   //1
                sqlParameter02 = new SqlParameter("pla_id", id);
            } 
            else 
            {
                sqlParameter01 = new SqlParameter("type", 3);   //2
                sqlParameter02 = new SqlParameter("ps_id", ps_id);
            }

            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);


            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            List<SpPlacesSamples> list = new List<SpPlacesSamples>();


            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                SpPlacesSamples item = new SpPlacesSamples();
                DataRow dr = dataTable.Rows[i];

                item.ps_id = Int32.Parse(dr["ps_id"].ToString());
                item.ps_barcode = dr["ps_barcode"].ToString();
                item.pla_id = Int32.Parse(dr["pla_id"].ToString());
                item.pla_name = dr["pla_name"].ToString();
                item.pla_location_reference = dr["pla_location_reference"].ToString();
                item.pla_campus = dr["pla_campus"].ToString();
                item.pla_details = dr["pla_details"].ToString();
                item.ps_date_created = dr["ps_date_created"] is DBNull ? (DateTime?)null : (DateTime?)dr["ps_date_created"];
                item.ps_time_created = dr["ps_time_created"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["ps_time_created"];
                item.ps_date_created_text = dr["ps_date_created_text"].ToString();
                item.ps_date_collected = dr["ps_date_collected"] is DBNull ? (DateTime?)null : (DateTime?)dr["ps_date_collected"];
                item.ps_time_collected = dr["ps_time_collected"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["ps_time_collected"];
                item.ps_date_collected_text = dr["ps_date_collected_text"].ToString();
                item.ps_details = dr["ps_details"].ToString();
                item.position = Int32.Parse(dr["position"].ToString());

                list.Add(item);
            }

            return View(list);
        }



    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using USF_Health_MVC_EF.Models;

namespace USF_Health_MVC_EF.Controllers
{
    public class EnvironmentalBarcodeReadingController : Controller
    {

        private readonly USF_Health_MVC_EFContext _context;
        public EnvironmentalBarcodeReadingController(USF_Health_MVC_EFContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {


            return View();
        }

        [Authorize("usfhealth_laboratory")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(int? id, string? barcode)
        {
            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "usp_places_samples_update";

            SqlParameter sqlParameter01 = new SqlParameter("type", 3);
            sqlParameter01.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter01);

            SqlParameter sqlParameter02 = new SqlParameter("ps_id", id);
            sqlParameter02.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter02);

            SqlParameter sqlParameter03 = new SqlParameter("usr_id_registered", Globals.currentUserId);
            sqlParameter03.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter03);

            SqlParameter sqlParameter04 = new SqlParameter("usr_id_audit", Globals.currentUserId);
            sqlParameter04.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter04);

            SqlParameter sqlParameter05 = new SqlParameter("ssn_id", Globals.sessionId);
            sqlParameter05.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter05);

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            return RedirectToAction(nameof(Index), new { type = 1, status = "Barcode " + barcode + " successfully registered" });

        }

        [Authorize("usfhealth_laboratory")]
        [HttpGet]
        public ActionResult Save(string? barcode)
        {
            if (barcode == null)
            {
                return NotFound();
            }

            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_places_samples_select", Globals.connection); 
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter sqlParameter01 = new SqlParameter("type", 4);  //3
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);

            SqlParameter sqlParameter02 = new SqlParameter("ps_barcode", barcode);
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            if (dataTable.Rows.Count >0)
            {

                SpPlacesSamples spPlacesSamples = new SpPlacesSamples();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow dr = dataTable.Rows[i];

                    spPlacesSamples.ps_id = Int32.Parse(dr["ps_id"].ToString());
                    spPlacesSamples.ps_barcode = dr["ps_barcode"].ToString();
                    spPlacesSamples.pla_id = Int32.Parse(dr["pla_id"].ToString());
                    spPlacesSamples.pla_name = dr["pla_name"].ToString();
                    spPlacesSamples.pla_location_reference = dr["pla_location_reference"].ToString();
                    spPlacesSamples.pla_campus = dr["pla_campus"].ToString();
                    spPlacesSamples.ps_details = dr["ps_details"].ToString();
                    spPlacesSamples.ps_date_created = dr["ps_date_created"] is DBNull ? (DateTime?)null : (DateTime?)dr["ps_date_created"];
                    spPlacesSamples.ps_time_created = dr["ps_time_created"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["ps_time_created"];
                    spPlacesSamples.ps_date_created_text = dr["ps_date_created_text"].ToString();
                    spPlacesSamples.usr_id_created = Int32.Parse(dr["usr_id_created"].ToString());
                    spPlacesSamples.ps_date_collected = dr["ps_date_collected"] is DBNull ? (DateTime?)null : (DateTime?)dr["ps_date_collected"];
                    spPlacesSamples.ps_time_collected = dr["ps_time_collected"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["ps_time_collected"];
                    spPlacesSamples.ps_date_collected_text = dr["ps_date_collected_text"].ToString();
                    spPlacesSamples.ps_date_registered = dr["ps_date_registered"] is DBNull ? (DateTime?)null : (DateTime?)dr["ps_date_registered"];
                    spPlacesSamples.ps_time_registered = dr["ps_time_registered"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["ps_time_registered"];
                    spPlacesSamples.ps_date_registered_text = dr["ps_date_registered_text"].ToString();
                    spPlacesSamples.ps_details = dr["ps_details"].ToString();
                    spPlacesSamples.samples_count = Int32.Parse(dr["samples_count"].ToString());
                    spPlacesSamples.position = Int32.Parse(dr["position"].ToString());

                }

                return View(spPlacesSamples);
            }
            else
            {
                return RedirectToAction(nameof(Index), new { type = 0, status = "Barcode " + barcode + " not found" });
            }

        }


    }
}

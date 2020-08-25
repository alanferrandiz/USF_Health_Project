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
    public class PersonsBarcodeReadingController : Controller
    {

        private readonly USF_Health_MVC_EFContext _context;
        public PersonsBarcodeReadingController(USF_Health_MVC_EFContext context)
        {
            _context = context;
        }

        [Authorize("usfhealth_laboratory")]
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
            sqlCommand.CommandText = "usp_individuals_samples_update";

            SqlParameter sqlParameter01 = new SqlParameter("type", 3);
            sqlParameter01.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter01);

            SqlParameter sqlParameter02 = new SqlParameter("is_id", id);
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

            SqlDataAdapter dataAdapter = new SqlDataAdapter("[usp_individuals_samples_select]", Globals.connection); 
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter sqlParameter01 = new SqlParameter("type", 4);  //3
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);

            SqlParameter sqlParameter02 = new SqlParameter("is_barcode", barcode);
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            if (dataTable.Rows.Count >0)
            {

                SpIndividualsSamples SpIndividualsSamples = new SpIndividualsSamples();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow dr = dataTable.Rows[i];

                    SpIndividualsSamples.is_id = Int32.Parse(dr["is_id"].ToString());
                    SpIndividualsSamples.is_barcode = dr["is_barcode"].ToString();
                    SpIndividualsSamples.ind_id = Int32.Parse(dr["ind_id"].ToString());
                    SpIndividualsSamples.ind_first_name = dr["ind_first_name"].ToString();
                    SpIndividualsSamples.ind_last_name = dr["ind_last_name"].ToString();
                    SpIndividualsSamples.first_name_last_name = dr["first_name_last_name"].ToString();
                    SpIndividualsSamples.ind_gender = dr["ind_gender"].ToString();
                    SpIndividualsSamples.ind_document = dr["ind_document"].ToString();
                    SpIndividualsSamples.ind_details = dr["ind_details"].ToString();
                    SpIndividualsSamples.ref_id = Int32.Parse(dr["ref_id"].ToString());
                    SpIndividualsSamples.ref_name = dr["ref_name"].ToString();
                    SpIndividualsSamples.std_id = Int32.Parse(dr["std_id"].ToString());
                    SpIndividualsSamples.std_name = dr["std_name"].ToString();
                    SpIndividualsSamples.is_date_created = dr["is_date_created"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_created"];
                    SpIndividualsSamples.is_time_created = dr["is_time_created"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_created"];
                    SpIndividualsSamples.is_date_created_text = dr["is_date_created_text"].ToString();
                    SpIndividualsSamples.usr_id_created = Int32.Parse(dr["usr_id_created"].ToString());
                    SpIndividualsSamples.is_date_collected = dr["is_date_collected"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_collected"];
                    SpIndividualsSamples.is_time_collected = dr["is_time_collected"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_collected"];
                    SpIndividualsSamples.is_date_collected_text = dr["is_date_collected_text"].ToString();
                    SpIndividualsSamples.is_date_registered = dr["is_date_registered"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_registered"];
                    SpIndividualsSamples.is_time_registered = dr["is_time_registered"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_registered"];
                    SpIndividualsSamples.is_date_registered_text = dr["is_date_registered_text"].ToString();
                    SpIndividualsSamples.is_details = dr["is_details"].ToString();
                    SpIndividualsSamples.poo_id = dr["poo_id"] is DBNull ? (int?)null : (int?) Int32.Parse(dr["poo_id"].ToString());
                    SpIndividualsSamples.samples_count = Int32.Parse(dr["samples_count"].ToString());
                    SpIndividualsSamples.position = Int32.Parse(dr["position"].ToString());

                }

                return View(SpIndividualsSamples);
            }
            else
            {
                return RedirectToAction(nameof(Index), new { type = 0, status = "Barcode " + barcode + " not found" });
            }

        }


    }
}

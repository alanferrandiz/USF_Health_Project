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
            sqlCommand.CommandText = "usp_individuals_samples_register_barcode";

            SqlParameter sqlParameter01 = new SqlParameter("is_id", id);
            sqlCommand.Parameters.Add(sqlParameter01);

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

            SqlDataAdapter dataAdapter = new SqlDataAdapter("[usp_individuals_samples_select_with_stats]", Globals.connection); 
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter sqlParameter01 = new SqlParameter("type", 3);
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);

            SqlParameter sqlParameter02 = new SqlParameter("is_barcode", barcode);
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            if (dataTable.Rows.Count >0)
            {

                SpIndividualsSamplesWithStats spIndividualsSamplesWithStats = new SpIndividualsSamplesWithStats();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow dr = dataTable.Rows[i];

                    spIndividualsSamplesWithStats.is_id = Int32.Parse(dr["is_id"].ToString());
                    spIndividualsSamplesWithStats.is_barcode = dr["is_barcode"].ToString();
                    spIndividualsSamplesWithStats.ind_id = Int32.Parse(dr["ind_id"].ToString());
                    spIndividualsSamplesWithStats.ind_first_name = dr["ind_first_name"].ToString();
                    spIndividualsSamplesWithStats.ind_last_name = dr["ind_last_name"].ToString();
                    spIndividualsSamplesWithStats.first_name_last_name = dr["first_name_last_name"].ToString();
                    spIndividualsSamplesWithStats.ind_gender = dr["ind_gender"].ToString();
                    spIndividualsSamplesWithStats.ind_document = dr["ind_document"].ToString();
                    spIndividualsSamplesWithStats.ind_details = dr["ind_details"].ToString();
                    spIndividualsSamplesWithStats.ref_id = Int32.Parse(dr["ref_id"].ToString());
                    spIndividualsSamplesWithStats.ref_name = dr["ref_name"].ToString();
                    spIndividualsSamplesWithStats.std_id = Int32.Parse(dr["std_id"].ToString());
                    spIndividualsSamplesWithStats.std_name = dr["std_name"].ToString();
                    spIndividualsSamplesWithStats.is_date_created = dr["is_date_created"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_created"];
                    spIndividualsSamplesWithStats.is_time_created = dr["is_time_created"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_created"];
                    spIndividualsSamplesWithStats.is_date_created_text = dr["is_date_created_text"].ToString();
                    spIndividualsSamplesWithStats.is_date_collected = dr["is_date_collected"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_collected"];
                    spIndividualsSamplesWithStats.is_time_collected = dr["is_time_collected"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_collected"];
                    spIndividualsSamplesWithStats.is_date_collected_text = dr["is_date_collected_text"].ToString();
                    spIndividualsSamplesWithStats.is_date_registered = dr["is_date_registered"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_registered"];
                    spIndividualsSamplesWithStats.is_time_registered = dr["is_time_registered"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_registered"];
                    spIndividualsSamplesWithStats.is_date_registered_text = dr["is_date_registered_text"].ToString();
                    spIndividualsSamplesWithStats.is_details = dr["is_details"].ToString();
                    spIndividualsSamplesWithStats.poo_id = dr["poo_id"] is DBNull ? (int?)null : (int?) Int32.Parse(dr["poo_id"].ToString());
                    spIndividualsSamplesWithStats.samples_count = Int32.Parse(dr["samples_count"].ToString());
                    spIndividualsSamplesWithStats.position = Int32.Parse(dr["position"].ToString());

                }

                return View(spIndividualsSamplesWithStats);
            }
            else
            {
                return RedirectToAction(nameof(Index), new { type = 0, status = "Barcode " + barcode + " not found" });
            }

        }


    }
}

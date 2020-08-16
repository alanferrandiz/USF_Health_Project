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
    public class PersonsPoolsController : Controller
    {

        private readonly USF_Health_MVC_EFContext _context;


        public PersonsPoolsController(USF_Health_MVC_EFContext context)
        {
            _context = context;
        }

        [Authorize("usfhealth_laboratory")]
        public ActionResult Index()
        {

            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_pools_select_with_stats", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            List<SpPoolsWithStats> list = new List<SpPoolsWithStats>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {

                SpPoolsWithStats item = new SpPoolsWithStats();
                DataRow dr = dataTable.Rows[i];

                item.poo_id = Int32.Parse(dr["poo_id"].ToString());
                item.poo_date_created_text = dr["poo_date_created_text"].ToString();
                item.poo_details = dr["poo_details"].ToString();
                item.pr_result = dr["pr_result"].ToString();
                item.pr_ct_value = dr["pr_ct_value"].ToString();
                item.poo_count = Int32.Parse(dr["poo_count"].ToString());

                list.Add(item);
            }

            return View(list);

        }

        [Authorize("usfhealth_laboratory")]
        public ActionResult Assign(int? id)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_samples_select_with_stats", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            System.Data.DataTable dataTable = new System.Data.DataTable();

            SqlParameter sqlParameter01 = new SqlParameter("type", 4);
            SqlParameter sqlParameter02 = new SqlParameter("poo_id", id);

            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

            dataAdapter.Fill(dataTable);

            List<SpIndividualsSamplesWithStats> list = new List<SpIndividualsSamplesWithStats>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                SpIndividualsSamplesWithStats item = new SpIndividualsSamplesWithStats();
                DataRow dr = dataTable.Rows[i];

                item.is_id = Int32.Parse(dr["is_id"].ToString());
                item.is_barcode = dr["is_barcode"].ToString();
                item.ind_id = Int32.Parse(dr["ind_id"].ToString());
                item.ind_first_name = dr["ind_first_name"].ToString();
                item.ind_last_name = dr["ind_last_name"].ToString();
                item.ind_gender = dr["ind_gender"].ToString();
                item.ref_id = Int32.Parse(dr["ref_id"].ToString());
                item.ref_name = dr["ref_name"].ToString();
                item.std_id = Int32.Parse(dr["std_id"].ToString());
                item.std_name = dr["std_name"].ToString();
                item.first_name_last_name = dr["first_name_last_name"].ToString();
                item.is_date_created = dr["is_date_created"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_created"];
                item.is_time_created = dr["is_time_created"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_created"];
                item.is_date_created_text = dr["is_date_created_text"].ToString();
                item.is_date_collected = dr["is_date_collected"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_collected"];
                item.is_time_collected = dr["is_time_collected"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_collected"];
                item.is_date_collected_text = dr["is_date_collected_text"].ToString();
                item.is_details = dr["is_details"].ToString();
                item.poo_id = Int32.Parse(dr["poo_id"].ToString());
                item.poo_details = dr["poo_details"].ToString();
                item.is_date_registered = dr["is_date_registered"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_registered"];
                item.is_time_registered = dr["is_time_registered"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_registered"];
                item.is_date_registered_text = dr["is_date_registered_text"].ToString();
                item.is_date_registered_pool = dr["is_date_registered_pool"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_registered_pool"];
                item.is_time_registered_pool = dr["is_time_registered_pool"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_registered_pool"];
                item.is_date_registered_pool_text = dr["is_date_registered_pool_text"].ToString();
                item.is_details = dr["is_details"].ToString();
                item.position = Int32.Parse(dr["position"].ToString());

                list.Add(item);
            }

            ViewBag.poo_id = id;

            return View(list);
        }




        [Authorize("usfhealth_laboratory")]
        public ActionResult Details(int? id)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_samples_select_with_stats", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            System.Data.DataTable dataTable = new System.Data.DataTable();

            SqlParameter sqlParameter01 = new SqlParameter("type", 4);
            SqlParameter sqlParameter02 = new SqlParameter("poo_id", id);

            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

            dataAdapter.Fill(dataTable);

            List<SpIndividualsSamplesWithStats> list = new List<SpIndividualsSamplesWithStats>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                SpIndividualsSamplesWithStats item = new SpIndividualsSamplesWithStats();
                DataRow dr = dataTable.Rows[i];

                item.is_id = Int32.Parse(dr["is_id"].ToString());
                item.is_barcode = dr["is_barcode"].ToString();
                item.ind_id = Int32.Parse(dr["ind_id"].ToString());
                item.ind_first_name = dr["ind_first_name"].ToString();
                item.ind_last_name = dr["ind_last_name"].ToString();
                item.ind_gender = dr["ind_gender"].ToString();
                item.ref_id = Int32.Parse(dr["ref_id"].ToString());
                item.ref_name = dr["ref_name"].ToString();
                item.std_id = Int32.Parse(dr["std_id"].ToString());
                item.std_name = dr["std_name"].ToString();
                item.first_name_last_name = dr["first_name_last_name"].ToString();
                item.is_date_created = dr["is_date_created"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_created"];
                item.is_time_created = dr["is_time_created"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_created"];
                item.is_date_created_text = dr["is_date_created_text"].ToString();
                item.is_date_collected = dr["is_date_collected"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_collected"];
                item.is_time_collected = dr["is_time_collected"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_collected"];
                item.is_date_collected_text = dr["is_date_collected_text"].ToString();
                item.is_details = dr["is_details"].ToString();
                item.poo_id = Int32.Parse(dr["poo_id"].ToString());
                item.poo_details = dr["poo_details"].ToString();
                item.is_date_registered = dr["is_date_registered"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_registered"];
                item.is_time_registered = dr["is_time_registered"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_registered"];
                item.is_date_registered_text = dr["is_date_registered_text"].ToString();
                item.is_date_registered_pool = dr["is_date_registered_pool"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_registered_pool"];
                item.is_time_registered_pool = dr["is_time_registered_pool"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_registered_pool"];
                item.is_date_registered_pool_text = dr["is_date_registered_pool_text"].ToString();
                item.is_details = dr["is_details"].ToString();
                item.position = Int32.Parse(dr["position"].ToString());

                list.Add(item);
            }

            ViewBag.poo_id = id;

            return View(list);
        }






        [Authorize("usfhealth_laboratory")]
        public ActionResult Unassign(int? id, int? poo_id)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_samples_select_with_stats", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            System.Data.DataTable dataTable = new System.Data.DataTable();

            SqlParameter sqlParameter01 = new SqlParameter("type", 2);
            SqlParameter sqlParameter02 = new SqlParameter("is_id", id);


            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

            dataAdapter.Fill(dataTable);

            //List<SpIndividualsSamplesWithStats> list = new List<SpIndividualsSamplesWithStats>();
            SpIndividualsSamplesWithStats SpIndividualsSamplesWithStats = new SpIndividualsSamplesWithStats();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dr = dataTable.Rows[i];

                SpIndividualsSamplesWithStats.is_id = Int32.Parse(dr["is_id"].ToString());
                SpIndividualsSamplesWithStats.is_barcode = dr["is_barcode"].ToString();
                SpIndividualsSamplesWithStats.ind_id = Int32.Parse(dr["ind_id"].ToString());
                SpIndividualsSamplesWithStats.ind_first_name = dr["ind_first_name"].ToString();
                SpIndividualsSamplesWithStats.ind_last_name = dr["ind_last_name"].ToString();
                SpIndividualsSamplesWithStats.ind_gender = dr["ind_gender"].ToString();
                SpIndividualsSamplesWithStats.ref_id = Int32.Parse(dr["ref_id"].ToString());
                SpIndividualsSamplesWithStats.ref_name = dr["ref_name"].ToString();
                SpIndividualsSamplesWithStats.std_id = Int32.Parse(dr["std_id"].ToString());
                SpIndividualsSamplesWithStats.std_name = dr["std_name"].ToString();
                SpIndividualsSamplesWithStats.first_name_last_name = dr["first_name_last_name"].ToString();
                SpIndividualsSamplesWithStats.is_date_created = dr["is_date_created"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_created"];
                SpIndividualsSamplesWithStats.is_time_created = dr["is_time_created"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_created"];
                SpIndividualsSamplesWithStats.is_date_created_text = dr["is_date_created_text"].ToString();
                SpIndividualsSamplesWithStats.is_date_collected = dr["is_date_collected"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_collected"];
                SpIndividualsSamplesWithStats.is_time_collected = dr["is_time_collected"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_collected"];
                SpIndividualsSamplesWithStats.is_date_collected_text = dr["is_date_collected_text"].ToString();
                SpIndividualsSamplesWithStats.is_details = dr["is_details"].ToString();
                SpIndividualsSamplesWithStats.poo_id = Int32.Parse(dr["poo_id"].ToString());
                SpIndividualsSamplesWithStats.poo_details = dr["poo_details"].ToString();
                SpIndividualsSamplesWithStats.is_date_registered = dr["is_date_registered"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_registered"];
                SpIndividualsSamplesWithStats.is_time_registered = dr["is_time_registered"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_registered"];
                SpIndividualsSamplesWithStats.is_date_registered_text = dr["is_date_registered_text"].ToString();
                SpIndividualsSamplesWithStats.is_date_registered_pool = dr["is_date_registered_pool"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_registered_pool"];
                SpIndividualsSamplesWithStats.is_time_registered_pool = dr["is_time_registered_pool"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_registered_pool"];
                SpIndividualsSamplesWithStats.is_date_registered_pool_text = dr["is_date_registered_pool_text"].ToString();
                SpIndividualsSamplesWithStats.is_details = dr["is_details"].ToString();
                SpIndividualsSamplesWithStats.position = Int32.Parse(dr["position"].ToString());

  
            }

            ViewBag.poo_id = poo_id;
            ViewBag.is_barcode = SpIndividualsSamplesWithStats.is_barcode.ToString();

            return View(SpIndividualsSamplesWithStats);
        }


        [Authorize("usfhealth_laboratory")]
        public ActionResult Delete(int? id)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_pools_select_with_stats", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            System.Data.DataTable dataTable = new System.Data.DataTable();

            SqlParameter sqlParameter01 = new SqlParameter("type", 1);
            SqlParameter sqlParameter02 = new SqlParameter("poo_id", id);


            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

            dataAdapter.Fill(dataTable);

            SpPoolsWithStats spPoolsWithStats = new SpPoolsWithStats();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {

                DataRow dr = dataTable.Rows[i];

                spPoolsWithStats.poo_id = Int32.Parse(dr["poo_id"].ToString());
                spPoolsWithStats.poo_date_created_text = dr["poo_date_created_text"].ToString();
                spPoolsWithStats.poo_details = dr["poo_details"].ToString();
                spPoolsWithStats.poo_count = Int32.Parse(dr["poo_count"].ToString());

            }

            return View(spPoolsWithStats);
        }



        [Authorize("usfhealth_laboratory")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize("usfhealth_laboratory")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("poo_id,poo_date_created,poo_time_created,poo_details")] Pool pool)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pool);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Assign), new { id = pool.poo_id });

            }
            return View(pool);
        }



        [HttpGet]
        public string UpdatePoolResult(int id, string? value)
        {

            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "usp_pools_results_update_result";

            SqlParameter sqlParameter01 = new SqlParameter("poo_id", id);
            sqlParameter01.IsNullable = false;
            sqlCommand.Parameters.Add(sqlParameter01);

            SqlParameter sqlParameter02 = new SqlParameter("pr_result", Globals.Iif(value is null, "", value));//Globals.Iif(value is null, "", value));
            sqlParameter02.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter02);

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            return value;
        }


        [HttpGet]
        public string UpdatePoolCTValue(int id, string? value)
        {

            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "usp_pools_results_update_ct_value";

            SqlParameter sqlParameter01 = new SqlParameter("poo_id", id);
            sqlParameter01.IsNullable = false;
            sqlCommand.Parameters.Add(sqlParameter01);

            SqlParameter sqlParameter02 = new SqlParameter("pr_ct_value", Globals.Iif(value is null, "", value));//Globals.Iif(value is null, "", value));
            sqlParameter02.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter02);

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            return value;
        }



        [HttpGet]
        public string UpdatePoolID(string id, int value, int? operation)
        {
            int rowsAffected = 0;

            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "usp_individuals_samples_update_pool_id";

            SqlParameter sqlParameter01 = new SqlParameter("is_barcode", id);
            sqlParameter01.IsNullable = false;
            sqlCommand.Parameters.Add(sqlParameter01);

            SqlParameter sqlParameter02 = new SqlParameter("poo_id", value);
            sqlParameter02.IsNullable = false;
            sqlCommand.Parameters.Add(sqlParameter02);

            SqlParameter sqlParameter03 = new SqlParameter("operation", operation);
            sqlParameter02.IsNullable = false;
            sqlCommand.Parameters.Add(sqlParameter03);

            sqlConnection.Open();
            rowsAffected = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            if (rowsAffected > 0)
            {

                if (operation == 1)
                {
                    return "Barcode " + id + " was assigned to pool ID " + value.ToString();
                }
                else if (operation == 2)
                {
                    return "Barcode " + id + " was unassigned from pool ID " + value.ToString();
                }
                else if (operation == 3)
                {
                    return "Pool ID " + value.ToString() + " was deleted";
                }
            }
            else {
                return "Barcode " + id + " was not found";
            }

            return String.Empty;

        }

    }
}

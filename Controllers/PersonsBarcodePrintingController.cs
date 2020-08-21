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
    public class PersonsBarcodePrintingController : Controller
    {

        private readonly USF_Health_MVC_EFContext _context;
        //private object m;

        public PersonsBarcodePrintingController(USF_Health_MVC_EFContext context)
        {
            _context = context;
        }

        [Authorize]
        public ActionResult Index()
        {
           
            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_select", Globals.connection); //I think the problem is here, how am I fixing it though? --Fixed but why can't it be put into a class?
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            List<SpIndividuals> list = new List<SpIndividuals>();


            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                SpIndividuals item = new SpIndividuals();
                DataRow dr = dataTable.Rows[i];

                item.ind_id = Int32.Parse(dr["ind_id"].ToString());
                //item.ind_date_time_created = dr["ind_date_time_created"] == null ? (DateTime?)null : (DateTime ?)dr["ind_date_time_created"];
                item.ind_date_created_text = dr["ind_date_created_text"].ToString();
                item.ind_first_name = dr["ind_first_name"].ToString();
                item.ind_last_name = dr["ind_last_name"].ToString();
                item.first_name_last_name = dr["first_name_last_name"].ToString();
                item.ind_email = dr["ind_email"].ToString();
                item.ind_phone = dr["ind_phone"].ToString();
                item.ind_gender = dr["ind_gender"].ToString();
                item.ind_document = dr["ind_document"].ToString();
                item.ref_id = Int32.Parse(dr["ref_id"].ToString());
                item.ref_name = dr["ref_name"].ToString();
                item.std_id = Int32.Parse(dr["std_id"].ToString());
                item.std_name = dr["std_name"].ToString();
                item.ind_details = dr["ind_details"].ToString();
                item.is_count = Int32.Parse(dr["is_count"].ToString());
               
                list.Add(item);
            }

            return View( list);

        }

        [Authorize]
        public IActionResult Create(int? id, string? std_name, string? ref_name)
        {
          
            if (ModelState.IsValid)
            {

                    ViewBag.ind_id = id;
                    ViewBag.std_name = std_name;
                    ViewBag.ref_name = ref_name;
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("is_id,is_barcode,is_date_created,is_time_created,is_date_collected,is_time_collected,ind_id,is_details")] IndividualSample individualSample)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(individualSample);
                //await _context.SaveChangesAsync();


                SqlCommand sqlCommand = new SqlCommand();
                SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "usp_individuals_samples_insert";

                SqlParameter sqlParameter01 = new SqlParameter("usr_id_created", Globals.currentUserId);
                sqlParameter01.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter01);
                
                SqlParameter sqlParameter02 = new SqlParameter("is_date_collected", individualSample.is_date_collected);
                sqlParameter02.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter02);

                SqlParameter sqlParameter03 = new SqlParameter("is_time_collected", individualSample.is_time_collected);
                sqlParameter03.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter03);
            
                SqlParameter sqlParameter04 = new SqlParameter("usr_id_collected", Globals.currentUserId);
                sqlParameter04.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter04);

                SqlParameter sqlParameter05 = new SqlParameter("ind_id", individualSample.ind_id);
                sqlParameter05.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter05);

                SqlParameter sqlParameter06 = new SqlParameter("is_details", individualSample.is_details);
                sqlParameter05.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter06);

                SqlParameter sqlParameter07 = new SqlParameter("usr_id_audit", Globals.currentUserId);
                sqlParameter07.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter07);

                SqlDataReader sqlDataReader;
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    individualSample.is_id = Convert.ToInt32(sqlDataReader["is_id"]);
                }

                sqlConnection.Close();

                return RedirectToAction("Print", "PersonsBarcodePrinting", new { id = individualSample.ind_id, is_id = individualSample.is_id});
                //return View(individualSample);
            }
            else
                return View();


        }


        [Authorize]
        [HttpGet]
        public IActionResult Print(int? id, int? is_id)
        {
       
            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_samples_select", Globals.connection);

            SqlParameter sqlParameter01;
            SqlParameter sqlParameter02;

            if (is_id == null) 
            {
                sqlParameter01 = new SqlParameter("type", 1);
                sqlParameter02 = new SqlParameter("ind_id", id);
            } 
            else 
            {
                sqlParameter01 = new SqlParameter("type", 2);
                sqlParameter02 = new SqlParameter("is_id", is_id);
            }


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
                item.poo_id = dr["poo_id"] is DBNull ? (int?)null : (int?) Int32.Parse(dr["poo_id"].ToString());
                item.is_details = dr["is_details"].ToString();
                item.position = Int32.Parse(dr["position"].ToString());

                list.Add(item);
            }

            return View(list);
        }



    }
}

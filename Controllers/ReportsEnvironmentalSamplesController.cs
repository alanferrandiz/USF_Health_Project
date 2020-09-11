using System;
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

namespace USF_Health_MVC_EF.Controllers
{
    public class ReportsEnvironmentalSamplesController : Controller
    {
       
        private readonly USF_Health_MVC_EFContext _context;
        public ReportsEnvironmentalSamplesController(USF_Health_MVC_EFContext context)
        {
            _context = context;
        }



        [Authorize]
        public IActionResult Index(int? type, DateTime? dateStart, DateTime? dateEnd, String ? sampleResult)
        {

            SqlDataAdapter dataAdapterPlacesSamples = new SqlDataAdapter("usp_places_samples_select", Globals.connection);
            dataAdapterPlacesSamples.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;

            System.Data.DataTable dataTablePlacesSamples = new System.Data.DataTable();

            SqlParameter sqlParameterPlacesSamples01 = new SqlParameter("type", 3);      //5
            sqlParameterPlacesSamples01.IsNullable = false;
            dataAdapterPlacesSamples.SelectCommand.Parameters.Add(sqlParameterPlacesSamples01);

            dataAdapterPlacesSamples.Fill(dataTablePlacesSamples);
            List<SpPlacesSamples> listPlacesSamples = new List<SpPlacesSamples>();

            for (int i = 0; i < dataTablePlacesSamples.Rows.Count; i++)
            {
                SpPlacesSamples item = new SpPlacesSamples();
                DataRow dr = dataTablePlacesSamples.Rows[i];

                item.ps_id = Int32.Parse(dr["ps_id"].ToString());
                listPlacesSamples.Add(item);
            }
            ViewData["ps_id"] = listPlacesSamples;


            if (type == 1)
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_places_samples_select", Globals.connection);
                dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                
                System.Data.DataTable dataTable = new System.Data.DataTable();

                SqlParameter sqlParameter01 = new SqlParameter("type", 6);      //5
                sqlParameter01.IsNullable = false;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);

                SqlParameter sqlParameter02 = new SqlParameter("date_start", dateStart);
                sqlParameter02.IsNullable = true;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

                SqlParameter sqlParameter03 = new SqlParameter("date_end", dateEnd);
                sqlParameter03.IsNullable = true;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter03);

                SqlParameter sqlParameter04 = new SqlParameter("psres_result", sampleResult);
                sqlParameter03.IsNullable = true;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter04);

                dataAdapter.Fill(dataTable);
                List<SpPlacesSamples> list = new List<SpPlacesSamples>();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    SpPlacesSamples item = new SpPlacesSamples();
                    DataRow dr = dataTable.Rows[i];

                    item.ps_id = Int32.Parse(dr["ps_id"].ToString());
                    item.ps_barcode = dr["ps_barcode"].ToString();
                    item.ps_date_created_text = dr["ps_date_created_text"].ToString();
                    item.ps_date_collected_text = dr["ps_date_collected_text"].ToString();
                    item.ps_date_registered_text = dr["ps_date_registered_text"].ToString();
                    item.pla_id = Int32.Parse(dr["pla_id"].ToString());
                    item.pla_name = dr["pla_name"].ToString();
                    item.pla_location_reference = dr["pla_location_reference"].ToString();
                    item.pla_campus = dr["pla_campus"].ToString();
                    item.pla_details = dr["pla_details"].ToString();
                    item.ps_well_number = dr["ps_well_number"].ToString();
                    item.ps_details = dr["ps_details"].ToString();
                    item.psres_result = dr["psres_result"].ToString();
                    item.psres_ct_value = dr["psres_ct_value"].ToString();
                    list.Add(item);
                }

                return View(list);
            }
            else
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_places_samples_select", Globals.connection);
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
                    item.ps_date_created_text = dr["ps_date_created_text"].ToString();
                    item.ps_date_collected_text = dr["ps_date_collected_text"].ToString();
                    item.ps_date_registered_text = dr["ps_date_registered_text"].ToString();
                    item.pla_id = Int32.Parse(dr["pla_id"].ToString());
                    item.pla_name = dr["pla_name"].ToString();
                    item.pla_location_reference = dr["pla_location_reference"].ToString();
                    item.pla_campus = dr["pla_campus"].ToString();
                    item.pla_details = dr["pla_details"].ToString();
                    item.ps_well_number = dr["ps_well_number"].ToString();
                    item.ps_details = dr["ps_details"].ToString();
                    item.psres_result = dr["psres_result"].ToString();
                    item.psres_ct_value = dr["psres_ct_value"].ToString();
                    list.Add(item);
                }

                return View(list);
            }
        }


        //[HttpGet]
        //public int ReportsPersonSamples(DateTime? dateStart, DateTime? dateEnd)
        //{

        //    return 1;
        //}
        



    }
}

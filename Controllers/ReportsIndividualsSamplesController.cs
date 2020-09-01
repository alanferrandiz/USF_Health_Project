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
    public class ReportsIndividualsSamplesController : Controller
    {
       
        private readonly USF_Health_MVC_EFContext _context;
        public ReportsIndividualsSamplesController(USF_Health_MVC_EFContext context)
        {
            _context = context;
        }



        [Authorize]
        public IActionResult Index(int? type, DateTime? dateStart, DateTime? dateEnd, String ? poolResult, int ? poolID)
        {

            SqlDataAdapter dataAdapterPools = new SqlDataAdapter("usp_pools_select", Globals.connection);
            dataAdapterPools.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;

            System.Data.DataTable dataTablePools = new System.Data.DataTable();

            SqlParameter sqlParameterPools01 = new SqlParameter("type", 3);      //5
            sqlParameterPools01.IsNullable = false;
            dataAdapterPools.SelectCommand.Parameters.Add(sqlParameterPools01);

            dataAdapterPools.Fill(dataTablePools);
            List<SpPools> listPools = new List<SpPools>();

            for (int i = 0; i < dataTablePools.Rows.Count; i++)
            {
                SpPools item = new SpPools();
                DataRow dr = dataTablePools.Rows[i];

                item.poo_id = Int32.Parse(dr["poo_id"].ToString());
                listPools.Add(item);
            }
            ViewData["pools"] = listPools;





            if (type == 1)
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_samples_select", Globals.connection);
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

                SqlParameter sqlParameter04 = new SqlParameter("poo_result", poolResult);
                sqlParameter03.IsNullable = true;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter04);

                SqlParameter sqlParameter05 = new SqlParameter("poo_id", poolID);
                sqlParameter03.IsNullable = true;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter05);

                dataAdapter.Fill(dataTable);
                List<SpIndividualsSamples> list = new List<SpIndividualsSamples>();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    SpIndividualsSamples item = new SpIndividualsSamples();
                    DataRow dr = dataTable.Rows[i];

                    item.is_id = Int32.Parse(dr["is_id"].ToString());
                    item.is_barcode = dr["is_barcode"].ToString();
                    item.is_date_created_text = dr["is_date_created_text"].ToString();
                    item.is_date_collected_text = dr["is_date_collected_text"].ToString();
                    item.is_date_registered_text = dr["is_date_registered_text"].ToString();
                    item.ind_id = Int32.Parse(dr["ind_id"].ToString());
                    item.ind_first_name = dr["ind_first_name"].ToString();
                    item.ind_last_name = dr["ind_last_name"].ToString();
                    item.ind_gender = dr["ind_gender"].ToString();
                    item.ind_document = dr["ind_document"].ToString();
                    item.is_well_number = dr["is_well_number"].ToString();
                    item.poo_id = dr["poo_id"] is DBNull ? (int?)null : (int?)Int32.Parse(dr["poo_id"].ToString());
                    item.poo_details = dr["poo_details"].ToString();
                    item.is_details = dr["is_details"].ToString();
                    item.ref_id = Int32.Parse(dr["ref_id"].ToString());
                    item.ref_name = dr["ref_name"].ToString();
                    item.std_id = Int32.Parse(dr["std_id"].ToString());
                    item.std_name = dr["std_name"].ToString();
                    item.pr_result = dr["pr_result"].ToString();
                    item.pr_ct_value = dr["pr_ct_value"].ToString();
                    list.Add(item);
                }

                return View(list);
            }
            else
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_samples_select", Globals.connection);
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
                    item.is_date_created_text = dr["is_date_created_text"].ToString();
                    item.is_date_collected_text = dr["is_date_collected_text"].ToString();
                    item.is_date_registered_text = dr["is_date_registered_text"].ToString();
                    item.ind_id = Int32.Parse(dr["ind_id"].ToString());
                    item.ind_first_name = dr["ind_first_name"].ToString();
                    item.ind_last_name = dr["ind_last_name"].ToString();
                    item.ind_gender = dr["ind_gender"].ToString();
                    item.ind_document = dr["ind_document"].ToString();
                    item.is_well_number = dr["is_well_number"].ToString();
                    item.poo_id = dr["poo_id"] is DBNull ? (int?)null : (int?)Int32.Parse(dr["poo_id"].ToString());
                    item.poo_details = dr["poo_details"].ToString();
                    item.is_details = dr["is_details"].ToString();
                    item.ref_id = Int32.Parse(dr["ref_id"].ToString());
                    item.ref_name = dr["ref_name"].ToString();
                    item.std_id = Int32.Parse(dr["std_id"].ToString());
                    item.std_name = dr["std_name"].ToString();
                    item.pr_result = dr["pr_result"].ToString();
                    item.pr_ct_value = dr["pr_ct_value"].ToString();
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

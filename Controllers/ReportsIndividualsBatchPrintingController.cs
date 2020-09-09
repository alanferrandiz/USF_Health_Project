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
        public IActionResult Index() {

         

           

            if (Request.Method == "GET")
            {

                SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_samples_select", Globals.connection);
                dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                System.Data.DataTable dataTable = new System.Data.DataTable();

                SqlParameter sqlParameter01 = new SqlParameter("type", 8);  //4

                dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);

                dataAdapter.Fill(dataTable);

                List<SpIndividualsSamples> list = new List<SpIndividualsSamples>();
                List<SpIndividualsSamples> listSelected = new List<SpIndividualsSamples>();


                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    SpIndividualsSamples item = new SpIndividualsSamples();
                    DataRow dr = dataTable.Rows[i];

                    item.is_id = Int32.Parse(dr["is_id"].ToString());
                    item.is_barcode = dr["is_barcode"].ToString();
                    item.ind_gender = dr["ind_gender"].ToString();
                    item.std_id = Int32.Parse(dr["std_id"].ToString());
                    item.std_name = dr["std_name"].ToString();
                    item.is_date_collected_text = dr["is_date_collected_text"].ToString();
                    item.is_details = dr["is_details"].ToString();
                    item.position = Int32.Parse(dr["position"].ToString());

                    list.Add(item);
                }

                ViewBag.is_list = list;
                ViewBag.is_list_selected = listSelected;
            }

            return View();

        }

        //[Authorize]
        //public IActionResult Index(int? type, DateTime? dateStart, DateTime? dateEnd, String ? poolResult, int ? poolID)
        //{

        //    SqlDataAdapter dataAdapterPools = new SqlDataAdapter("usp_pools_select", Globals.connection);
        //    dataAdapterPools.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;

        //    System.Data.DataTable dataTablePools = new System.Data.DataTable();

        //    SqlParameter sqlParameterPools01 = new SqlParameter("type", 3);      //5
        //    sqlParameterPools01.IsNullable = false;
        //    dataAdapterPools.SelectCommand.Parameters.Add(sqlParameterPools01);

        //    dataAdapterPools.Fill(dataTablePools);
        //    List<SpPools> listPools = new List<SpPools>();

        //    for (int i = 0; i < dataTablePools.Rows.Count; i++)
        //    {
        //        SpPools item = new SpPools();
        //        DataRow dr = dataTablePools.Rows[i];

        //        item.poo_id = Int32.Parse(dr["poo_id"].ToString());
        //        listPools.Add(item);
        //    }
        //    ViewData["pools"] = listPools;





        //    if (type == 1)
        //    {
        //        SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_samples_select", Globals.connection);
        //        dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;

        //        System.Data.DataTable dataTable = new System.Data.DataTable();

        //        SqlParameter sqlParameter01 = new SqlParameter("type", 6);      //5
        //        sqlParameter01.IsNullable = false;
        //        dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);

        //        SqlParameter sqlParameter02 = new SqlParameter("date_start", dateStart);
        //        sqlParameter02.IsNullable = true;
        //        dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

        //        SqlParameter sqlParameter03 = new SqlParameter("date_end", dateEnd);
        //        sqlParameter03.IsNullable = true;
        //        dataAdapter.SelectCommand.Parameters.Add(sqlParameter03);

        //        SqlParameter sqlParameter04 = new SqlParameter("poo_result", poolResult);
        //        sqlParameter03.IsNullable = true;
        //        dataAdapter.SelectCommand.Parameters.Add(sqlParameter04);

        //        SqlParameter sqlParameter05 = new SqlParameter("poo_id", poolID);
        //        sqlParameter03.IsNullable = true;
        //        dataAdapter.SelectCommand.Parameters.Add(sqlParameter05);

        //        dataAdapter.Fill(dataTable);
        //        List<SpIndividualsSamples> list = new List<SpIndividualsSamples>();

        //        for (int i = 0; i < dataTable.Rows.Count; i++)
        //        {
        //            SpIndividualsSamples item = new SpIndividualsSamples();
        //            DataRow dr = dataTable.Rows[i];

        //            item.is_id = Int32.Parse(dr["is_id"].ToString());
        //            item.is_barcode = dr["is_barcode"].ToString();
        //            item.is_date_created_text = dr["is_date_created_text"].ToString();
        //            item.is_date_collected_text = dr["is_date_collected_text"].ToString();
        //            item.is_date_registered_text = dr["is_date_registered_text"].ToString();
        //            item.ind_id = Int32.Parse(dr["ind_id"].ToString());
        //            item.ind_first_name = dr["ind_first_name"].ToString();
        //            item.ind_last_name = dr["ind_last_name"].ToString();
        //            item.ind_gender = dr["ind_gender"].ToString();
        //            item.ind_document = dr["ind_document"].ToString();
        //            item.is_well_number = dr["is_well_number"].ToString();
        //            item.poo_id = dr["poo_id"] is DBNull ? (int?)null : (int?)Int32.Parse(dr["poo_id"].ToString());
        //            item.poo_details = dr["poo_details"].ToString();
        //            item.is_details = dr["is_details"].ToString();
        //            item.ref_id = Int32.Parse(dr["ref_id"].ToString());
        //            item.ref_name = dr["ref_name"].ToString();
        //            item.std_id = Int32.Parse(dr["std_id"].ToString());
        //            item.std_name = dr["std_name"].ToString();
        //            item.pr_result = dr["pr_result"].ToString();
        //            item.pr_ct_value = dr["pr_ct_value"].ToString();
        //            list.Add(item);
        //        }

        //        return View(list);
        //    }
        //    else
        //    {
        //        SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_samples_select", Globals.connection);
        //        dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
        //        System.Data.DataTable dataTable = new System.Data.DataTable();
        //        dataAdapter.Fill(dataTable);
        //        List<SpIndividualsSamples> list = new List<SpIndividualsSamples>();

        //        for (int i = 0; i < dataTable.Rows.Count; i++)
        //        {
        //            SpIndividualsSamples item = new SpIndividualsSamples();
        //            DataRow dr = dataTable.Rows[i];

        //            item.is_id = Int32.Parse(dr["is_id"].ToString());
        //            item.is_barcode = dr["is_barcode"].ToString();
        //            item.is_date_created_text = dr["is_date_created_text"].ToString();
        //            item.is_date_collected_text = dr["is_date_collected_text"].ToString();
        //            item.is_date_registered_text = dr["is_date_registered_text"].ToString();
        //            item.ind_id = Int32.Parse(dr["ind_id"].ToString());
        //            item.ind_first_name = dr["ind_first_name"].ToString();
        //            item.ind_last_name = dr["ind_last_name"].ToString();
        //            item.ind_gender = dr["ind_gender"].ToString();
        //            item.ind_document = dr["ind_document"].ToString();
        //            item.is_well_number = dr["is_well_number"].ToString();
        //            item.poo_id = dr["poo_id"] is DBNull ? (int?)null : (int?)Int32.Parse(dr["poo_id"].ToString());
        //            item.poo_details = dr["poo_details"].ToString();
        //            item.is_details = dr["is_details"].ToString();
        //            item.ref_id = Int32.Parse(dr["ref_id"].ToString());
        //            item.ref_name = dr["ref_name"].ToString();
        //            item.std_id = Int32.Parse(dr["std_id"].ToString());
        //            item.std_name = dr["std_name"].ToString();
        //            item.pr_result = dr["pr_result"].ToString();
        //            item.pr_ct_value = dr["pr_ct_value"].ToString();
        //            list.Add(item);
        //        }

        //        return View(list);
        //    }
        //}



        [HttpGet]
        //        public int UpdateIndividualsBatchPrinting(String id, int operation, List<SpIndividualsSamples> pList, List<SpIndividualsSamples> pListSelected)
        public int UpdateIndividualsBatchPrinting(int id, int operation)

        {

    


            //SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_samples_select", Globals.connection);
            //dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //System.Data.DataTable dataTable = new System.Data.DataTable();

            //SqlParameter sqlParameter01 = new SqlParameter("type", 3);
            //SqlParameter sqlParameter02 = new SqlParameter("is_id", id);

            //dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            //dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);


            //dataAdapter.Fill(dataTable);

            ////List<SpIndividualsSamples> list = pList;
            ////List<SpIndividualsSamples> listSelected = pListSelected;

            //List<SpIndividualsSamples> list = new List<SpIndividualsSamples>();
            //List<SpIndividualsSamples> listSelected = new List<SpIndividualsSamples>();
            //SpIndividualsSamples item = new SpIndividualsSamples();


            //for (int i = 0; i < dataTable.Rows.Count; i++)
            //{
            //    DataRow dr = dataTable.Rows[i];

            //    item.is_id = Int32.Parse(dr["is_id"].ToString());
            //    item.is_barcode = dr["is_barcode"].ToString();
            //    item.ind_gender = dr["ind_gender"].ToString();
            //    item.std_id = Int32.Parse(dr["std_id"].ToString());
            //    item.std_name = dr["std_name"].ToString();
            //    item.is_date_collected_text = dr["is_date_collected_text"].ToString();
            //    item.is_details = dr["is_details"].ToString();
            //    item.position = Int32.Parse(dr["position"].ToString());
            //}

            //ViewBag.is_list = spIndividualsSamples;
            //ViewBag.is_list_selected = spIndividualsSamplesSelected;

            if (operation == 1) //agregar uno para imprimir
            {

                //listSelected.Add(item);
                //list.Remove(item);
            }
            else if (operation == 2)//quitar uno para imprimir
            {
                //listSelected.Remove(item);
                //list.Add(item);
            }

            //ViewBag.is_list = list;
            //ViewBag.is_list_selected = listSelected;


            //SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_samples_select", Globals.connection);
            //dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //System.Data.DataTable dataTable = new System.Data.DataTable();

            //SqlParameter sqlParameter01 = new SqlParameter("type", 7);  //4
            //SqlParameter sqlParameter02 = new SqlParameter("is_id_list", id);

            //dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            //dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

            //dataAdapter.Fill(dataTable);

            //List<SpIndividualsSamples> list = new List<SpIndividualsSamples>();

            //for (int i = 0; i < dataTable.Rows.Count; i++)
            //{
            //    SpIndividualsSamples item = new SpIndividualsSamples();
            //    DataRow dr = dataTable.Rows[i];

            //    item.is_id = Int32.Parse(dr["is_id"].ToString());
            //    item.is_barcode = dr["is_barcode"].ToString();
            //    item.ind_id = Int32.Parse(dr["ind_id"].ToString());
            //    item.ind_first_name = dr["ind_first_name"].ToString();
            //    item.ind_last_name = dr["ind_last_name"].ToString();
            //    item.ind_gender = dr["ind_gender"].ToString();
            //    item.ref_id = Int32.Parse(dr["ref_id"].ToString());
            //    item.ref_name = dr["ref_name"].ToString();
            //    item.std_id = Int32.Parse(dr["std_id"].ToString());
            //    item.std_name = dr["std_name"].ToString();
            //    item.first_name_last_name = dr["first_name_last_name"].ToString();
            //    item.is_date_created = dr["is_date_created"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_created"];
            //    item.is_time_created = dr["is_time_created"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_created"];
            //    item.is_date_created_text = dr["is_date_created_text"].ToString();
            //    item.is_date_collected = dr["is_date_collected"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_collected"];
            //    item.is_time_collected = dr["is_time_collected"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_collected"];
            //    item.is_date_collected_text = dr["is_date_collected_text"].ToString();
            //    item.is_details = dr["is_details"].ToString();
            //    item.poo_id = Int32.Parse(dr["poo_id"].ToString());
            //    item.poo_details = dr["poo_details"].ToString();
            //    item.is_date_registered = dr["is_date_registered"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_registered"];
            //    item.is_time_registered = dr["is_time_registered"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_registered"];
            //    item.is_date_registered_text = dr["is_date_registered_text"].ToString();
            //    item.is_date_registered_pool = dr["is_date_registered_pool"] is DBNull ? (DateTime?)null : (DateTime?)dr["is_date_registered_pool"];
            //    item.is_time_registered_pool = dr["is_time_registered_pool"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["is_time_registered_pool"];
            //    item.is_date_registered_pool_text = dr["is_date_registered_pool_text"].ToString();
            //    item.is_details = dr["is_details"].ToString();
            //    item.position = Int32.Parse(dr["position"].ToString());

            //    list.Add(item);
            //}


            //ViewBag.is_list = list;

            return 1;

        }



        //[HttpGet]
        //public int ReportsPersonSamples(DateTime? dateStart, DateTime? dateEnd)
        //{

        //    return 1;
        //}




    }
}

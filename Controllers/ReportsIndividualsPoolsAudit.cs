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
    public class ReportsIndividualsPoolsAudit : Controller
    {
       
        private readonly USF_Health_MVC_EFContext _context;
        public ReportsIndividualsPoolsAudit(USF_Health_MVC_EFContext context)
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
                SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_audit_individuals_select", Globals.connection);
                dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                
                System.Data.DataTable dataTable = new System.Data.DataTable();

                SqlParameter sqlParameter01 = new SqlParameter("type", 2);      //5
                sqlParameter01.IsNullable = false;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);

                SqlParameter sqlParameter02 = new SqlParameter("date_start", dateStart);
                sqlParameter02.IsNullable = true;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

                SqlParameter sqlParameter03 = new SqlParameter("date_end", dateEnd);
                sqlParameter03.IsNullable = true;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter03);

                SqlParameter sqlParameter04 = new SqlParameter("pr_result", poolResult);
                sqlParameter03.IsNullable = true;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter04);

                SqlParameter sqlParameter05 = new SqlParameter("poo_id", poolID);
                sqlParameter03.IsNullable = true;
                dataAdapter.SelectCommand.Parameters.Add(sqlParameter05);

                dataAdapter.Fill(dataTable);
                List<SpAudit> list = new List<SpAudit>();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    SpAudit item = new SpAudit();
                    DataRow dr = dataTable.Rows[i];

                    item.aud_id = Int32.Parse(dr["aud_id"].ToString());
                    item.aud_operation_id = Int32.Parse(dr["aud_operation_id"].ToString());
                    item.aud_operation = dr["aud_operation"].ToString();
                    item.aud_date = dr["aud_date"] is DBNull ? (DateTime?)null : (DateTime?)dr["aud_date"];
                    item.aud_time = dr["aud_time"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["aud_time"];
                    item.aud_table = dr["aud_table"].ToString();
                    item.aud_identifier_id = dr["aud_identifier_id"].ToString();
                    item.aud_identifier_field = dr["aud_identifier_field"].ToString();
                    item.aud_field = dr["aud_field"].ToString();
                    item.aud_identifier_field = dr["aud_identifier_field"].ToString();
                    item.aud_before = dr["aud_before"].ToString();
                    item.aud_after = dr["aud_after"].ToString();
                    item.usr_id_audit = Int32.Parse(dr["usr_id_audit"].ToString());
                    item.usr_username = dr["usr_username"].ToString();
                    item.ssn_id = Int32.Parse(dr["ssn_id"].ToString());
                    list.Add(item);
                }

                return View(list);
            }
            else
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_audit_individuals_select", Globals.connection);
                dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                System.Data.DataTable dataTable = new System.Data.DataTable();
                dataAdapter.Fill(dataTable);
                List<SpAudit> list = new List<SpAudit>();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    SpAudit item = new SpAudit();
                    DataRow dr = dataTable.Rows[i];

                    item.aud_id = Int32.Parse(dr["aud_id"].ToString());
                    item.aud_operation_id = Int32.Parse(dr["aud_operation_id"].ToString());
                    item.aud_operation = dr["aud_operation"].ToString();
                    item.aud_date = dr["aud_date"] is DBNull ? (DateTime?)null : (DateTime?)dr["aud_date"];
                    item.aud_time = dr["aud_time"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["aud_time"];
                    item.aud_table = dr["aud_table"].ToString();
                    item.aud_identifier_id = dr["aud_identifier_id"].ToString();
                    item.aud_identifier_field = dr["aud_identifier_field"].ToString();
                    item.aud_field = dr["aud_field"].ToString();
                    item.aud_identifier_field = dr["aud_identifier_field"].ToString();
                    item.aud_before = dr["aud_before"].ToString();
                    item.aud_after = dr["aud_after"].ToString();
                    item.usr_id_audit = Int32.Parse(dr["usr_id_audit"].ToString());
                    item.usr_username = dr["usr_username"].ToString();
                    item.ssn_id = Int32.Parse(dr["ssn_id"].ToString());
                    list.Add(item);
                }

                return View(list);
            }
        }

    }
}

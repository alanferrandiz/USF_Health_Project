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
    public class PersonsSamplesController : Controller
    {
       
        private readonly USF_Health_MVC_EFContext _context;
        public PersonsSamplesController(USF_Health_MVC_EFContext context)
        {
            _context = context;
        }

        [Authorize("usfhealth_laboratory")]
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var individualSamples = await _context.tb_individuals_samples
            //    .FirstOrDefaultAsync(m => m.is_id == id);
            //if (individualSamples == null)
            //{
            //    return NotFound();
            //}

            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_samples_select_with_stats", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter sqlParameter01 = new SqlParameter("type",2);
            sqlParameter01.IsNullable = false;

            SqlParameter sqlParameter02 = new SqlParameter("is_id", id);
            sqlParameter02.IsNullable = false;

            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            //List<SpIndividualsSamplesWithStats> list = new List<SpIndividualsSamplesWithStats>();
            SpIndividualsSamplesWithStats spIndividualsSamplesWithStats = new SpIndividualsSamplesWithStats();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                //SpIndividualsSamplesWithStats item = new SpIndividualsSamplesWithStats();
                DataRow dr = dataTable.Rows[i];

                spIndividualsSamplesWithStats.is_id = Int32.Parse(dr["is_id"].ToString());
                spIndividualsSamplesWithStats.is_barcode = dr["is_barcode"].ToString();
                spIndividualsSamplesWithStats.is_date_created_text = dr["is_date_created_text"].ToString();
                spIndividualsSamplesWithStats.is_date_collected_text = dr["is_date_collected_text"].ToString();
                spIndividualsSamplesWithStats.is_date_registered_text = dr["is_date_registered_text"].ToString();
                spIndividualsSamplesWithStats.ind_id = Int32.Parse(dr["ind_id"].ToString());
                spIndividualsSamplesWithStats.ind_first_name = dr["ind_first_name"].ToString();
                spIndividualsSamplesWithStats.ind_last_name = dr["ind_last_name"].ToString();
                spIndividualsSamplesWithStats.ind_gender = dr["ind_gender"].ToString();
                spIndividualsSamplesWithStats.ind_document = dr["ind_document"].ToString();
                spIndividualsSamplesWithStats.is_well_number = dr["is_well_number"].ToString();
                spIndividualsSamplesWithStats.poo_id = dr["poo_id"] is DBNull ? (int?)null : (int?)Int32.Parse(dr["poo_id"].ToString());
                spIndividualsSamplesWithStats.is_details = dr["is_details"].ToString();
                spIndividualsSamplesWithStats.ref_id = Int32.Parse(dr["ref_id"].ToString());
                spIndividualsSamplesWithStats.ref_name = dr["ref_name"].ToString();
                spIndividualsSamplesWithStats.std_id = Int32.Parse(dr["std_id"].ToString());
                spIndividualsSamplesWithStats.std_name = dr["std_name"].ToString();

                //list.Add(item);
            }


            //SpIndividualsWithStats spIndividualsWithStats = new SpIndividualsWithStats();
            //ViewData["spIndividualsWithStats"] = spIndividualsWithStats.GetIndividualbyId(spIndividualsSamplesWithStats.ind_id);

            return View(spIndividualsSamplesWithStats);
        }

        [Authorize("usfhealth_laboratory")]
        public IActionResult Index()
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_samples_select_with_stats", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;

            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            List<SpIndividualsSamplesWithStats> list = new List<SpIndividualsSamplesWithStats>();


            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                SpIndividualsSamplesWithStats item = new SpIndividualsSamplesWithStats();
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
                item.poo_id =  dr["poo_id"] is DBNull ? (int?) null  : (int?)Int32.Parse(dr["poo_id"].ToString());
                item.is_details = dr["is_details"].ToString();
                item.ref_id = Int32.Parse(dr["ref_id"].ToString());
                item.ref_name = dr["ref_name"].ToString();
                item.std_id = Int32.Parse(dr["std_id"].ToString());
                item.std_name = dr["std_name"].ToString();

                list.Add(item);
            }

            return View(list);
        }



        [Authorize("usfhealth_laboratory")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var individualSamples = await _context.tb_individuals_samples
                .FirstOrDefaultAsync(m => m.is_id == id);
            if (individualSamples == null)
            {
                return NotFound();
            }


            SpIndividualsWithStats spIndividualsWithStats = new SpIndividualsWithStats();
            ViewData["spIndividualsWithStats"] = spIndividualsWithStats.GetIndividualbyId(individualSamples.ind_id);

            return View(individualSamples);
        }


        [Authorize("usfhealth_laboratory")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var individualSamples = await _context.tb_individuals_samples.FindAsync(id);
            _context.tb_individuals_samples.Remove(individualSamples);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        [Authorize("usfhealth_laboratory")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var individualSamples = await _context.tb_individuals_samples.FindAsync(id);
       
            if (individualSamples == null)
            {
                return NotFound();
            }

            SpIndividualsWithStats spIndividualsWithStats = new SpIndividualsWithStats();
            ViewData["spIndividualsWithStats"] = spIndividualsWithStats.GetAllIndividuals();

            return View(individualSamples);
        }



        [Authorize("usfhealth_laboratory")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("is_id,is_barcode,is_date_created,is_time_created,is_date_collected,is_time_collected,is_date_registered,is_time_registered,ind_id,is_well_number,is_details")] IndividualSample individualSample)
        {
            if (id != individualSample.is_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(individualSample);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IndividualSamplesExists(individualSample.is_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(individualSample);
        }

        private bool IndividualSamplesExists(int id)
        {
            return _context.tb_individuals_samples.Any(e => e.is_id == id);
        }





        [HttpGet]
        public string UpdateWellNumber(int id, string? value)
        {

            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "usp_individuals_samples_update_well_number";

            SqlParameter sqlParameter01 = new SqlParameter("is_id", id);
            sqlParameter01.IsNullable = false;
            sqlCommand.Parameters.Add(sqlParameter01);

            SqlParameter sqlParameter02 = new SqlParameter("is_well_number", value);//Globals.Iif(value is null, "", value));
            sqlParameter02.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter02);

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            return value;
        }



        [Authorize("usfhealth_laboratory")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

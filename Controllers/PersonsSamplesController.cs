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

        [Authorize]
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

            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_samples_select", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter sqlParameter01 = new SqlParameter("type",3);   //2
            sqlParameter01.IsNullable = false;

            SqlParameter sqlParameter02 = new SqlParameter("is_id", id);
            sqlParameter02.IsNullable = false;

            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            //List<SpIndividualsSamples> list = new List<SpIndividualsSamples>();
            SpIndividualsSamples SpIndividualsSamples = new SpIndividualsSamples();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                //SpIndividualsSamples item = new SpIndividualsSamples();
                DataRow dr = dataTable.Rows[i];

                SpIndividualsSamples.is_id = Int32.Parse(dr["is_id"].ToString());
                SpIndividualsSamples.is_barcode = dr["is_barcode"].ToString();
                SpIndividualsSamples.is_date_created_text = dr["is_date_created_text"].ToString();
                SpIndividualsSamples.is_date_collected_text = dr["is_date_collected_text"].ToString();
                SpIndividualsSamples.is_date_registered_text = dr["is_date_registered_text"].ToString();
                SpIndividualsSamples.ind_id = Int32.Parse(dr["ind_id"].ToString());
                SpIndividualsSamples.ind_first_name = dr["ind_first_name"].ToString();
                SpIndividualsSamples.ind_last_name = dr["ind_last_name"].ToString();
                SpIndividualsSamples.ind_gender = dr["ind_gender"].ToString();
                SpIndividualsSamples.ind_document = dr["ind_document"].ToString();
                SpIndividualsSamples.is_well_number = dr["is_well_number"].ToString();
                SpIndividualsSamples.poo_id = dr["poo_id"] is DBNull ? (int?)null : (int?)Int32.Parse(dr["poo_id"].ToString());
                SpIndividualsSamples.is_details = dr["is_details"].ToString();
                SpIndividualsSamples.ref_id = Int32.Parse(dr["ref_id"].ToString());
                SpIndividualsSamples.ref_name = dr["ref_name"].ToString();
                SpIndividualsSamples.std_id = Int32.Parse(dr["std_id"].ToString());
                SpIndividualsSamples.std_name = dr["std_name"].ToString();

                //list.Add(item);
            }


            //SpIndividuals SpIndividuals = new SpIndividuals();
            //ViewData["SpIndividuals"] = SpIndividuals.GetIndividualbyId(SpIndividualsSamples.ind_id);

            return View(SpIndividualsSamples);
        }

        [Authorize]
        public IActionResult Index(String search)
        {
            //SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_samples_select", Globals.connection);
            //dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;

            //System.Data.DataTable dataTable = new System.Data.DataTable();

            //dataAdapter.Fill(dataTable);

            //List<SpIndividualsSamples> list = new List<SpIndividualsSamples>();


            //for (int i = 0; i < dataTable.Rows.Count; i++)
            //{
            //    SpIndividualsSamples item = new SpIndividualsSamples();
            //    DataRow dr = dataTable.Rows[i];

            //    item.is_id = Int32.Parse(dr["is_id"].ToString());
            //    item.is_barcode = dr["is_barcode"].ToString();
            //    item.is_date_created_text = dr["is_date_created_text"].ToString();
            //    item.is_date_collected_text = dr["is_date_collected_text"].ToString();
            //    item.is_date_registered_text = dr["is_date_registered_text"].ToString();
            //    item.ind_id = Int32.Parse(dr["ind_id"].ToString());
            //    item.ind_first_name = dr["ind_first_name"].ToString();
            //    item.ind_last_name = dr["ind_last_name"].ToString();
            //    item.ind_gender = dr["ind_gender"].ToString();
            //    item.ind_document = dr["ind_document"].ToString();
            //    item.is_well_number = dr["is_well_number"].ToString();
            //    item.poo_id =  dr["poo_id"] is DBNull ? (int?) null  : (int?)Int32.Parse(dr["poo_id"].ToString());
            //    item.is_details = dr["is_details"].ToString();
            //    item.ref_id = Int32.Parse(dr["ref_id"].ToString());
            //    item.ref_name = dr["ref_name"].ToString();
            //    item.std_id = Int32.Parse(dr["std_id"].ToString());
            //    item.std_name = dr["std_name"].ToString();

            //    list.Add(item);
            //}

            //return View(list);

            //SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_samples_select", Globals.connection);
            //dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;

            //System.Data.DataTable dataTable = new System.Data.DataTable();

            //dataAdapter.Fill(dataTable);

            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "usp_individuals_samples_select";

            SqlParameter sqlParameter01 = new SqlParameter("search", Globals.Iif(search is null, "", search));
            sqlParameter01.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter01);

            SqlParameter sqlParameter02 = new SqlParameter("type", 8);
            sqlParameter02.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter02);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand); //I think the problem is here, how am I fixing it though? --Fixed but why can't it be put into a class?
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
                item.is_details = dr["is_details"].ToString();
                item.ref_id = Int32.Parse(dr["ref_id"].ToString());
                item.ref_name = dr["ref_name"].ToString();
                item.std_id = Int32.Parse(dr["std_id"].ToString());
                item.std_name = dr["std_name"].ToString();

                list.Add(item);
            }

            return View(list);
        }



        [Authorize]
        public async Task<IActionResult> Delete(int? id, String search)
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


            SpIndividuals spIndividuals = new SpIndividuals();
            ViewData["spIndividuals"] = spIndividuals.GetIndividualbyId(individualSamples.ind_id);
            Globals.search = search;

            return View(individualSamples);
        }


        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var individualSamples = await _context.tb_individuals_samples.FindAsync(id);
            //_context.tb_individuals_samples.Remove(individualSamples);
            //await _context.SaveChangesAsync();
            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "usp_individuals_samples_delete";

            SqlParameter sqlParameter01 = new SqlParameter("is_id", id);
            sqlParameter01.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter01);

            SqlParameter sqlParameter02 = new SqlParameter("usr_id_audit", Globals.currentUserId);
            sqlParameter02.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter02);

            SqlParameter sqlParameter03 = new SqlParameter("ssn_id", Globals.sessionId);
            sqlParameter03.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter03);

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            return RedirectToAction(nameof(Index), new { search = Globals.search });
        }



        [Authorize]
        public async Task<IActionResult> Edit(int? id, String search)
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

            SpIndividuals spIndividuals = new SpIndividuals();
            ViewData["spIndividuals"] = spIndividuals.GetAllIndividuals();
            Globals.search = search;

            return View(individualSamples);
        }



        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("is_id, is_date_collected, is_time_collected, usr_id_collected, is_date_registered, is_time_registered, usr_id_registered, ind_id, is_well_number, is_details")] IndividualSample individualSample)
        {
            if (id != individualSample.is_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(individualSample);
                    //await _context.SaveChangesAsync();

                    SqlCommand sqlCommand = new SqlCommand();
                    SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "usp_individuals_samples_update";

                    SqlParameter sqlParameter01 = new SqlParameter("type", 1);
                    sqlParameter01.IsNullable = false;
                    sqlCommand.Parameters.Add(sqlParameter01);

                    SqlParameter sqlParameter02 = new SqlParameter("is_id", individualSample.is_id);
                    sqlParameter02.IsNullable = false;
                    sqlCommand.Parameters.Add(sqlParameter02);

                    SqlParameter sqlParameter03 = new SqlParameter("is_date_collected", individualSample.is_date_collected);
                    sqlParameter03.IsNullable = false;
                    sqlCommand.Parameters.Add(sqlParameter03);

                    SqlParameter sqlParameter04 = new SqlParameter("is_time_collected", individualSample.is_time_collected);
                    sqlParameter04.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter04);

                    SqlParameter sqlParameter05 = new SqlParameter("usr_id_collected", individualSample.usr_id_collected);
                    sqlParameter05.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter05);


                    SqlParameter sqlParameter06 = new SqlParameter("is_date_registered", individualSample.is_date_registered);
                    sqlParameter06.IsNullable = false;
                    sqlCommand.Parameters.Add(sqlParameter06);

                    if  (individualSample.is_date_registered == null)
                    {
                        SqlParameter sqlParameter07 = new SqlParameter("is_time_registered", individualSample.is_time_registered);
                        sqlParameter07.IsNullable = true;
                        sqlCommand.Parameters.Add(sqlParameter07);

                        SqlParameter sqlParameter08 = new SqlParameter("usr_id_registered", individualSample.usr_id_registered);
                        sqlParameter08.IsNullable = true;
                        sqlCommand.Parameters.Add(sqlParameter08);
                    }
                    else
                    {
                        if (individualSample.is_time_registered == null)
                        {
                            SqlParameter sqlParameter07 = new SqlParameter("is_time_registered", String.Empty);
                            sqlParameter07.IsNullable = true;
                            sqlCommand.Parameters.Add(sqlParameter07);

                            SqlParameter sqlParameter08 = new SqlParameter("usr_id_registered", String.Empty);
                            sqlParameter08.IsNullable = true;
                            sqlCommand.Parameters.Add(sqlParameter08);
                        }
                        else 
                        {
                            SqlParameter sqlParameter07 = new SqlParameter("is_time_registered", individualSample.is_time_registered);
                            sqlParameter07.IsNullable = true;
                            sqlCommand.Parameters.Add(sqlParameter07);

                            SqlParameter sqlParameter08 = new SqlParameter("usr_id_registered", individualSample.usr_id_registered);
                            sqlParameter08.IsNullable = true;
                            sqlCommand.Parameters.Add(sqlParameter08);
                        }
                    }

                    SqlParameter sqlParameter09 = new SqlParameter("ind_id", individualSample.ind_id);
                    sqlParameter09.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter09);

                    SqlParameter sqlParameter10 = new SqlParameter("is_well_number", individualSample.is_well_number);
                    sqlParameter10.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter10);

                    SqlParameter sqlParameter11 = new SqlParameter("is_details", individualSample.is_details);
                    sqlParameter11.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter11);

                    SqlParameter sqlParameter12 = new SqlParameter("usr_id_audit", Globals.currentUserId);
                    sqlParameter12.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter12);

                    SqlParameter sqlParameter13 = new SqlParameter("ssn_id", Globals.sessionId);
                    sqlParameter13.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter13);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();


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
                return RedirectToAction(nameof(Index), new { search = Globals.search });
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
            sqlCommand.CommandText = "usp_individuals_samples_update";

            SqlParameter sqlParameter01 = new SqlParameter("type", 4);
            sqlParameter01.IsNullable = false;
            sqlCommand.Parameters.Add(sqlParameter01);

            SqlParameter sqlParameter02 = new SqlParameter("is_id", id);
            sqlParameter02.IsNullable = false;
            sqlCommand.Parameters.Add(sqlParameter02);

            SqlParameter sqlParameter03 = new SqlParameter("is_well_number", value);//Globals.Iif(value is null, "", value));
            sqlParameter03.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter03);

            SqlParameter sqlParameter04 = new SqlParameter("usr_id_audit", Globals.currentUserId);//Globals.Iif(value is null, "", value));
            sqlParameter04.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter04);

            SqlParameter sqlParameter05 = new SqlParameter("ssn_id", Globals.sessionId);//Globals.Iif(value is null, "", value));
            sqlParameter05.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter05);

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            return value;
        }



        [Authorize]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

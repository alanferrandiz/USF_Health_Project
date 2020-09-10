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
    public class EnvironmentalSamplesController : Controller
    {
       
        private readonly USF_Health_MVC_EFContext _context;
        public EnvironmentalSamplesController(USF_Health_MVC_EFContext context)
        {
            _context = context;
        }

        [Authorize("usfhealth_laboratory")]
        public ActionResult Details(int? id)
        {

            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_places_samples_select", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter sqlParameter01 = new SqlParameter("type",3);   //2
            sqlParameter01.IsNullable = false;

            SqlParameter sqlParameter02 = new SqlParameter("ps_id", id);
            sqlParameter02.IsNullable = false;

            dataAdapter.SelectCommand.Parameters.Add(sqlParameter01);
            dataAdapter.SelectCommand.Parameters.Add(sqlParameter02);

            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            SpPlacesSamples spPlacesSamples = new SpPlacesSamples();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                //SpIndividualsSamples item = new SpIndividualsSamples();
                DataRow dr = dataTable.Rows[i];

                spPlacesSamples.ps_id = Int32.Parse(dr["ps_id"].ToString());
                spPlacesSamples.ps_barcode = dr["ps_barcode"].ToString();
                spPlacesSamples.ps_date_created_text = dr["ps_date_created_text"].ToString();
                spPlacesSamples.ps_date_collected_text = dr["ps_date_collected_text"].ToString();
                spPlacesSamples.ps_date_registered_text = dr["ps_date_registered_text"].ToString();
                spPlacesSamples.pla_id = Int32.Parse(dr["pla_id"].ToString());
                spPlacesSamples.pla_name = dr["pla_name"].ToString();
                spPlacesSamples.pla_location_reference = dr["pla_location_reference"].ToString();
                spPlacesSamples.pla_campus = dr["pla_campus"].ToString();
                spPlacesSamples.pla_details = dr["pla_details"].ToString();
                spPlacesSamples.ps_well_number = dr["ps_well_number"].ToString();
                spPlacesSamples.psres_result = dr["psres_result"].ToString();
                spPlacesSamples.psres_ct_value = dr["psres_ct_value"].ToString();
                spPlacesSamples.ps_details = dr["ps_details"].ToString();


                //list.Add(item);
            }


            //SpIndividuals SpIndividuals = new SpIndividuals();
            //ViewData["SpIndividuals"] = SpIndividuals.GetIndividualbyId(SpIndividualsSamples.ind_id);

            return View(spPlacesSamples);
        }

        [Authorize("usfhealth_laboratory")]
        public IActionResult Index()
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
                item.psres_result = dr["psres_result"].ToString();
                item.psres_ct_value = dr["psres_ct_value"].ToString();
                item.ps_details = dr["ps_details"].ToString();

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

            var placesSamples = await _context.tb_places_samples
                .FirstOrDefaultAsync(m => m.ps_id == id);
            if (placesSamples == null)
            {
                return NotFound();
            }


            SpPlaces spPlaces = new SpPlaces();
            ViewData["spPlaces"] = spPlaces.GetPlacebyId(placesSamples.pla_id);

            return View(placesSamples);
        }


        [Authorize("usfhealth_laboratory")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var placeSample = await _context.tb_places_samples.FindAsync(id);
            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "usp_places_samples_delete";

            SqlParameter sqlParameter01 = new SqlParameter("ps_id", id);
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


            return RedirectToAction(nameof(Index));
        }



        [Authorize("usfhealth_laboratory")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placeSample = await _context.tb_places_samples.FindAsync(id);
       
            if (placeSample == null)
            {
                return NotFound();
            }

            SpPlaces spPlaces = new SpPlaces();
            ViewData["spPlaces"] = spPlaces.GetPlacebyId(placeSample.pla_id);

            return View(placeSample);
        }



        [Authorize("usfhealth_laboratory")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ps_id, ps_date_collected, ps_time_collected, usr_id_collected, ps_date_registered, ps_time_registered, usr_id_registered, pla_id, ps_well_number, ps_details")] PlaceSample placeSample)
        {
            if (id != placeSample.ps_id)
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
                    sqlCommand.CommandText = "usp_places_samples_update";

                    SqlParameter sqlParameter01 = new SqlParameter("type", 1);
                    sqlParameter01.IsNullable = false;
                    sqlCommand.Parameters.Add(sqlParameter01);

                    SqlParameter sqlParameter02 = new SqlParameter("ps_id", placeSample.ps_id);
                    sqlParameter02.IsNullable = false;
                    sqlCommand.Parameters.Add(sqlParameter02);

                    SqlParameter sqlParameter03 = new SqlParameter("ps_date_collected", placeSample.ps_date_collected);
                    sqlParameter03.IsNullable = false;
                    sqlCommand.Parameters.Add(sqlParameter03);

                    SqlParameter sqlParameter04 = new SqlParameter("ps_time_collected", placeSample.ps_time_collected);
                    sqlParameter04.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter04);

                    SqlParameter sqlParameter05 = new SqlParameter("usr_id_collected", placeSample.usr_id_collected);
                    sqlParameter05.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter05);


                    SqlParameter sqlParameter06 = new SqlParameter("ps_date_registered", placeSample.ps_date_registered);
                    sqlParameter06.IsNullable = false;
                    sqlCommand.Parameters.Add(sqlParameter06);

                    if  (placeSample.ps_date_registered == null)
                    {
                        SqlParameter sqlParameter07 = new SqlParameter("ps_time_registered", placeSample.ps_time_registered);
                        sqlParameter07.IsNullable = true;
                        sqlCommand.Parameters.Add(sqlParameter07);

                        SqlParameter sqlParameter08 = new SqlParameter("usr_id_registered", placeSample.usr_id_registered);
                        sqlParameter08.IsNullable = true;
                        sqlCommand.Parameters.Add(sqlParameter08);
                    }
                    else
                    {
                        if (placeSample.ps_time_registered == null)
                        {
                            SqlParameter sqlParameter07 = new SqlParameter("ps_time_registered", String.Empty);
                            sqlParameter07.IsNullable = true;
                            sqlCommand.Parameters.Add(sqlParameter07);

                            SqlParameter sqlParameter08 = new SqlParameter("usr_id_registered", String.Empty);
                            sqlParameter08.IsNullable = true;
                            sqlCommand.Parameters.Add(sqlParameter08);
                        }
                        else 
                        {
                            SqlParameter sqlParameter07 = new SqlParameter("ps_time_registered", placeSample.ps_time_registered);
                            sqlParameter07.IsNullable = true;
                            sqlCommand.Parameters.Add(sqlParameter07);

                            SqlParameter sqlParameter08 = new SqlParameter("usr_id_registered", placeSample.usr_id_registered);
                            sqlParameter08.IsNullable = true;
                            sqlCommand.Parameters.Add(sqlParameter08);
                        }
                    }

                    SqlParameter sqlParameter09 = new SqlParameter("pla_id", placeSample.pla_id);
                    sqlParameter09.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter09);

                    SqlParameter sqlParameter10 = new SqlParameter("ps_well_number", placeSample.ps_well_number);
                    sqlParameter10.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter10);

                    SqlParameter sqlParameter11 = new SqlParameter("ps_details", placeSample.ps_details);
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
                    if (!PlaceSamplesExists(placeSample.ps_id))
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

            return View(placeSample);
        }

        private bool PlaceSamplesExists(int id)
        {
            return _context.tb_places_samples.Any(e => e.ps_id == id);
        }





        [HttpGet]
        public string UpdateWellNumber(int id, string? value)
        {

            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "usp_places_samples_update";

            SqlParameter sqlParameter01 = new SqlParameter("type", 4);
            sqlParameter01.IsNullable = false;
            sqlCommand.Parameters.Add(sqlParameter01);

            SqlParameter sqlParameter02 = new SqlParameter("ps_id", id);
            sqlParameter02.IsNullable = false;
            sqlCommand.Parameters.Add(sqlParameter02);

            SqlParameter sqlParameter03 = new SqlParameter("ps_well_number", value);//Globals.Iif(value is null, "", value));
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


        [HttpGet]
        public string UpdateResult(int id, string? value)
        {

            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "usp_places_samples_results_update";

            SqlParameter sqlParameter01 = new SqlParameter("type", 2);
            sqlParameter01.IsNullable = false;
            sqlCommand.Parameters.Add(sqlParameter01);

            SqlParameter sqlParameter02 = new SqlParameter("ps_id", id);
            sqlParameter02.IsNullable = false;
            sqlCommand.Parameters.Add(sqlParameter02);

            SqlParameter sqlParameter03 = new SqlParameter("psres_value", value);//Globals.Iif(value is null, "", value));
            sqlParameter03.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter03);

            SqlParameter sqlParameter04 = new SqlParameter("usr_id", Globals.currentUserId);//Globals.Iif(value is null, "", value));
            sqlParameter04.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter04);

            SqlParameter sqlParameter05 = new SqlParameter("usr_id_audit", Globals.currentUserId);//Globals.Iif(value is null, "", value));
            sqlParameter04.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter05);

            SqlParameter sqlParameter06 = new SqlParameter("ssn_id", Globals.sessionId);//Globals.Iif(value is null, "", value));
            sqlParameter05.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter06);

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            return value;
        }


        [HttpGet]
        public string UpdateCTValue(int id, string? value)
        {

            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "usp_places_samples_results_update";

            SqlParameter sqlParameter01 = new SqlParameter("type", 1);
            sqlParameter01.IsNullable = false;
            sqlCommand.Parameters.Add(sqlParameter01);

            SqlParameter sqlParameter02 = new SqlParameter("ps_id", id);
            sqlParameter02.IsNullable = false;
            sqlCommand.Parameters.Add(sqlParameter02);

            SqlParameter sqlParameter03 = new SqlParameter("psres_value", value);//Globals.Iif(value is null, "", value));
            sqlParameter03.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter03);

            SqlParameter sqlParameter04 = new SqlParameter("usr_id", Globals.currentUserId);//Globals.Iif(value is null, "", value));
            sqlParameter04.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter04);

            SqlParameter sqlParameter05 = new SqlParameter("usr_id_audit", Globals.currentUserId);//Globals.Iif(value is null, "", value));
            sqlParameter04.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter05);

            SqlParameter sqlParameter06 = new SqlParameter("ssn_id", Globals.sessionId);//Globals.Iif(value is null, "", value));
            sqlParameter05.IsNullable = true;
            sqlCommand.Parameters.Add(sqlParameter06);

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

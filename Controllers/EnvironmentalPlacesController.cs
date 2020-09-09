using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using USF_Health_MVC_EF.Models;

namespace USF_Health_MVC_EF.Controllers
{
    public class EnvironmentalPlacesController : Controller
    {

   private readonly USF_Health_MVC_EFContext _context;

        public EnvironmentalPlacesController(USF_Health_MVC_EFContext context)
        {
            _context = context;
        }

        [Authorize("usfhealth_laboratory")]
        public  ActionResult Index()
        {

            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_places_select", Globals.connection);
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            List<SpPlaces> list = new List<SpPlaces>();
          
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {

                SpPlaces item = new SpPlaces();
                DataRow dr = dataTable.Rows[i];

                item.pla_id = Int32.Parse(dr["pla_id"].ToString());
                item.pla_date_created = dr["pla_date_created"] is DBNull ? (DateTime?)null : (DateTime?)dr["pla_date_created"];
                item.pla_time_created = dr["pla_time_created"] is DBNull ? (TimeSpan?)null : (TimeSpan?)dr["pla_time_created"];
                item.pla_date_created_text = dr["pla_date_created_text"].ToString();
                item.usr_id_created = Int32.Parse(dr["usr_id_created"].ToString());
                item.pla_name = dr["pla_name"].ToString();
                item.name_id = dr["name_id"].ToString();
                item.pla_location_reference = dr["pla_location_reference"].ToString();
                item.pla_campus = dr["pla_campus"].ToString();
                item.pla_details = dr["pla_details"].ToString();

                list.Add(item);
            }

            return View(list);

        }

        [Authorize("usfhealth_laboratory")]
        public IActionResult Create()
        {

            return View();
        }

        [Authorize("usfhealth_laboratory")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("pla_id,pla_date_created,pla_time_created,usr_id_created,pla_name,pla_location_reference,pla_campus,pla_details")] Place place)
        {
            if (ModelState.IsValid)
            {

                SqlCommand sqlCommand = new SqlCommand();
                SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "usp_places_insert";

                SqlParameter sqlParameter01 = new SqlParameter("usr_id_created", Globals.currentUserId);
                sqlParameter01.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter01);

                SqlParameter sqlParameter02 = new SqlParameter("pla_name", place.pla_name);
                sqlParameter02.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter02);

                SqlParameter sqlParameter03 = new SqlParameter("pla_location_reference", place.pla_location_reference);
                sqlParameter03.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter03);

                SqlParameter sqlParameter04 = new SqlParameter("pla_campus", place.pla_campus);
                sqlParameter04.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter04);

                SqlParameter sqlParameter05 = new SqlParameter("pla_details", place.pla_details);
                sqlParameter05.IsNullable = true;
                sqlCommand.Parameters.Add(sqlParameter05);
        
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return RedirectToAction(nameof(Index));

            }
            return View(place);
        }


        [Authorize("usfhealth_laboratory")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _context.tb_places
                .FirstOrDefaultAsync(m => m.pla_id == id);
            if (place == null)
            {
                return NotFound();
            }

            return View(place);
        }




        [Authorize("usfhealth_laboratory")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _context.tb_places.FindAsync(id);
            if (place == null)
            {
                return NotFound();
            }

            return View(place);
        }

        [Authorize("usfhealth_laboratory")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("pla_id,pla_date_created,pla_time_created,usr_id_created,pla_name,pla_location_reference,pla_campus,pla_details")] Place place)
        {
            if (id != place.pla_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(individual);
                    //await _context.SaveChangesAsync();

                    SqlCommand sqlCommand = new SqlCommand();
                    SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = "usp_places_update";

                    SqlParameter sqlParameter01 = new SqlParameter("pla_id", place.pla_id);
                    sqlParameter01.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter01);

                    SqlParameter sqlParameter02 = new SqlParameter("pla_name", place.pla_name);
                    sqlParameter02.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter02);

                    SqlParameter sqlParameter03 = new SqlParameter("pla_location_reference", place.pla_location_reference);
                    sqlParameter03.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter03);

                    SqlParameter sqlParameter04 = new SqlParameter("pla_campus", place.pla_campus);
                    sqlParameter04.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter04);

                    SqlParameter sqlParameter05 = new SqlParameter("pla_details", place.pla_details);
                    sqlParameter05.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter05);

                    SqlParameter sqlParameter06 = new SqlParameter("usr_id_audit", Globals.currentUserId);
                    sqlParameter06.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter06);

                    SqlParameter sqlParameter07 = new SqlParameter("ssn_id", Globals.sessionId);
                    sqlParameter07.IsNullable = true;
                    sqlCommand.Parameters.Add(sqlParameter07);


                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaceExists(place.pla_id))
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

            return View(place);
        }

        [Authorize("usfhealth_laboratory")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var place = await _context.tb_places
                .FirstOrDefaultAsync(m => m.pla_id == id);
            if (place == null)
            {
                return NotFound();
            }

            return View(place);
        }

        [Authorize("usfhealth_laboratory")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            

            var places = await _context.tb_places.FindAsync(id);


            SqlCommand sqlCommand = new SqlCommand();
            SqlConnection sqlConnection = new SqlConnection(Globals.connection.ToString());
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "usp_places_delete";

            SqlParameter sqlParameter01 = new SqlParameter("pla_id", id);
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

            //_context.tb_individuals.Remove(individual);
            //await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PlaceExists(int id)
        {
            return _context.tb_places.Any(e => e.pla_id == id);
        }



    }
}
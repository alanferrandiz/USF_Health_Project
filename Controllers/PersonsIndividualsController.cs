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
    public class PersonsIndividualsController : Controller
    {

   private readonly USF_Health_MVC_EFContext _context;

        public PersonsIndividualsController(USF_Health_MVC_EFContext context)
        {
            _context = context;
        }

        [Authorize("usfhealth_athletics")]    
        public  ActionResult Index()
        {

            SqlDataAdapter dataAdapter = new SqlDataAdapter("usp_individuals_select_with_stats", Globals.connection); //I think the problem is here, how am I fixing it though? --Fixed but why can't it be put into a class?
            dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            System.Data.DataTable dataTable = new System.Data.DataTable();

            dataAdapter.Fill(dataTable);

            List<SpIndividualsWithStats> list = new List<SpIndividualsWithStats>();
          
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                
                SpIndividualsWithStats item = new SpIndividualsWithStats();
                DataRow dr = dataTable.Rows[i];

                item.ind_id = Int32.Parse(dr["ind_id"].ToString());
                item.ind_date_created_text = dr["ind_date_created_text"].ToString();
                item.ind_first_name = dr["ind_first_name"].ToString();
                item.ind_last_name = dr["ind_last_name"].ToString();
                item.ind_email = dr["ind_email"].ToString();
                item.ind_phone = dr["ind_phone"].ToString();
                item.ind_gender = dr["ind_gender"].ToString();
                item.ind_document = dr["ind_document"].ToString();
                item.ref_id = Int32.Parse(dr["ref_id"].ToString());
                item.ref_name = dr["ref_name"].ToString();
                item.std_id = Int32.Parse(dr["std_id"].ToString());
                item.std_name = dr["std_name"].ToString();
                item.ind_details = dr["ind_details"].ToString();

                list.Add(item);
            }

            return View(list);

        }

        [Authorize("usfhealth_athletics")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var individual = await _context.tb_individuals
                .FirstOrDefaultAsync(m => m.ind_id == id);
            if (individual == null)
            {
                return NotFound();
            }

            SpStudies spStudies = new SpStudies();
            SpReferences spReferences = new SpReferences();

            ViewData["studies"] = spStudies.GetAllStudies(2, individual.std_id);
            ViewData["references"] = spReferences.GetAllReferences(2, individual.ref_id);
            return View(individual);
        }

        [Authorize("usfhealth_athletics")]
        public IActionResult Create()
        {

            SpStudies spStudies = new SpStudies();
            SpReferences spReferences = new SpReferences();

            ViewData["studies"] = spStudies.GetAllStudies(1,0);
            ViewData["references"] = spReferences.GetAllReferences(1, 0);

            return View();
        }

        [Authorize("usfhealth_athletics")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ind_id,ind_date_created,ind_time_created,ind_first_name,ind_last_name,ind_email,ind_phone,ind_birthdate,ind_gender,ind_document,ref_id,std_id,ind_details")] Individual individual)
        {
            if (ModelState.IsValid)
            {
                _context.Add(individual);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(individual);
        }

        [Authorize("usfhealth_athletics")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var individual = await _context.tb_individuals.FindAsync(id);
            if (individual == null)
            {
                return NotFound();
            }
            SpStudies spStudies = new SpStudies();
            SpReferences spReferences = new SpReferences();
            ViewData["studies"] = spStudies.GetAllStudies(2,individual.std_id);
            ViewData["references"] = spReferences.GetAllReferences(2, individual.ref_id);

            return View(individual);
        }

        [Authorize("usfhealth_athletics")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ind_id,ind_date_created,ind_time_created,ind_first_name,ind_last_name,ind_email,ind_phone,ind_birthdate,ind_gender,ind_document,ref_id,std_id,indcat_id,ind_details")] Individual individual)
        {
            if (id != individual.ind_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(individual);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IndividualExists(individual.ind_id))
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
            SpStudies spStudies = new SpStudies();
            SpReferences spReferences = new SpReferences();

            ViewData["studies"] = spStudies.GetAllStudies(2, individual.std_id);
            ViewData["references"] = spReferences.GetAllReferences(2, individual.ref_id);

            return View(individual);
        }

        [Authorize("usfhealth_athletics")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var individual = await _context.tb_individuals
                .FirstOrDefaultAsync(m => m.ind_id == id);
            if (individual == null)
            {
                return NotFound();
            }

            SpStudies spStudies = new SpStudies();
            SpReferences spReferences = new SpReferences();

            ViewData["studies"] = spStudies.GetAllStudies(2,individual.std_id);
            ViewData["references"] = spReferences.GetAllReferences(2, individual.ref_id);

            return View(individual);
        }

        [Authorize("usfhealth_athletics")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            

            var individual = await _context.tb_individuals.FindAsync(id);

            SpStudies spStudies = new SpStudies();
            SpReferences spReferences = new SpReferences();

            ViewData["studies"] = spStudies.GetAllStudies(2, individual.std_id);
            ViewData["references"] = spReferences.GetAllReferences(2, individual.ref_id);


            _context.tb_individuals.Remove(individual);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IndividualExists(int id)
        {
            return _context.tb_individuals.Any(e => e.ind_id == id);
        }



    }
}
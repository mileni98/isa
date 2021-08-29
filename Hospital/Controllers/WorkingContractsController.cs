﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hospital.Data;
using Hospital.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hospital.Controllers
{
    public class WorkingContractsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkingContractsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WorkingContracts
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.WorkingContract.ToListAsync());
        }

        // GET: WorkingContracts/Details/5
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workingContract = await _context.WorkingContract
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workingContract == null)
            {
                return NotFound();
            }

            return View(workingContract);
        }

        // GET: WorkingContracts/Create
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult Create()
        {
            ViewData["pharmacies"] = _context.Pharmacy;
            ViewData["workers"] = _context.Users;
            return View();
        }

        // POST: WorkingContracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Create([Bind("WorkerId,PharmacyId,StartTime,EndTime,WorkTimeStart,WorkTimeEnd,Id,RowVersion")] WorkingContract workingContract)
        {
            if (ModelState.IsValid)
            {
                workingContract.Id = Guid.NewGuid();
                _context.Add(workingContract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workingContract);
        }

        // GET: WorkingContracts/Edit/5
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workingContract = await _context.WorkingContract.FindAsync(id);
            if (workingContract == null)
            {
                return NotFound();
            }
            return View(workingContract);
        }

        // POST: WorkingContracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Edit(Guid id, [Bind("WorkerId,PharmacyId,StartTime,EndTime,WorkTimeStart,WorkTimeEnd,Id,RowVersion")] WorkingContract workingContract)
        {
            if (id != workingContract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workingContract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkingContractExists(workingContract.Id))
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
            return View(workingContract);
        }

        // GET: WorkingContracts/Delete/5
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workingContract = await _context.WorkingContract
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workingContract == null)
            {
                return NotFound();
            }

            return View(workingContract);
        }

        // POST: WorkingContracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var workingContract = await _context.WorkingContract.FindAsync(id);
            _context.WorkingContract.Remove(workingContract);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkingContractExists(Guid id)
        {
            return _context.WorkingContract.Any(e => e.Id == id);
        }
    }
}
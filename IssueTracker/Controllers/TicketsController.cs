﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IssueTracker.Data;
using IssueTracker.Models;

namespace IssueTracker.Controllers
{
    public class TicketsController : Controller
    {
        private readonly IssueTrackerContext _context;

        public TicketsController(IssueTrackerContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tickets.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tickets = await _context.Tickets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tickets == null)
            {
                return NotFound();
            }

            return View(tickets);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            var ticket = new Ticket { CreationDate = DateTime.Today, SeverityLevel=Enums.SeverityEnum.Normal};
            return View(ticket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CreationDate,Creator,SeverityLevel")] Ticket tickets)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tickets);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tickets);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tickets = await _context.Tickets.FindAsync(id);
            if (tickets == null)
            {
                return NotFound();
            }
            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CreationDate,Creator,SeverityLevel")] Ticket tickets)
        {
            if (id != tickets.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tickets);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketsExists(tickets.Id))
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
            return View(tickets);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tickets = await _context.Tickets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tickets == null)
            {
                return NotFound();
            }

            return View(tickets);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tickets = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(tickets);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketsExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
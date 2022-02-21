#nullable disable
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
        public async Task<IActionResult> Index(string Creator, string searchString)
        {
         IQueryable<User> creatorQuery = from u in _context.Users
                                             orderby u.Name
                                             select u;

         var tickets = from ticket in _context.Tickets
                          select ticket;

            if (!string.IsNullOrEmpty(searchString))
            {
                tickets = tickets.Where(ticket => ticket.Name.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(Creator))
            {
                var selectedCreator = (from u in _context.Users
                                       where u.Name == Creator
                                       select u).First();

                tickets = tickets.Where(x => x.Creator == selectedCreator);
            }

            var creatorVM = new FilterViewModel
            {
                Creators = new SelectList(await creatorQuery.Distinct().ToListAsync()),
                Tickets = await tickets.ToListAsync(),
            };

            return View(creatorVM);
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
        public async Task<IActionResult> Create()
        {
            IQueryable<User> creatorQuery = from u in _context.Users
                                            orderby u.Name
                                            select u;
            var creatorsList = new SelectList(await creatorQuery.Distinct().ToListAsync());
            ViewData["CreatorsList"] = creatorsList;

            var ticket = new Ticket { CreationDate = DateTime.Today, SeverityLevel=Enums.SeverityEnum.Normal, Status = Enums.StatusEnum.New};
            return View(ticket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CreationDate,Creator,SeverityLevel,Status")] Ticket tickets, string selectedCreator)
        {
            if (ModelState.IsValid)
            {
                var creator = (from u in _context.Users
                                       where u.Name == selectedCreator
                                       select u).First();
                tickets.Creator = creator;
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

            IQueryable<User> creatorQuery = from u in _context.Users
                                            orderby u.Name
                                            select u;
            var creatorsList = new SelectList(await creatorQuery.Distinct().ToListAsync());
            ViewData["CreatorsList"] = creatorsList;

            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CreationDate,Creator,SeverityLevel,Status")] Ticket tickets, string selectedCreator)
        {
            if (id != tickets.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var creator = (from u in _context.Users
                                   where u.Name == selectedCreator
                                   select u).First();
                    tickets.Creator = creator;
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

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
using Microsoft.AspNetCore.Authorization;

namespace IssueTracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly IssueTrackerContext _context;

        public TicketsController(IssueTrackerContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(string Creator, string Assignee, string searchString)
        {
         var tickets = from ticket in _context.Tickets
                          select ticket;

            if (!string.IsNullOrEmpty(searchString))
            {
                tickets = tickets.Where(ticket => ticket.Subject.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(Creator))
            {
                User selectedCreator = GetSelectedUserByName(Creator);

                tickets = tickets.Where(x => x.Creator == selectedCreator);
            }

            if (!string.IsNullOrEmpty(Assignee))
            {
                User selectedCreator = GetSelectedUserByName(Assignee);

                tickets = tickets.Where(x => x.Assignee == selectedCreator);
            }

            var userSelectList = await MakeUserSelectList();

            var creatorVM = new FilterViewModel
            {
                Creators = userSelectList,
                Assignees = userSelectList,
                Tickets = await tickets.ToListAsync(),
                SearchString = searchString
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
            //Run this method to populate Creator property in ticket data. Data does not need to be retained.
            await MakeUserSelectList();

            return View(tickets);
        }

        // GET: Tickets/Create
        public async Task<IActionResult> Create()
        {
            var usersList = MakeUserSelectList();
            ViewData["UsersList"] = await usersList;

            var ticket = new Ticket { CreationDate = DateTime.Today, SeverityLevel = Enums.SeverityEnum.Normal, Status = Enums.StatusEnum.New };
            return View(ticket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Subject,Description,CreationDate,Creator,Assignee,SeverityLevel,Status")] Ticket tickets, string selectedCreator, string assignedUser)
        {
             if (ModelState.IsValid)
            {
                var creator = GetSelectedUserByName(selectedCreator);
                var assignee = GetSelectedUserByName(assignedUser);
                tickets.Creator = creator;
                tickets.Assignee = assignee;
                tickets.Status = Enums.StatusEnum.New;

                _context.Add(tickets);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //Ticket creation invalid. Return initial create page to fill user dropdown, default date, status.
            return await Create();
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

            var usersList = MakeUserSelectList();
            ViewData["UsersList"] = await usersList;

            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Subject,Description,CreationDate,Creator,Assignee,SeverityLevel,Status")] Ticket tickets, string selectedCreator, string assignedUser)
        {
            if (id != tickets.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    SetTicketCreatorAssignee(tickets, selectedCreator, assignedUser);

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
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
                return RedirectToAction(nameof(Index));
            }

            //Ticket edit invalid. Return initial edit page to pre-fill fields.
            return await Edit(id);
        }

        private void SetTicketCreatorAssignee(Ticket tickets, string selectedCreator, string assignedUser)
        {
            var creator = GetSelectedUserByName(selectedCreator);
            var assignee = GetSelectedUserByName(assignedUser);
            tickets.Creator = creator;
            tickets.Assignee = assignee;
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
            //Run this method to populate Creator property in ticket data. Data does not need to be retained.
            await MakeUserSelectList();

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

        private async Task<SelectList> MakeUserSelectList()
        {
            IQueryable<User> creatorQuery = from u in _context.Users
                                            orderby u.Name
                                            select u;
            var creatorsList = new SelectList(await creatorQuery.Distinct().ToListAsync());
            return creatorsList;
        }

        private User GetSelectedUserByName(string name)
        {
            return (from u in _context.Users
                    where u.Name == name
                    select u).First();
        }
    }
}

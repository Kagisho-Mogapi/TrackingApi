﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tracking.WebApp.Data;
using Tracking.WebApp.Models;

namespace Tracking.WebApp.Controllers
{
    public class IssuesController : Controller
    {
        HttpClient client = new HttpClient();

        private readonly TrackingWebAppContext _context;

        public IssuesController(TrackingWebAppContext context)
        {
            _context = context;
        }

        // GET: Issues
        public async Task<IActionResult> Index()
        {
            client.BaseAddress = new Uri("https://localhost:7071");

            HttpResponseMessage response = await client.GetAsync("api/issue/All");
            response.EnsureSuccessStatusCode();
            var issues = await response.Content.ReadFromJsonAsync<IEnumerable<Issue>>();

            return View(issues);
        }

        // GET: Issues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Issue == null)
            {
                return NotFound();
            }
            client.BaseAddress = new Uri("https://localhost:7071");

            HttpResponseMessage response = await client.GetAsync($"api/issue/{id}");
            response.EnsureSuccessStatusCode();
            var issue = await response.Content.ReadFromJsonAsync<Issue>();
            /*
            var issue = await _context.Issue
                .FirstOrDefaultAsync(m => m.Id == id);*/
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        // GET: Issues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Issues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Priority,IssueType,Created,Completed")] Issue issue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(issue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(issue);
        }

        // GET: Issues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Issue == null)
            {
                return NotFound();
            }

            var issue = await _context.Issue.FindAsync(id);
            if (issue == null)
            {
                return NotFound();
            }
            return View(issue);
        }

        // POST: Issues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Priority,IssueType,Created,Completed")] Issue issue)
        {
            if (id != issue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(issue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssueExists(issue.Id))
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
            return View(issue);
        }

        // GET: Issues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Issue == null)
            {
                return NotFound();
            }

            var issue = await _context.Issue
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Issue == null)
            {
                return Problem("Entity set 'TrackingWebAppContext.Issue'  is null.");
            }
            var issue = await _context.Issue.FindAsync(id);
            if (issue != null)
            {
                _context.Issue.Remove(issue);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IssueExists(int id)
        {
          return _context.Issue.Any(e => e.Id == id);
        }
    }
}

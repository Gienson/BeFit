using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;

namespace BeFit.Controllers
{
    [Authorize]
    public class SessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

   
            return View(await _context.Session
                .Where(s => s.UserId == userId)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var session = await _context.Session
                .FirstOrDefaultAsync(m => m.Id == id);

            if (session == null) return NotFound();
            if (session.UserId != userId) return Forbid();

            return View(session);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Start,End")] Session session)
        {
            if (ModelState.IsValid)
            {
                session.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.Add(session);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var session = await _context.Session.FindAsync(id);

            if (session == null) return NotFound();

            if (session.UserId != userId) return Forbid();

            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Start,End")] Session session)
        {
            if (id != session.Id) return NotFound();


            var existingSession = await _context.Session.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (existingSession == null || existingSession.UserId != userId)
            {
                return Forbid();
            }

            session.UserId = userId;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(session);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionExists(session.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var session = await _context.Session
                .FirstOrDefaultAsync(m => m.Id == id);

            if (session == null) return NotFound();
            if (session.UserId != userId) return Forbid();

            return View(session);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var session = await _context.Session.FindAsync(id);

            if (session != null)
            {
                if (session.UserId == userId)
                {
                    _context.Session.Remove(session);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SessionExists(int id)
        {
            return _context.Session.Any(e => e.Id == id);
        }
    }
}
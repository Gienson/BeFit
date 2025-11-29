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
    public class ExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExercisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exercises
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var applicationDbContext = _context.Exercise
                .Include(e => e.ExerciseType)
                .Include(e => e.Session)
                .Where(e => e.Session.UserId == userId);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Exercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var exercise = await _context.Exercise
                .Include(e => e.ExerciseType)
                .Include(e => e.Session)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exercise == null) return NotFound();

            if (exercise.Session.UserId != userId) return Forbid();

            return View(exercise);
        }

        // GET: Exercises/Create
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseType, "Id", "Name");
            ViewData["SessionId"] = new SelectList(_context.Session.Where(s => s.UserId == userId), "Id", "Start");

            return View();
        }

        // POST: Exercises/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Weight,NumOfSeries,NumOfReps,ExerciseTypeId,SessionId")] Exercise exercise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseType, "Id", "Name", exercise.ExerciseTypeId);
            ViewData["SessionId"] = new SelectList(_context.Session.Where(s => s.UserId == userId), "Id", "Start", exercise.SessionId);

            return View(exercise);
        }

        // GET: Exercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var exercise = await _context.Exercise
                                .Include(e => e.Session)
                                .FirstOrDefaultAsync(e => e.Id == id);

            if (exercise == null) return NotFound();

            if (exercise.Session.UserId != userId) return Forbid();

            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseType, "Id", "Name", exercise.ExerciseTypeId); // Zmieniłem "Id" na "Name" dla czytelności
            ViewData["SessionId"] = new SelectList(_context.Session.Where(s => s.UserId == userId), "Id", "Start", exercise.SessionId);
            return View(exercise);
        }

        // POST: Exercises/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Weight,NumOfSeries,NumOfReps,ExerciseTypeId,SessionId")] Exercise exercise)
        {
            if (id != exercise.Id) return NotFound();


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseExists(exercise.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseType, "Id", "Name", exercise.ExerciseTypeId);
            ViewData["SessionId"] = new SelectList(_context.Session.Where(s => s.UserId == userId), "Id", "Start", exercise.SessionId);
            return View(exercise);
        }

        // GET: Exercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var exercise = await _context.Exercise
                .Include(e => e.ExerciseType)
                .Include(e => e.Session)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exercise == null) return NotFound();
            if (exercise.Session.UserId != userId) return Forbid();

            return View(exercise);
        }

        // POST: Exercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercise = await _context.Exercise.FindAsync(id);
            if (exercise != null)
            {
                _context.Exercise.Remove(exercise);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseExists(int id)
        {
            return _context.Exercise.Any(e => e.Id == id);
        }

        // GET: Exercises/Stats
        public async Task<IActionResult> Stats()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentDate = DateTime.Now;
            var fourWeeksAgo = currentDate.AddDays(-28);

            var exercises = await _context.Exercise
                .Include(e => e.ExerciseType)
                .Include(e => e.Session)
                .Where(e => e.Session.UserId == userId)
                .Where(e => e.Session.Start >= fourWeeksAgo)
                .ToListAsync();

            var exerciseStats = exercises
                .GroupBy(e => e.ExerciseTypeId)
                .Select(group => new Stat
                {
                    ExerciseTypeName = group.FirstOrDefault()?.ExerciseType?.Name ?? "Nieznane",
                    SessionsCount = group.Count(),
                    TotalReps = group.Sum(e => e.NumOfSeries * e.NumOfReps),
                    AvgWeight = Math.Round(group.Average(e => e.Weight), 1),
                    MaxWeight = group.Max(e => e.Weight)
                })
                .ToList();

            return View(exerciseStats);
        }
    }
}
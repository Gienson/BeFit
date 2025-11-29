using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;

namespace BeFit.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var today = DateTime.Now;
            var fourWeeksAgo = today.AddDays(-28);

            var exercises = await _context.Exercise
                .Include(e => e.ExerciseType)
                .Include(e => e.Session)
                .Where(e => e.Session.UserId == userId)
                .Where(e => e.Session.Start >= fourWeeksAgo)
                .ToListAsync();

            var stats = exercises
                .GroupBy(e => e.ExerciseType.Name)
                .Select(g => new Stat
                {
                    ExerciseTypeName = g.Key,
                    SessionsCount = g.Count(),
                    TotalReps = g.Sum(e => e.NumOfSeries * e.NumOfReps),
                    AvgWeight = Math.Round(g.Average(e => e.Weight), 1),
                    MaxWeight = g.Max(e => e.Weight)
                })
                .ToList();

            return View(stats);
        }
    }
}
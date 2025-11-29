using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class Stat
    {
        public string ExerciseTypeName { get; set; }
        public int SessionsCount { get; set; }
        public int BestResult { get; set; }
    }
}
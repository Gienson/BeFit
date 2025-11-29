using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class Stat
    {
        [Display(Name = "Rodzaj ćwiczenia")]
        public string ExerciseTypeName { get; set; }

        [Display(Name = "Liczba treningów")]
        public int SessionsCount { get; set; }

        [Display(Name = "Łączna liczba powtórzeń")]
        public int TotalReps { get; set; }

        [Display(Name = "Średnie obciążenie")]
        public double AvgWeight { get; set; }

        [Display(Name = "Maksymalne obciążenie")]
        public int MaxWeight { get; set; }
    }
}
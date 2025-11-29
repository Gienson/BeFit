using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        [Display(Name = "Waga obciążenia (kg)")]
        [Range(0.5, 450, ErrorMessage = "Dopuszczalna waga obciążenia wynosi od 0.5 do 450kg.")]
        public int Weight { get; set; }
        [Display(Name = "Liczba serii")]
        [Range(1, 10, ErrorMessage = "Liczba serii przyjmuje wartości od 1 do 10.")]
        public int NumOfSeries { get; set; }
        [Display(Name = "Liczba powtórzeń")]
        [Range(1, 30, ErrorMessage = "Liczba powtórzeń przyjmuje wartości od 1 do 30.")]
        public int NumOfReps { get; set; }
        [Display(Name = "Rodzaj ćwiczenia")]
        public int ExerciseTypeId { get; set; }
        [Display(Name = "Rodzaj ćwiczenia")]
        public virtual ExerciseType? ExerciseType { get; set; }
        [Display(Name = "Sesja")]
        public int SessionId { get; set; }
        [Display(Name = "Sesja")]
        public virtual Session? Session { get; set;}
    }
}

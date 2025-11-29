using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class ExerciseType
    {
        public int Id { get; set; }
        [Display(Name = "Rodzaj ćwiczenia")]
        [StringLength(50, ErrorMessage = "Nazwa ćwiczenie nie może przekroczyć 50 znaków")]
        public string Name { get; set; }
    }
}

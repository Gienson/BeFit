using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class Session
    {
        public int Id { get; set; }
        [Display(Name = "Data i godzina rozpoczęcia sesji")]
        public DateTime Start {  get; set; }
        [Display(Name = "Data i godzina zakończenia sesji")]
        public DateTime End { get; set; }
    }
}

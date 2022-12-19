using System.ComponentModel.DataAnnotations;

namespace VacationRental.Api.BindingModels
{
    public class RentalBindingModel
    {
        [Required]
        [Range(1, 99999999999)]
        public int Units { get; set; }
    }
}

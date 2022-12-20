namespace VacationRental.Domain.ViewModels.Request.Rental
{
    public class PutRentalViewModel
    {
        public int Id { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.BindingModels;
using VacationRental.Domain.ViewModels;
using Xunit;

namespace VacationRental.Api.Tests
{
    public class SharedRequest
    {
        private readonly HttpClient _client;

        public SharedRequest(HttpClient client) => _client = client;

        public async Task<HttpResponseMessage> PostRental(int unit, int preparationTimeInDays)
        {
            var postRentalRequest = new PostRentalBindingModel
            {
                Units = unit,
                PreparationTimeInDays = preparationTimeInDays
            };

            return await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest);
        }

        public async Task<HttpResponseMessage> PutRental(int id, int unit, int preparationTimeInDays)
        {
            var putRentalRequest = new PutRentalBindingModel
            {
                Id = id,
                Units = unit,
                PreparationTimeInDays = preparationTimeInDays
            };

            return await _client.PutAsJsonAsync($"/api/v1/rentals", putRentalRequest);
        }

        public async Task<HttpResponseMessage> PutRentalAssetingStatusOk(int id, int unit, int preparationTimeInDays)
        {
            using (var resp = await PutRental(id, unit, preparationTimeInDays))
            {
                Assert.True(resp.IsSuccessStatusCode);
                return resp;
            }
        }

        public async Task<HttpResponseMessage> PostBooking(int rentalId, DateTime start, int night)
        {
            var postBookingRequest = new BookingBindingModel
            {
                RentalId = rentalId,
                Nights = night,
                Start = start
            };

            return await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest);
        }

        public async Task<HttpResponseMessage> PostBookingAssetingStatusOk(int rentalId, DateTime start, int night)
        {
            using (var resp = await PostBooking(rentalId, start, night))
            {
                Assert.True(resp.IsSuccessStatusCode);
                return resp;
            }
        }

        public async Task<HttpResponseMessage> GetRentalById(int rentalId)
        {
            return await _client.GetAsync($"/api/v1/rentals/{rentalId}");
        }

        public async Task<HttpResponseMessage> GetCalendar(int rentalId, string start, int night)
        {
            return await _client.GetAsync($"/api/v1/calendar?rentalId={rentalId}&start={start}&nights={night}");
        }

    }
}

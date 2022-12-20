using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.BindingModels;
using VacationRental.Domain.ViewModels;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostBookingTests
    {
        private readonly HttpClient _client;
        private readonly SharedRequest _requests;

        public PostBookingTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
            _requests = new SharedRequest(_client);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            int unitRequested = 3;

            ResourceIdViewModel postRentalResult;
            using (var resp = await _requests.PostRental(3, 1))
            {
                Assert.True(resp.IsSuccessStatusCode);
                postRentalResult = await resp.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            ResourceIdViewModel postBookingResult;
            using (var resp = await _requests.PostBooking(postRentalResult.Id, new DateTime(2001, 01, 01), unitRequested))
            {
                Assert.True(resp.IsSuccessStatusCode);
                postBookingResult = await resp.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);

                var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
                Assert.Equal(postRentalResult.Id, getBookingResult.RentalId);
                Assert.Equal(unitRequested, getBookingResult.Nights);
                Assert.Equal(new DateTime(2001, 01, 01), getBookingResult.Start);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
        {
            ResourceIdViewModel postRentalResult;
            using (var resp = await _requests.PostRental(1, 1))
            {
                Assert.True(resp.IsSuccessStatusCode);
                postRentalResult = await resp.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            ResourceIdViewModel postBookingResult;
            using (var resp = await _requests.PostBooking(postRentalResult.Id, new DateTime(2001, 01, 01), 3))
            {
                Assert.True(resp.IsSuccessStatusCode);
                postBookingResult = await resp.Content.ReadAsAsync<ResourceIdViewModel>();
            }
             
            using (var resp = await _requests.PostBooking(postRentalResult.Id, new DateTime(2001, 01, 02), 1))
            {
                string contentBody = await resp.Content.ReadAsStringAsync();
                Assert.True(resp.StatusCode == HttpStatusCode.PreconditionFailed);
                Assert.Contains("available", contentBody);
            } 
        }
    }
}

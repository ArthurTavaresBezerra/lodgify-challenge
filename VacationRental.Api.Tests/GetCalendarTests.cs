using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.BindingModels;
using VacationRental.Domain.ViewModels;
using VacationRental.Domain.ViewModels.Response.Calendar;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class GetCalendarTests
    {
        private readonly HttpClient _client;
        private readonly SharedRequest _requests;

        public GetCalendarTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
            _requests = new SharedRequest(_client);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _requests.PostRental(2, 1))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            ResourceIdViewModel postBooking1Result;
            using (var resp = await _requests.PostBooking(postRentalResult.Id, new DateTime(2000, 01, 02), 2))
            {
                Assert.True(resp.IsSuccessStatusCode);
                postBooking1Result = await resp.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            ResourceIdViewModel postBooking2Result;
            using (var resp = await _requests.PostBooking(postRentalResult.Id, new DateTime(2000, 01, 03), 2))
            {
                Assert.True(resp.IsSuccessStatusCode);
                postBooking2Result = await resp.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getCalendarResponse = await _requests.GetCalendar(postRentalResult.Id, "2000-01-01", 5))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<GetCalendarViewModel>();

                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(5, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2000, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.Empty(getCalendarResult.Dates[0].Bookings);

                Assert.Equal(new DateTime(2000, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);

                Assert.Equal(new DateTime(2000, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);

                Assert.Equal(new DateTime(2000, 01, 04), getCalendarResult.Dates[3].Date);
                Assert.Single(getCalendarResult.Dates[3].Bookings);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result.Id);

                Assert.Equal(new DateTime(2000, 01, 05), getCalendarResult.Dates[4].Date);
                Assert.Empty(getCalendarResult.Dates[4].Bookings);
            }
        }

         
    }
}

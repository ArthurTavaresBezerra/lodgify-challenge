using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.BindingModels;
using VacationRental.Domain.ViewModels;
using VacationRental.Domain.ViewModels.Response.Rental;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PutRentalTests
    {
        private readonly HttpClient _client;
        private readonly SharedRequest _requests;

        public PutRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
            _requests = new SharedRequest(_client);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenUpdateUnitsToMore()
        {
            int unitsRequested = 2;
            int ppTimeRequested = 1;

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _requests.PostRental(unitsRequested, ppTimeRequested))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            await _requests.PostBookingAssetingStatusOk(postRentalResult.Id, new DateTime(2001, 01, 01), 2);
            await _requests.PostBookingAssetingStatusOk(postRentalResult.Id, new DateTime(2001, 01, 01), 2);
            await _requests.PostBookingAssetingStatusOk(postRentalResult.Id, new DateTime(2001, 01, 04), 1);

            await _requests.PutRentalAssetingStatusOk(postRentalResult.Id, unitsRequested + 1, ppTimeRequested);

            using (var resp = await _requests.GetRentalById(postRentalResult.Id))
            {
                Assert.True(resp.IsSuccessStatusCode);
                var getResult = await resp.Content.ReadAsAsync<GetRentalViewModel>();
                Assert.Equal(unitsRequested + 1, getResult.Units);
                Assert.Equal(ppTimeRequested, getResult.PreparationTimeInDays);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenUpdateUnitsToLessAndOverlapping()
        {
            int unitsRequested = 2;
            int ppTimeRequested = 1;

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _requests.PostRental(unitsRequested, ppTimeRequested))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            await _requests.PostBookingAssetingStatusOk(postRentalResult.Id, new DateTime(2001, 01, 01), 2);
            await _requests.PostBookingAssetingStatusOk(postRentalResult.Id, new DateTime(2001, 01, 01), 2);
            await _requests.PostBookingAssetingStatusOk(postRentalResult.Id, new DateTime(2001, 01, 04), 1);
             
            using (var resp = await _requests.PutRental(postRentalResult.Id, unitsRequested - 1, ppTimeRequested))
            {
                string contentBody = await resp.Content.ReadAsStringAsync();
                Assert.True(resp.StatusCode == HttpStatusCode.PreconditionFailed);
                Assert.Contains("Overlapping", contentBody);
            }
        }


        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenUpdatePreparationTimeToLess()
        {
            int unitsRequested = 2;
            int ppTimeRequested = 2;

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _requests.PostRental(unitsRequested, ppTimeRequested))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            await _requests.PostBookingAssetingStatusOk(postRentalResult.Id, new DateTime(2001, 01, 01), 2);
            await _requests.PostBookingAssetingStatusOk(postRentalResult.Id, new DateTime(2001, 01, 01), 2);
            await _requests.PostBookingAssetingStatusOk(postRentalResult.Id, new DateTime(2001, 01, 05), 1);

            await _requests.PutRentalAssetingStatusOk(postRentalResult.Id, unitsRequested, ppTimeRequested-1);

            using (var resp = await _requests.GetRentalById(postRentalResult.Id))
            {
                Assert.True(resp.IsSuccessStatusCode);
                var getResult = await resp.Content.ReadAsAsync<GetRentalViewModel>();
                Assert.Equal(unitsRequested, getResult.Units);
                Assert.Equal(ppTimeRequested-1, getResult.PreparationTimeInDays);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenUpdatePreparationTimeToMoreAndOverlapping()
        {
            int unitsRequested = 2;
            int ppTimeRequested = 2;

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _requests.PostRental(unitsRequested, ppTimeRequested))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            await _requests.PostBookingAssetingStatusOk(postRentalResult.Id, new DateTime(2001, 01, 01), 2);
            await _requests.PostBookingAssetingStatusOk(postRentalResult.Id, new DateTime(2001, 01, 01), 2);
            await _requests.PostBookingAssetingStatusOk(postRentalResult.Id, new DateTime(2001, 01, 05), 1);
             
            using (var resp = await _requests.PutRental(postRentalResult.Id, unitsRequested - 1, ppTimeRequested))
            {
                string contentBody = await resp.Content.ReadAsStringAsync();
                Assert.True(resp.StatusCode == HttpStatusCode.PreconditionFailed);
                Assert.Contains("Overlapping", contentBody);
            }
        }
    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.BindingModels;
using VacationRental.Domain.ViewModels;
using VacationRental.Domain.ViewModels.Request.Rental;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostRentalTests
    {
        private readonly HttpClient _client;
        private readonly SharedRequest _requests;

        public PostRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
            _requests = new SharedRequest(_client);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            int unitsRequested = 25;
            int ppTimeRequested = 1;

            ResourceIdViewModel postResult;
            using (var postResponse = await _requests.PostRental(unitsRequested, ppTimeRequested))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _requests.GetRentalById(postResult.Id))
            {
                Assert.True(getResponse.IsSuccessStatusCode);
                var getResult = await getResponse.Content.ReadAsAsync<PostRentalViewModel>();
                Assert.Equal(unitsRequested, getResult.Units);
                Assert.Equal(ppTimeRequested, getResult.PreparationTimeInDays);
            }
        }
    }
}

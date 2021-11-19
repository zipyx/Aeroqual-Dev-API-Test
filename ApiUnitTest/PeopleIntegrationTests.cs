using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryAPI
{
    public class PeopleIntegrationTests : IClassFixture<WebApplicationFactory<ApiTest.Startup>>
    {
        private readonly WebApplicationFactory<ApiTest.Startup> _factory;

        public PeopleIntegrationTests(WebApplicationFactory<ApiTest.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/people")]
        [InlineData("/api/people/12")]
        [InlineData("/api/people/search?name=matt")]
        public async Task Get_EndpointsReturnPeopleSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}

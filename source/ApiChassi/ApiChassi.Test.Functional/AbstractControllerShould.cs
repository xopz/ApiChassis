using ApiChassi.WebApi;
using ApiChassi.WebApi.V1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace ApiChassi.Test.Functional
{
    public class AbstractControllerShould
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public AbstractControllerShould()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task ReturnARecordWhenItExists()
        {
            //act
            var expectedRecord = new SampleModel
            {
                Id = Guid.NewGuid(),
                Description = "A sample"
            };
            var response = await _client.GetAsync($"/v1/sample/{expectedRecord.Id}");
            response.EnsureSuccessStatusCode();
            var actualRecord = await response.Content.ReadFromJsonAsync<SampleModel>();
            //assert
            Assert.Equal(expectedRecord.Id, actualRecord.Id);
            Assert.Equal(expectedRecord.Description, actualRecord.Description);
        }
    }
}

using ApiChassi.WebApi.V1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Xunit;
using api = ApiChassi.WebApi.V1.Controllers;

namespace ApiChassi.Test.Unit.Controllers
{
    public class SampleController
    {
        public SampleController()
        {
        }

        [Fact]
        public async Task GettingARecordWithExistingIdShouldReturnTheRecord()
        {
            //arrange
            var expectedRecord = new SampleModel
            {
                Id = Guid.NewGuid(),
                Description = "A sample"
            };
            var controller = new api.SampleController();
            //act
            var result = await controller.GetItem(ApiVersion.Default, expectedRecord.Id);
            //assert
            Assert.IsType<OkObjectResult>(result.Result);
            var resultingRecord = ((result.Result as OkObjectResult).Value as SampleModel);
            Assert.Equal(expectedRecord.Id, resultingRecord.Id);
            Assert.Equal(expectedRecord.Description, resultingRecord.Description);
        }
    }
}

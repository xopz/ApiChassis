using ApiChassi.Test.Unit.Controllers.Stubs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ApiChassi.Test.Unit.Controllers
{
    public class BaseControllerShould
    {
        public BaseControllerShould() { }

        [Fact]
        public async Task ReturnACollectionOfNone() {
            //arrange
            var controller = new StubController();
            //act
            var response = await controller.GetList(ApiVersion.Default, new StubRequest());
            //asert
            Assert.IsType<OkObjectResult>(response.Result);
            var resultingRecords = ((response.Result as ObjectResult).Value as IEnumerable<StubModel>);
            Assert.Empty(resultingRecords);
        }

        [Fact]
        public async Task ReturnBadRequestWhenMissingRequiredField()
        {
            //arrange
            var controller = new StubController();
            var wrongRequest = new StubModel();
            //act
            var response = await controller.Post(ApiVersion.Default, wrongRequest);
            //assert
            Assert.IsType<BadRequestObjectResult>(response.Result);
        }

        [Fact]
        public async Task ReturnCreatedWhenRunSuccessfully()
        {
            //arrange
            var lorem = new Bogus.DataSets.Lorem();
            var controller = new StubController();
            var request = new StubModel
            {
                Id = Guid.NewGuid(),
                Description = lorem.Sentence(3)
            };
            //act
            var response = await controller.Post(ApiVersion.Default, request);
            //assert
            Assert.IsType<CreatedResult>(response.Result);
            var result = (response.Result as CreatedResult).Value as StubModel;
            Assert.Equal(request.Id, result.Id);
            Assert.Equal(request.Description, result.Description);
        }

        [Fact]
        public async Task ReturnNoutFoundWhenLookingforUnexistentRecord() { }

        [Fact]
        public async Task ReturnTheRecordWhenLookingforAValidId() { }

        [Fact]
        public async Task ReturnACollectionOfOne() { }


    }
}

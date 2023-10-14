using ApiChassi.Test.Unit.Controllers.Stubs;
using Bogus.DataSets;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ApiChassi.Test.Unit.Controllers
{
    public class BaseControllerShould
    {
        private StubController _controller;
        private Guid _id;
        private Lorem _lorem;

        public BaseControllerShould()
        {
            _controller = new StubController();
            _id = Guid.NewGuid();
            _lorem = new Lorem();
        }

        [Fact]
        public async Task ReturnACollectionOfNone() {
            //act
            var response = await _controller.GetList(ApiVersion.Default, new StubRequest());
            //asert
            Assert.IsType<OkObjectResult>(response.Result);
            var resultingRecords = ((response.Result as ObjectResult).Value as IEnumerable<StubModel>);
            Assert.Empty(resultingRecords);
        }

        [Fact]
        public async Task ReturnBadRequestWhenPostingNull()
        {
            //act
            var response = await _controller.Post(ApiVersion.Default, null);
            //assert
            Assert.IsType<BadRequestObjectResult>(response.Result);
        }

        [Fact]
        public async Task ReturnCreatedWhenPostRunSuccessfully()
        {
            //arrange
            var request = new StubModel
            {
                Id = _id,
                Description = _lorem.Sentence(3)
            };
            //act
            var response = await _controller.Post(ApiVersion.Default, request);
            //assert
            Assert.IsType<CreatedResult>(response.Result);
            var result = (response.Result as CreatedResult).Value as StubModel;
            Assert.Equal(request.Id, result.Id);
            Assert.Equal(request.Description, result.Description);
        }

        [Fact]
        public async Task ReturnNoutFoundWhenLookingforUnexistentRecord()
        {
            //arrange
            await _controller.Post(ApiVersion.Default, new StubModel
            {
                Id = _id,
                Description = _lorem.Sentence(2)
            });
            //act
            var response = await _controller.GetItem(ApiVersion.Default, Guid.Empty);
            //assert
            Assert.IsType<NotFoundObjectResult>(response.Result);
        }

        [Fact]
        public async Task ReturnTheRecordWhenLookingforAValidId()
        {
            //arrange
            await _controller.Post(ApiVersion.Default, new StubModel
            {
                Id = _id,
                Description = _lorem.Sentence(2)
            });
            //act
            var response = await _controller.GetItem(ApiVersion.Default, _id);
            //assert
            Assert.IsType<OkObjectResult>(response.Result);
        }

        [Fact]
        public async Task ReturnACollectionOfOne()
        {
            //arrange
            await _controller.Post(ApiVersion.Default, new StubModel
            {
                Id = _id,
                Description = _lorem.Sentence(2)
            });
            //act
            var response = await _controller.GetList(ApiVersion.Default, new StubRequest());
            //assert
            Assert.IsType<OkObjectResult>(response.Result);
            var resultingRecords = ((response.Result as ObjectResult).Value as IEnumerable<StubModel>);
            Assert.Single(resultingRecords);
        }

        [Fact]
        public async Task ReturnBadRequestWhenPuttingNull()
        {
            //act
            var response = await _controller.Put(ApiVersion.Default, Guid.Empty, null);
            //assert
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task ReturnBadRequestWhenPuttingNoMatchingId()
        {
            //arrange
            var request = new StubModel
            {
                Id = _id,
                Description = _lorem.Sentence(3)
            };
            //act
            var response = await _controller.Put(ApiVersion.Default, Guid.Empty, request);
            //assert
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task ReturnNotFoundWhenPuttingUnexistingId()
        {
            //arrange
            var request = new StubModel
            {
                Id = _id,
                Description = _lorem.Sentence(3)
            };
            await _controller.Post(ApiVersion.Default, request);
            var updatedId = Guid.NewGuid();
            var updateRequest = new StubModel
            {
                Id = updatedId,
                Description = _lorem.Sentence(3)
            };
            //act
            var response = await _controller.Put(ApiVersion.Default, updatedId, updateRequest);
            //assert
            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact]
        public async Task ReturnNoContentWhenRunPutSuccessfully()
        {
            //arrange
            var request = new StubModel
            {
                Id = _id,
                Description = _lorem.Sentence(3)
            };
            await _controller.Post(ApiVersion.Default, request);
            //act
            var response = await _controller.Put(ApiVersion.Default, _id, request);
            //assert
            Assert.IsType<NoContentResult>(response);
        }

        [Fact]
        public async Task ReturnNotFoundWhenDeletingUnexistingId()
        {
            //arrange
            var request = new StubModel
            {
                Id = _id,
                Description = _lorem.Sentence(3)
            };
            await _controller.Post(ApiVersion.Default, request);
            //act
            var response = await _controller.Delete(ApiVersion.Default, Guid.NewGuid());
            //assert
            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact]
        public async Task ReturnNoContentWhenRunDeleteSuccessfully()
        {
            //arrange
            var request = new StubModel
            {
                Id = _id,
                Description = _lorem.Sentence(3)
            };
            await _controller.Post(ApiVersion.Default, request);
            //act
            var response = await _controller.Delete(ApiVersion.Default, _id);
            //assert
            Assert.IsType<NoContentResult>(response);
        }
    }
}

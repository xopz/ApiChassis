using ApiChassi.WebApi.V1.Controllers;
using ApiChassi.WebApi.V1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ApiChassi.Test.Unit.Controllers
{
    public class SampleControllerShould
    {
        private SampleController _controller;

        public SampleControllerShould()
        {
            _controller = new SampleController();
        }

        [Fact]
        public async Task ReturnTheRecordWhenGettingARecordWithExistingId()
        {
            //arrange
            var expectedRecord = new SampleModel
            {
                Id = Guid.NewGuid(),
                Description = "A sample"
            };
            //act
            var response = await _controller.GetItem(ApiVersion.Default, expectedRecord.Id);
            //assert
            Assert.IsType<OkObjectResult>(response.Result);
            var resultingRecord = ((response.Result as OkObjectResult).Value as SampleModel);
            Assert.Equal(expectedRecord.Id, resultingRecord.Id);
            Assert.Equal(expectedRecord.Description, resultingRecord.Description);
        }

        [Fact]
        public async Task ReturnAListOfRecordsWhenGettingAList()
        {
            //act
            var response = await _controller.GetList(ApiVersion.Default, new SampleSearchRequestModel());
            //assert
            Assert.IsType<OkObjectResult>(response.Result);
            var resultingRecords = (response.Result as OkObjectResult).Value as IEnumerable<SampleModel>;
            Assert.Single(resultingRecords);
        }

        [Fact]
        public async Task ReturnTheCreatedRecordWhenPosting()
        {
            //arrange
            var expectedRecord = new SampleModel
            {
                Id = Guid.NewGuid(),
                Description = "A sample"
            };
            //act
            var response = await _controller.Post(ApiVersion.Default, expectedRecord);
            //assert
            Assert.IsType<CreatedResult>(response.Result);
            var resultingRecord = ((response.Result as CreatedResult).Value as SampleModel);
            Assert.Equal(expectedRecord.Id, resultingRecord.Id);
            Assert.Equal(expectedRecord.Description, resultingRecord.Description);
        }

        [Fact]
        public async Task ReturnNoContentWhenPuttingValidData()
        {
            //arrage
            var id = Guid.NewGuid();
            var record = new SampleModel { Id = id, Description = "A description" };
            await _controller.Post(ApiVersion.Default, record);
            //act
            var result = await _controller.Put(ApiVersion.Default, id, record);
            //assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task ReturnNoContentWhenDeletingValidRecord()
        {
            //arrage
            var id = Guid.NewGuid();
            var record = new SampleModel { Id = id, Description = "A description" };
            await _controller.Post(ApiVersion.Default, record);
            //act
            var result = await _controller.Delete(ApiVersion.Default, id);
            //assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}

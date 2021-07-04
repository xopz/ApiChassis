using ApiChassi.WebApi.Shared.Controllers;
using ApiChassi.WebApi.Shared.Models;
using ApiChassi.WebApi.Shared.Models.Request.Interfaces;
using ApiChassi.WebApi.Shared.Models.Response.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiChassi.Test.Unit.Controllers.Stubs
{
    class StubModel : IPostResponseModel, IPutRequestModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Description { get; set; }
    }

    class StubRequest : ISearchRequestModel
    {
        public uint Offset { get; set; }
        public ushort Limit { get; set; }
        public string Order { get; set; }
        public string Fields { get; set; }
        public string Description { get; set; }
    }

    class StubController : BaseCrudController<StubModel, StubRequest>
    {
        private readonly List<StubModel> _collection;

        public StubController()
        {
            _collection = new List<StubModel>();
        }

        protected override Task<StubModel> CreateAsync(StubModel request)
        {
            _collection.Add(request);
            return Task.FromResult(request);
        }

        protected override Task DeleteAsync(StubModel request)
        {
            var index = _collection.FindIndex(stub => stub.Id == request.Id);
            _collection.RemoveAt(index);
            return Task.CompletedTask;
        }

        protected override Task<bool> ExistsAsync(Guid id)
        {
            return Task.FromResult(_collection.FindIndex(stub => stub.Id == id) >= 0);
        }

        protected override Task<SearchResult<StubModel>> FindAsync(StubRequest request)
        {
            return Task.FromResult(new SearchResult<StubModel>(_collection, (uint)_collection.Count));
        }

        protected override Task<StubModel> GetAsync(Guid id)
        {
            return Task.FromResult(_collection.FirstOrDefault(stub => stub.Id == id));
        }

        protected override Task UpdateAsync(StubModel request)
        {
            var index = _collection.FindIndex(stub => stub.Id == request.Id);
            _collection[index] = request;
            return Task.CompletedTask;
        }
    }
}

using ApiChassi.WebApi.Shared.Controllers;
using ApiChassi.WebApi.Shared.Models;
using ApiChassi.WebApi.V1.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ApiChassi.WebApi.V1.Controllers
{
    public class SampleController : BaseCrudController<SampleModel, SampleSearchRequestModel>
    {
        private bool IsError => RandomNumberGenerator.GetInt32(0, 10) == 1;

        protected override Task<SampleModel> CreateAsync(SampleModel request)
        {
            if (IsError) throw new Exception("A bug!");
            return Task.FromResult(request);
        }

        protected override Task DeleteAsync(SampleModel request)
        {
            if (IsError) throw new Exception("A bug!");
            return Task.Run(() => { });
        }

        protected override Task<bool> ExistsAsync(Guid id)
        {
            if (IsError) throw new Exception("A bug!");
            return Task.FromResult(true);
        }

        protected override Task<SearchResult<SampleModel>> FindAsync(SampleSearchRequestModel request)
        {
            if (IsError) throw new Exception("A bug!");
            var _data = new[] { new SampleModel { Id = new Guid(), Description = "A sample" } }.AsEnumerable();
            return Task.FromResult(new SearchResult<SampleModel>(_data));
        }

        protected override Task<SampleModel> GetAsync(Guid id)
        {
            if (IsError) throw new Exception("A bug!");
            return Task.FromResult(new SampleModel
            {
                Id = id,
                Description = "A sample"
            });
        }

        protected override Task UpdateAsync(SampleModel request)
        {
            if (IsError) throw new Exception("A bug!");
            return Task.Run(() => { });
        }
    }
}

namespace ApiChassi.WebApi.V1.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using ApiChassi.WebApi.Controllers;
    using ApiChassi.WebApi.Models;
    using ApiChassi.WebApi.V1.Models;

    /// <summary>
    /// 
    /// </summary>
    public class SampleController : BaseCRUDController<SampleModel, SampleFindRequestModel>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override Task<SampleModel> CreateAsync(SampleModel request)
        {
            return Task.FromResult(request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override Task DeleteAsync(SampleModel request)
        {
            return Task.Run(() => { });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override Task<bool> ExistsAsync(Guid id)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override Task<FindResult<SampleModel>> FindAsync(SampleFindRequestModel request)
        {
            return Task.FromResult(new FindResult<SampleModel>
            {
                Data = new[] { new SampleModel() }.AsEnumerable(),
                TotalCount = 1
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override Task<SampleModel> GetAsync(Guid id)
        {
            return Task.FromResult(new SampleModel());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override Task UpdateAsync(SampleModel request)
        {
            return Task.Run(() => { });
        }
    }
}

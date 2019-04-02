﻿namespace ApiChassi.WebApi.V1.Controllers
{
    using System;
    using System.Threading.Tasks;
    using ApiChassi.WebApi.Controllers;
    using ApiChassi.WebApi.V1.Models;

    public class SampleController : BaseCRUDController<SampleModel, SampleFindRequestModel>
    {
        protected override Task<SampleModel> CreateAsync(SampleModel request)
        {
            return Task.FromResult(request);
        }

        protected override Task DeleteAsync(SampleModel request)
        {
            return Task.Run(() => { });
        }

        protected override Task<bool> ExistsAsync(Guid id)
        {
            return Task.FromResult(true);
        }

        protected override Task<SampleModel> FindAsync(SampleFindRequestModel request)
        {
            return Task.FromResult(new SampleModel());
        }

        protected override Task<SampleModel> GetAsync(Guid id)
        {
            return Task.FromResult(new SampleModel());
        }

        protected override Task UpdateAsync(SampleModel request)
        {
            return Task.Run(() => { });
        }
    }
}

using ApiChassi.WebApi.Shared.Models.Request.Interfaces;
using ApiChassi.WebApi.Shared.Models.Response.Interfaces;
using System;

namespace ApiChassi.WebApi.V1.Models
{
    public class SampleModel : IPostResponseModel, IPutRequestModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}

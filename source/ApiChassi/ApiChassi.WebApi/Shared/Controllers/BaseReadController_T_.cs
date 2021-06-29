using ApiChassi.WebApi.Shared.Models;
using ApiChassi.WebApi.Shared.Models.Request.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ApiChassi.WebApi.Shared.Controllers
{
    public abstract class BaseReadController<TGetResponseModel, TSearchRequestModel, TSearchResponseModel> : BaseController
        where TGetResponseModel: class
        where TSearchResponseModel: class
        where TSearchRequestModel: ISearchRequestModel
    {

        protected abstract Task<TGetResponseModel> GetAsync(Guid id);

        protected abstract Task<SearchResult<TSearchResponseModel>> FindAsync(TSearchRequestModel request);

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<ActionResult<IEnumerable<TGetResponseModel>>> GetList(ApiVersion version, [FromQuery]TSearchRequestModel request)
        {
            var _result = await FindAsync(request);
            Response.Headers.Add("X-Total-Count", _result.TotalCount.ToString());
            return Ok(_result.Data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<ActionResult<TGetResponseModel>> GetItem(ApiVersion version, Guid id)
        {
            var _result = await GetAsync(id);
            if (_result == null)
            {
                return NotFound(new ProblemDetails
                {
                    Detail = $"Record with id {id} could not be found in app records.",
                    Title = "Record not found."
                });
            }
            return Ok(_result);
        }
    }

    public abstract class BaseReadController<TResponse, TSearchRequestModel> : BaseReadController<TResponse, TSearchRequestModel, TResponse>
        where TResponse : class
        where TSearchRequestModel : ISearchRequestModel
    { }
}

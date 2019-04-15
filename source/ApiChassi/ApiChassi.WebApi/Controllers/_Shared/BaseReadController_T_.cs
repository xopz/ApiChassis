namespace ApiChassi.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using ApiChassi.WebApi.Models.Request.Interfaces;
    using ApiChassi.WebApi.Models;

    public abstract class BaseReadController<TGetResponseModel, TFindRequestModel, TFindResponseModel> : BaseController
        where TGetResponseModel : class 
        where TFindRequestModel: IFindRequestModel
    {

        protected abstract Task<TGetResponseModel> GetAsync(Guid id);

        protected abstract Task<FindResult<TFindResponseModel>> FindAsync(TFindRequestModel request);

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <returns>The list.</returns>
        /// <param name="version">Version.</param>
        /// <param name="request">Request.</param>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<ActionResult<IEnumerable<TFindResponseModel>>> GetList(ApiVersion version, [FromQuery] TFindRequestModel request)
        {
            var _result = await FindAsync(request);
            Response.Headers.Add("X-Total-Count", _result.TotalCount.ToString());
            return Ok(_result.Data);
        }

        /// <summary>
        /// Gets an item.
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="version">Version.</param>
        /// <param name="id">Identifier.</param>
        [HttpGet("{id}", Name = "get-record")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<ActionResult<TGetResponseModel>> GetItem(ApiVersion version, Guid id)
        {
            var _result = await GetAsync(id);
            if (_result == null)
            {
                return NotFound();
            }
            return Ok(_result);
        }
    }

    public abstract class BaseReadController<TModel, TFindRequestModel> : BaseReadController<TModel, TFindRequestModel, TModel> 
        where TModel: class
        where TFindRequestModel: IFindRequestModel 
    { }
}

using ApiChassi.WebApi.Shared.Models;
using ApiChassi.WebApi.Shared.Models.Request.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ApiChassi.WebApi.Shared.Controllers
{
    /// <summary>
    /// A base controller with `get` and `search` operations throught GET methods
    /// </summary>
    /// <typeparam name="TGetResponseModel">The type of the `get` response</typeparam>
    /// <typeparam name="TSearchRequestModel">The type of the `search` resquest</typeparam>
    /// <typeparam name="TSearchResponseModel">The type of the `search` response</typeparam>
    public abstract class BaseReadController<TGetResponseModel, TSearchRequestModel, TSearchResponseModel> : BaseController
        where TGetResponseModel: class
        where TSearchResponseModel: class
        where TSearchRequestModel: ISearchRequestModel
    {
        /// <summary>
        /// An abstraction of an operation to retrieve a single record of data
        /// </summary>
        /// <param name="id">The record identification</param>
        /// <returns>The data mapped as the type expected as `get` response</returns>
        protected abstract Task<TGetResponseModel> GetAsync(Guid id);

        /// <summary>
        /// An abstraction of an operation to retrieve a recordset of data based on a query
        /// </summary>
        /// <param name="request">Parameters to query data</param>
        /// <returns>The recordset mapped as an structure expetect as `search` response</returns>
        protected abstract Task<SearchResult<TSearchResponseModel>> FindAsync(TSearchRequestModel request);

        /// <summary>
        /// Search data based on search parameters.
        /// </summary>
        /// <param name="version">Version of the api</param>
        /// <param name="request">Search parameters</param>
        /// <returns>Response payload with data retrieved from the query</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public virtual async Task<ActionResult<IEnumerable<TGetResponseModel>>> GetList(ApiVersion version, [FromQuery]TSearchRequestModel request)
        {
            var _result = await FindAsync(request);
            Response?.Headers?.Add("X-Total-Count", _result.TotalCount.ToString());
            return Ok(_result.Data);
        }

        /// <summary>
        /// Get data based on the record identification
        /// </summary>
        /// <param name="version">Version of the api</param>
        /// <param name="id">Record identification</param>
        /// <returns>Response payload with data retrieved</returns>
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

    /// <summary>
    /// A base controller with `get` and `search` operations throught GET methods
    /// </summary>
    /// <typeparam name="TResponse">The type of the `get` and `search` response</typeparam>
    /// <typeparam name="TSearchRequestModel">The type of the `search` resquest</typeparam>
    public abstract class BaseReadController<TResponse, TSearchRequestModel> : BaseReadController<TResponse, TSearchRequestModel, TResponse>
        where TResponse : class
        where TSearchRequestModel : ISearchRequestModel
    { }
}
